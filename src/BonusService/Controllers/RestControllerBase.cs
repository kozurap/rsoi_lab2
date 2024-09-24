using AutoMapper;
using Confluent.Kafka;
using Kernel.AbstractClasses;
using Kernel.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace PrivilegeService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RestControllerBase<TEntity, TDto, TFilter> : ControllerBase
       where TEntity : EntityBase, new()
    {
        protected readonly IMapper Mapper;
        protected readonly AppDbContext DbContext;
        private readonly LogsProducer _producer;
        protected virtual Expression<Func<TEntity, bool>>? UidPredicate(Guid uId) => null;
        private const string serviceName = "PrivilegeService";
        protected bool IsUidSupported => UidPredicate(Guid.NewGuid()) != null;
        protected virtual Guid? GetUid(TEntity e) => null;

        protected virtual void SetUid(TEntity entity, Guid uId)
        {
        }

        public RestControllerBase(IMapper mapper, AppDbContext dbContext, LogsProducer producer)
        {
            Mapper = mapper;
            DbContext = dbContext;
            _producer = producer;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TDto>> GetByIdAsync(string id)
        {
            await _producer.Produce(serviceName + "Get" + $"/{id}");
            int? intId = int.TryParse(id, out var asInt) ? asInt : null;
            Guid? guid = Guid.TryParse(id, out var asGuid) ? asGuid : null;

            if (guid == null && IsUidSupported) return BadRequest("UID is in wrong format");
            if (intId == null && !IsUidSupported) return BadRequest("Entity doesn't support UID");

            var e = await AttachEagerLoadingStrategyToQueryable(DbContext.Set<TEntity>().AsNoTracking())
                .FirstOrDefaultAsync(intId == null ? UidPredicate(guid!.Value)! : x => x.Id == intId);

            if (e == null) return NotFound();

            return Ok(await ToDtoAsync(e));
        }


        [HttpGet]
        public async Task<ActionResult<PaginationModel<TDto>>> GetAllAsync([FromQuery] TFilter filter, [FromQuery] int size = 10,
            [FromQuery] int page = 1)
        {
            await _producer.Produce(serviceName + "Get");
            Console.WriteLine("Filters: " + JsonSerializer.Serialize(filter));
            var q = AttachEagerLoadingStrategyToQueryable(
                AttachFilterToQueryable(DbContext.Set<TEntity>(), filter)
                    .OrderByDescending(x => x.Id));

            var lst = await q
                .Page(page, size)
                .ToListAsync();

            return Ok(new PaginationModel<TDto>
            {
                Page = page,
                PageSize = size,
                TotalElements = await q.CountAsync(),
                Items = await ToDtoListAsync(lst)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] TDto dto)
        {
            await _producer.Produce(serviceName + "Post");
            var e = new TEntity();
            MapDtoToEntity(e, dto);

            if (IsUidSupported) SetUid(e, Guid.NewGuid());

            await DbContext.AddAsync(e);
            await DbContext.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = e.Id }, e);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TDto>> UpdateAsync([FromBody] TDto dto, string id)
        {
            await _producer.Produce(serviceName + "Patch" + $"/{id}");
            int? intId = int.TryParse(id, out var asInt) ? asInt : null;
            Guid? guid = Guid.TryParse(id, out var asGuid) ? asGuid : null;

            if (guid == null && IsUidSupported) return BadRequest("UID is in wrong format");
            if (intId == null && !IsUidSupported) return BadRequest("Entity doesn't support UID");

            var e = await DbContext.Set<TEntity>()
                .FirstOrDefaultAsync(intId == null ? UidPredicate(guid.Value)! : x => x.Id == intId);

            if (e == null) return NotFound();

            MapDtoToEntity(e, dto);

            await DbContext.SaveChangesAsync();
            DbContext.Entry(e).State = EntityState.Detached;

            return await GetByIdAsync(id);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            await _producer.Produce(serviceName + "Delete" + $"/{id}");
            var e = await DbContext.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e == null) return NotFound();

            DbContext.Remove(e);
            await DbContext.SaveChangesAsync();

            return NoContent();
        }

        protected virtual void MapDtoToEntity(TEntity e, TDto dto)
        {
            e = Mapper.Map<TEntity>(dto);
        }

        protected virtual IQueryable<TEntity> AttachFilterToQueryable(IQueryable<TEntity> q, TFilter f)
            => q;

        protected virtual IQueryable<TEntity> AttachEagerLoadingStrategyToQueryable(IQueryable<TEntity> q)
            => q;

        protected virtual Task<TDto> ToDtoAsync(TEntity e) => Task.FromResult(Mapper.Map<TDto>(e));

        protected virtual async Task<List<TDto>> ToDtoListAsync(List<TEntity> list)
        {
            var r = new List<TDto>();
            foreach (var item in list)
                r.Add(await ToDtoAsync(item));

            return r;
        }
    }
}
