using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Extensions
{
    public class LogsProducer
    {
        private readonly IProducer<Null, string> _producer;

        public LogsProducer()
        {
            var config = new ProducerConfig
            {
                // User-specific properties that you must set
                BootstrapServers = "broker-kafka:29092",
                // Fixed properties
                Acks = Acks.All,
               
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task<DeliveryResult<Null, string>> Produce(string message)
        {
            //var topic = new TopicPartition(message, new Partition(0));
            return await _producer.ProduceAsync("logs", new Message<Null, string>
            {
                Value = message
            });
        }
    }
}
