using Kanject.Core.Queue.Abstractions.Models;
using Kanject.Core.Queue.Provider.AwsSqs.Annotations.Attributes;
using Trifted.Core.Trifted.Identity;
using Trifted.Core.Trifted.Identity.Events.AccountManagement;
using Trifted.Core.Trifted.Points;

namespace Trifted.Points.Tasks.RouteConsumers.DefaultQueue;

[EventHubQueueRouter]
[RouteQueueConsumer(
    Message = typeof(UserCreatedArgs),
    QueueName = TriftedPoints.Queues.DefaultServiceQueue
)]
[QueueConsumerRoute(topic: TriftedIdentity.EventTopics.UserCreated)]
public partial class UserCreatedConsumer
{
    protected override Task ConsumeAsync(List<MessageContext<UserCreatedArgs>> messageContexts)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            return Task.FromException(exception);
        }
    }
}