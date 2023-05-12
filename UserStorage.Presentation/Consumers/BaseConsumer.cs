using Confluent.Kafka;
using Microsoft.Extensions.Options;
using UserStorage.Presentation.Options;

public abstract class BaseConsumer<TEntity> : BackgroundService
{
    private readonly ILogger<BaseConsumer<TEntity>> _logger;
    private readonly IConsumer<Ignore, string> _consumer;

    private readonly int _batchSize;
    private readonly TimeSpan _batchTimeout;

    protected BaseConsumer(ILogger<BaseConsumer<TEntity>> logger, IOptions<KafkaOptions> kafkaOptions)
    {
        _logger = logger;

        KafkaOptions kafkaConfig = kafkaOptions.Value;

        ConsumerConfig consumerConfig = new ConsumerConfig
        {
            BootstrapServers = kafkaConfig.BootstrapServers,
            GroupId = kafkaConfig.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig)
            .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}"))
            .Build();

        _batchSize = kafkaConfig.BatchSize;
        _batchTimeout = TimeSpan.FromSeconds(kafkaConfig.BatchTimeoutInSeconds);

        _consumer.Subscribe(kafkaConfig.Topic);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{typeof(TEntity).Name} KafkaBatchConsumerService is starting.");

        cancellationToken.Register(() =>
        {
            _logger.LogInformation($"{typeof(TEntity).Name} KafkaBatchConsumerService background task is stopping.");
            _consumer.Close();
        });

        try
        {
            List<ConsumeResult<Ignore, string>> batch = new List<ConsumeResult<Ignore, string>>();
            DateTime lastBatchTime = DateTime.UtcNow;

            while (!cancellationToken.IsCancellationRequested)
            {
                ConsumeResult<Ignore, string>? consumeResult = _consumer.Consume(cancellationToken);

                if (consumeResult != null)
                {
                    batch.Add(consumeResult);

                    if (batch.Count >= _batchSize || DateTime.UtcNow - lastBatchTime >= _batchTimeout)
                    {
                        await ProcessBatchAsync(batch, cancellationToken);

                        batch.Clear();
                        lastBatchTime = DateTime.UtcNow;
                    }
                }
            }

            if (batch.Any())
            {
                await ProcessBatchAsync(batch, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occurred while consuming {typeof(TEntity).Name} messages.");
        }
        finally
        {
            _logger.LogInformation($"{typeof(TEntity).Name} KafkaBatchConsumerService background task is stopping.");
            _consumer.Close();    
        }
    }

    protected abstract Task ProcessBatchAsync(List<ConsumeResult<Ignore, string>> batch, CancellationToken cancellationToken);
}
