using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using UserStorage.Application.Commands.AddUser;
using UserStorage.Presentation.Consumers.User.Contracts;
using UserStorage.Presentation.Consumers.User.Converters;
using UserStorage.Presentation.Options;

namespace UserStorage.Presentation.Consumers.User;

public class UserKafkaBatchConsumer : BaseConsumer<UserContract>
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserKafkaBatchConsumer> _logger;

    public UserKafkaBatchConsumer(ILogger<UserKafkaBatchConsumer> logger, IMediator mediator,  IOptions<KafkaOptions> kafkaOptions)
        : base(logger, kafkaOptions)
    {
        _logger = logger;
        _mediator = mediator;
    }

    protected override async Task ProcessBatchAsync(List<ConsumeResult<Ignore, string>> batch, CancellationToken cancellationToken)
    {
        foreach (ConsumeResult<Ignore, string> consumeResult in batch)
        {
            _logger.LogInformation("Start processing message {Value}", consumeResult.Message.Value);

            UserContract? userContract = JsonConvert.DeserializeObject<UserContract>(consumeResult.Message.Value);

            AddUserCommand addUserCommand = UserContractConverter.ToAddUserCommand(userContract);
            
            await _mediator.Publish(addUserCommand, cancellationToken);
        }
    }
}
