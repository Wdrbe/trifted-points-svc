using Amazon.Lambda.Annotations;
using Amazon.Lambda.SQSEvents;
using Kanject.Core.Queue.Provider.AwsSqs.Abstractions.Extensions;
using Trifted.Points.Tasks.Config.Aws;
using Trifted.Points.Tasks.Consumers;
using Trifted.Points.Tasks.RouteConsumers.DefaultQueue;

namespace Trifted.Points.Tasks;

/// <summary>
/// A collection of Lambda functions designed to handle Kanject.Aws.Lambda background running tasks 
/// </summary>
public partial class Functions
{
    /// <summary>
    /// Processes SQS events related to Wdrbe quest consumption.
    /// </summary>
    /// <param name="sqsEvent">The SQS event containing the messages to be processed.</param>
    /// <returns>A batch response indicating the result of processing the SQS messages.</returns>
    [LambdaFunction(
        ResourceName = "WdrbeQuestConsumerFunc",
        MemorySize = 1024,
        Timeout = 30,
        Policies = IamPolicyDefaults.FunctionPolicy,
        PackageType = LambdaPackageType.Zip
    )]
    public Task<SQSBatchResponse> WdrbeQuestConsumerFuncAsync(SQSEvent sqsEvent)
    {
        sqsEvent.PrintInConsole(); //log request payload
        return ServiceProvider.ProcessWdrbeQuestEventAsync(sqsEvent);
    }

    /// <summary>
    /// Routes SQS events to the Points Service Default Queue.
    /// </summary>
    /// <param name="sqsEvent">The SQS event containing the messages to be routed.</param>
    /// <returns>A batch response indicating the result of processing the SQS messages.</returns>
    [LambdaFunction(
        ResourceName = "PointsTaskRouterFunc",
        MemorySize = 1024,
        Timeout = 30,
        Policies = IamPolicyDefaults.FunctionPolicy,
        PackageType = LambdaPackageType.Zip
    )]
    public Task<SQSBatchResponse> PointsTaskRouterAsync(SQSEvent sqsEvent)
    {
        sqsEvent.PrintInConsole(); //log request payload
        return ServiceProvider.RouteSqsEventToPointsSvcDefaultQueueAsync(sqsEvent);
    }
}