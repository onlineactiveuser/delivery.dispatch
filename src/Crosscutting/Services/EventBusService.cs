using Application.Contracts.Settings;
using Application.Interfaces;
using Confluent.Kafka;
using Domain.Abstraction.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Crosscutting.Services
{
    public class EventBusService : IEventBusService
    {
        private readonly KafkaSettings _kafkaSettings;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        public EventBusService(
            KafkaSettings kafkaSettings,
            IServiceScopeFactory serviceScopeFactory)
        {
            _kafkaSettings = kafkaSettings;
            _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }
        public async Task Publish<E>(E @event) where E : IEvent
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServer,
                EnableIdempotence = true,
            };
            var eventName = @event.GetType().Name;
            var message = JsonConvert.SerializeObject(@event);

            using var producer = new ProducerBuilder<Null, string>(config)
                .Build();
            try
            {

                await producer.ProduceAsync(eventName, new Message<Null, string> { Value = message });
            }
            catch (ProduceException<Null, string> e)
            {
            }
        }

        public void Subscribe<E, EH>(CancellationToken cancellationToken = default)
            where E : IEvent
            where EH : IEventHandler<E>
        {

            var eventName = typeof(E).Name;

            var handlerType = typeof(EH).Name;

            _eventTypes.Add(typeof(E));
            _handlers.Add(eventName, new List<Type>());
            _handlers[eventName].Add(typeof(EH));

            Task.Run(() => ConsumeEvent<E, EH>(eventName, handlerType));
        }

        private void ConsumeEvent<E, EH>(string eventName, string handlerType) 
            where E : IEvent
            where EH : IEventHandler<E>
        {
            var conf = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServer,
                GroupId = _kafkaSettings.ConsumerGroup,
                EnableAutoCommit = false,
                SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();

            consumer.Subscribe(eventName);

            try
            {
                while (true)
                {
                    try
                    {
                        var consumeResult = consumer.Consume();

                        if (consumeResult.IsPartitionEOF)
                        {
                            continue;
                        }
                        ProcessEvent<E, EH>(eventName, consumeResult.Message.Value);
                        consumer.Commit(consumeResult);
                    }
                    catch (ConsumeException e)
                    {
                    }
                    catch (KafkaException e)
                    {
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
            }
        }

        private void ProcessEvent<E, EH>(string eventName, string @event) 
            where E : IEvent
            where EH : IEventHandler<E>
        {
            if (_handlers.ContainsKey(eventName))
            {
                var subscriptions = _handlers[eventName];
                foreach (var subscription in subscriptions)
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var handler = (IEventHandler<E>)scope.ServiceProvider.GetRequiredService(subscription);
                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                        var @eventHandle = JsonConvert.DeserializeObject<E>(@event)!;
                        handler.Handle(@eventHandle);
                    }
                }
            }
        }
    }
}
