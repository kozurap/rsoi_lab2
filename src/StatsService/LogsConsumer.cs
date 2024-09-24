using Confluent.Kafka;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using static Confluent.Kafka.ConfigPropertyNames;


namespace StatsService
{
    public class LogsConsumer
    {
        public IConsumer<Null, string> _consumer;
        public LogsConsumer()
        {
            var config = new ConsumerConfig
            {
                // User-specific properties that you must set
                BootstrapServers = "broker-kafka:29092",
                GroupId = "statsService",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Null, string>(config).Build();
            _consumer.Subscribe("logs");
        }
        public string Consume()
        {
            try
            {
                var res = _consumer.Consume(10000);
                if(res == null)
                {
                    return "plz stop";
                }
                _consumer.Commit(res);
                return res.Message.Value;
            }
            catch
            {
                return "plz stop";
            }
        }
    }
}
