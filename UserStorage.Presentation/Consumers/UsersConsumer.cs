using Confluent.Kafka;
using UserStorage.Infrastructure.Kafka.Extensions;

namespace UserStorage.Presentation.Consumers;

internal sealed class UsersConsumer : IHostedService
{
    private readonly string topic = "test";
    private readonly string groupId = "test_group";
    private readonly string bootstrapServers = "localhost:9092";
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("my-topic");

        while (true)
        {
            var consumeResult = consumer.ConsumeBatch(TimeSpan.FromSeconds(1), 1000);

            if (consumeResult == null || consumeResult.Count == 0)
            {
                continue;
            }

            Console.WriteLine($"Received batch of {consumeResult.Count} messages:");

            foreach (var message in consumeResult)
            {
                Console.WriteLine($"Message: {message.Value}, Offset: {message.TopicPartitionOffset}");
            }

            consumer.Commit(); // коммитим сообщения вручную
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}