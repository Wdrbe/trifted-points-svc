using System.Text.Json;
using Amazon.Lambda.SQSEvents;
using Amazon.SQS.Model;
using Kanject.Core.Api.Abstractions.Exceptions;
using Kanject.Core.Queue.Provider.AwsSqs.Annotations.Attributes;
using Trifted.Points.Data;
using Trifted.Points.Data.Entities.Users;
using Trifted.Points.Data.Entities.WdrbeQuest;
using Trifted.Points.Data.Repositories;
using static Trifted.Points.Data.Repositories.WdrbeQuestRepositoryItemCollectionType;

namespace Trifted.Points.Tasks.Consumers;

[QueueConsumer]
[QueueConsumerDependency(typeof(WdrbeQuestRepository))]
[QueueConsumerDependency(typeof(UserQuestRepository))]
public partial class WdrbeQuestConsumer
{
    protected override async Task ConsumeAsync(List<Message> messages)
    {
        var topics = messages.Select(message => message.MessageAttributes["EventTopic"].StringValue).ToArray();
        var questTasks = await WdrbeQuestRepository.WdrbeQuestTask
            .FindWdrbeQuestTasksByQuestEventTopicsAsync(topics);

        var questTasksLookup = questTasks.ToLookup(task => task.EventTopic);

        var questLookup = (await WdrbeQuestRepository.GetItemCollectionAsync(
                ids: [..questTasks.Select(task => task.QuestId)],
                includes:
                [
                    WdrbeQuest,
                    WdrbeQuestTask
                ]))
            .ToLookup(quest => quest.WdrbeQuest!.Id);

        foreach (var messageContext in messages)
        {
            try
            {
                var eventTopic = messageContext.MessageAttributes["EventTopic"].StringValue;

                if (string.IsNullOrWhiteSpace(eventTopic))
                {
                    "Unable to find event topic in message attributes".PrintInConsole();
                    continue;
                }

                var parsedMessage = JsonSerializer.Deserialize<JsonElement>(messageContext.Body);

                var relatedQuests = questTasksLookup[eventTopic];

                foreach (var quest in relatedQuests)
                {
                    var userIdentifier = parsedMessage.GetProperty(quest.UserIdentifier).GetString();

                    if (!Guid.TryParse(userIdentifier, out var userId))
                    {
                        "Unable to parse user identifier from message body".PrintInConsole();
                        messageContext.PrintInConsole(tag: nameof(ConsumeAsync));
                        throw new ApiValidationException("Unable to parse user identifier from message body");
                    }

                    var response = await ProcessUserPointAsync(userId: userId, questId: quest.Id);

                    response.PrintInConsole();
                }
            }
            catch (Exception ex)
            {
                ex.PrintInConsole();
                Response.BatchItemFailures.Add(new SQSBatchResponse.BatchItemFailure
                    { ItemIdentifier = messageContext.MessageId });
            }
        }
    }

    private async Task<WdrbeQuestEntity> ProcessUserPointAsync(Guid userId, Guid questId)
    {
        var quest = await WdrbeQuestRepository.FindWdrbeQuestAsync(questId)
                    ?? throw new ApiValidationException("Quest not found");

        UserQuestRepository.BeginTransaction();

        var userQuest = await UserQuestRepository.FindUserQuestAsync(userId, questId);

        var pointsToAward = quest.PointPerAction;

        if (userQuest == null)
        {
            userQuest = new UserQuestEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                QuestId = questId,
                Points = pointsToAward
            };
        }
        else
        {
            var accumulatedPoints = userQuest.Points;

            if (accumulatedPoints >= quest.Points)
            {
                ($"User with id {userId} already completed quest {questId}").PrintInConsole();
                return quest;
            }

            if (accumulatedPoints + pointsToAward > quest.Points)
            {
                pointsToAward = quest.Points - accumulatedPoints;
            }

            userQuest.Points += pointsToAward;
        }

        var userPoint = await UserQuestRepository.UserPoint.FindUserPointAsync(userId);

        if (userPoint == null)
        {
            userPoint = new UserPointEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Points = pointsToAward
            };
        }
        else
        {
            userPoint.Points += pointsToAward;
        }

        await UserQuestRepository.AddOrUpdateAsync(userQuest);
        await UserQuestRepository.UserPoint.AddOrUpdateAsync(userPoint);

        await UserQuestRepository.CommitAsync();

        return quest;
    }
}