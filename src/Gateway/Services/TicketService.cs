﻿using Gateway.Dtos;
using Gateway.Enums;
using Kernel.AbstractClasses;
using Kernel.Exceptions;

namespace Gateway.Services
{
    public class TicketService : ClientServiceBase
    {
        protected override string BaseUri => "http://ticketservice:80";

        private readonly FlightService _flightService;

        private readonly PrivilegeService _privilegeService;

        public TicketService(PrivilegeService privilegeService, FlightService flightService)
        {
            _privilegeService = privilegeService;
            _flightService = flightService;
        }

        public async Task<PaginationModel<GetTicketDto>?> GetAllTicketsAsync(int page, int size, string userName)
        {
            var tickets = await Client.GetAsync<PaginationModel<TicketDto>>(BuildUri("api/v1/tickets"), null, new Dictionary<string, string>
            {
                { "page", page.ToString() },
                { "size", size.ToString() },
                { "userName", userName }
            });

            if (tickets is null)
            {
                throw new NotFoundException();
            }
            var result = new PaginationModel<GetTicketDto>()
            {
                Page = tickets.Page,
                TotalElements = tickets.TotalElements,
                PageSize = tickets.PageSize
            };
            try
            {
                foreach (TicketDto ticket in tickets.Items)
                {
                    var flights = await _flightService.GetAllAsync(1, 100);
                    var flight = flights.Items.FirstOrDefault(x => x.Flightnumber == ticket.Flightnumber);
                    result.Items.Add(new GetTicketDto()
                    {
                        Flightnumber = ticket.Flightnumber,
                        Ticketuid = ticket.Ticketuid,
                        ToAirport = flight.Toairport.Name,
                        FromAirport = flight.Fromairport.Name,
                        Date = flight.Datetime,
                        Id = ticket.Id,
                        Price = ticket.Price,
                        Status = ticket.Status,
                        Username = ticket.Username,
                    });
                }
            }
            catch
            {
                foreach (TicketDto ticket in tickets.Items)
                {
                    result.Items.Add(new GetTicketDto()
                    {
                        Flightnumber = ticket.Flightnumber,
                        Ticketuid = ticket.Ticketuid,
                        ToAirport = null,
                        FromAirport = null,
                        Date = null,
                        Id = ticket.Id,
                        Price = ticket.Price,
                        Status = ticket.Status,
                        Username = ticket.Username,
                    });
                }
            }

            return result;
        }

        public async Task<GetTicketDto?> GetTicketByUidAsync(string guid)
        {
            var ticket = await Client.GetAsync<TicketDto>(BuildUri("api/v1/tickets/" + guid));
            if (ticket is null)
            {
                throw new NotFoundException();
            }
            var result = new GetTicketDto();
            try
            {
                var flights = await _flightService.GetAllAsync(100, 1);
                var flight = flights.Items.FirstOrDefault(x => x.Flightnumber == ticket.Flightnumber);
                result = new GetTicketDto()
                {
                    Flightnumber = ticket.Flightnumber,
                    Ticketuid = ticket.Ticketuid,
                    ToAirport = flight.Toairport.Name,
                    FromAirport = flight.Fromairport.Name,
                    Date = flight.Datetime,
                    Id = ticket.Id,
                    Price = ticket.Price,
                    Status = ticket.Status,
                    Username = ticket.Username,
                };
            }
            catch
            {
                result = new GetTicketDto()
                {
                    Flightnumber = ticket.Flightnumber,
                    Ticketuid = ticket.Ticketuid,
                    ToAirport = null,
                    FromAirport = null,
                    Date = null,
                    Id = ticket.Id,
                    Price = ticket.Price,
                    Status = ticket.Status,
                    Username = ticket.Username,
                };
            }

            return result;
        }

        public async Task<UserDto?> GetUserInfoAsync(string userName)
        {
            var tickets = await GetAllTicketsAsync(1, 100, userName);
            var privelege = new PrivilegeDto();
            try
            {
                privelege = await _privilegeService.GetPrivilegeAsync(userName);
                if (privelege is null)
                {
                    throw new NotFoundException();
                }
            }
            catch
            {
                //ignore
            }
            
            return new UserDto()
            {
                Privilege = privelege,
                Tickets = tickets?.Items ?? new List<GetTicketDto>()
            };
        }

        public async Task<TicketDto?> CreateTicketAsync(TicketDto dto)
       => await Client.PostAsync<TicketDto, TicketDto>(BuildUri("api/v1/tickets"), dto);


        public async Task<TicketDto?> UpdateTicketAsync(string id, GetTicketDto dto)
        {
            var ticketDto = new TicketDto()
            {
                Id = dto.Id,
                Ticketuid = dto.Ticketuid,
                Username = dto.Username,
                Flightnumber = dto.Flightnumber,
                Price = dto.Price,
                Status = dto.Status
            };
            return await Client.PatchAsync<TicketDto, TicketDto>(BuildUri("api/v1/tickets/" + id), ticketDto);     
        }

        public async Task<TicketPurchaseDto> BuyTicketsAsync(string flightNumber, int price, bool paidFromBalance, string userName)
        {
            var flights = await _flightService.GetAllAsync(1, 100);
            var flight = flights.Items.FirstOrDefault(x => x.Flightnumber == flightNumber);
            if(flight == null)
            {
                throw new NotFoundException($"Полет с номером рейса {flightNumber} не найден");
            }
            var privilege = await _privilegeService.GetPrivilegeAsync(userName);
            var paidByMoney = 0;
            var paidByBonuses = 0;
            if (paidFromBalance)
            {
                paidByBonuses = privilege.Balance.Value <= flight.Price ? privilege.Balance.Value : flight.Price;
                paidByMoney = price - paidByBonuses;
                var ticket = new TicketDto
                {
                    Ticketuid = Guid.NewGuid(),
                    Flightnumber = flightNumber,
                    Price = flight.Price,
                    Status = "PAID",
                    Username = userName
                };

                var t = await CreateTicketAsync(ticket);
                var paidTicket = await GetTicketByUidAsync(ticket.Ticketuid.ToString());
                var history = new PrivilegeHistoryDto
                {
                    BalanceDiff = -paidByBonuses,
                    Datetime = DateTime.Now,
                    OperationType = "FILL_IN_BALANCE",
                    PrivilegeId = privilege.Id,
                    TicketUid = paidTicket.Ticketuid
                };
                privilege.Balance -= paidByBonuses; 
                await _privilegeService.UpdatePrivilegeAsync(privilege.Id, privilege);
                await _privilegeService.AddPrivilegeHistoryAsync(history);

                return new TicketPurchaseDto
                {
                    FlightNumber = flightNumber,
                    Price = price,
                    Status = "PAID",
                    TicketUid = paidTicket.Ticketuid,
                    Date = flight.Datetime,
                    UserName = userName,
                    PaidByBonuses = paidByBonuses,
                    PaidByMoney = paidByMoney,
                    Privilege = new PrivilegeTicketPurchaseDto
                    {
                        Balance = privilege.Balance.Value,
                        Status = privilege.Status
                    }
                };
            }
            else
            {
                paidByMoney = price;
                var ticket = new TicketDto
                {
                    Ticketuid = Guid.NewGuid(),
                    Flightnumber = flightNumber,
                    Price = flight.Price,
                    Status = "PAID",
                    Username = userName
                };
                var paidTicket = await CreateTicketAsync(ticket);
                var history = new PrivilegeHistoryDto
                {
                    BalanceDiff = price/10,
                    Datetime = DateTime.Now,
                    OperationType = "DEBIT_THE_ACCOUNT",
                    PrivilegeId = privilege.Id,
                    TicketUid = paidTicket.Ticketuid
                };
                privilege.Balance += price/10;
                await _privilegeService.UpdatePrivilegeAsync(privilege.Id, privilege);
                await _privilegeService.AddPrivilegeHistoryAsync(history);

                return new TicketPurchaseDto
                {
                    FlightNumber = flightNumber,
                    Price = price,
                    Status = "PAID",
                    TicketUid = paidTicket.Ticketuid,
                    Date = flight.Datetime,
                    UserName = userName,
                    PaidByBonuses = paidByBonuses,
                    PaidByMoney = paidByMoney,
                    Privilege = new PrivilegeTicketPurchaseDto
                    {
                        Balance = privilege.Balance.Value,
                        Status = privilege.Status
                    }
                };
            }

        }

        public async Task ReturnTicketsAsync(string ticketUid, string userName, Queue<QueueModel> queue)
        {
            var ticket = await GetTicketByUidAsync(ticketUid);
            if (ticket is null)
            {
                throw new NotFoundException("Билет не найден");
            }
            ticket.Status = "CANCELED";
            try
            {
                await UpdateTicketAsync(ticket.Ticketuid.ToString(), ticket);
            }
            catch (Exception)
            {
                queue.Enqueue(new QueueModel
                {
                    Enum = QueueEnum.TicketServiceFallback,
                    TicketUid = ticketUid,
                    UserName = userName
                });
            }
            try
            {
                await UpdatePrivilegesAfterTicketDelete(userName, ticket);
            }
            catch (Exception)
            {
                queue.Enqueue(new QueueModel
                {
                    Enum = QueueEnum.ReservationServiceFallback,
                    UserName = userName,
                    Ticket = ticket
                });
            }
        }

        public async Task UpdatePrivilegesAfterTicketDelete(string userName, GetTicketDto? ticket)
        {
            var privilege = await _privilegeService.GetPrivilegeAsync(userName);
            var histories = await _privilegeService.GetAllPrivilegeHistories(1, 100);
            var history = histories.Items.Where(x => x.PrivilegeId == privilege.Id).FirstOrDefault(x => x.TicketUid == ticket.Ticketuid);
            var newHistory = new PrivilegeHistoryDto
            {
                BalanceDiff = -history.BalanceDiff,
                Datetime = DateTime.Now,
                OperationType = history.OperationType == "FILL_IN_BALANCE" ? "DEBIT_THE_ACCOUNT" : "FILL_IN_BALANCE",
                PrivilegeId = privilege.Id,
                TicketUid = ticket.Ticketuid
            };
            privilege.Balance += newHistory.BalanceDiff;
            await _privilegeService.UpdatePrivilegeAsync(privilege.Id, privilege);
            await _privilegeService.AddPrivilegeHistoryAsync(newHistory);
        }
    }
}
