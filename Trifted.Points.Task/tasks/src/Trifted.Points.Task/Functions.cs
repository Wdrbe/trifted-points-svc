using Amazon.Lambda.Annotations;
using Amazon.Lambda.SQSEvents;
using Trifted.Points.Task.Config.Aws;
using Kanject.Core.Queue.Provider.AwsSqs.Abstractions.Extensions;

namespace Trifted.Points.Task;

/// <summary>
/// A collection of Lambda functions designed to handle Kanject.Aws.Lambda background running tasks 
/// </summary>
public partial class Functions
{
    /// <summary>
    /// Process payment authorization
    /// </summary>
    /// <param name="sqsEvent"></param>
    [LambdaFunction(
        ResourceName = "SampleSqsTaskFunc",
        MemorySize = 1024,
        Timeout = 30,
        Policies = IamPolicyDefaults.FunctionPolicy,
        PackageType = LambdaPackageType.Image
    )]
    public Task<SQSBatchResponse> SampleSqsTaskAsync(SQSEvent sqsEvent)
    {
        sqsEvent.PrintInConsole(); //log request payload
        throw new NotImplementedException();
    }

    /// <summary>
    /// Process payment authorization
    /// </summary>
    /// <param name="sqsEvent"></param>
    [LambdaFunction(
        ResourceName = "SampleSqsRouteConsumerTaskFunc",
        MemorySize = 1024,
        Timeout = 30,
        Policies = IamPolicyDefaults.FunctionPolicy,
        PackageType = LambdaPackageType.Image
    )]
    public Task<SQSBatchResponse> SampleSqsTaskRouterAsync(SQSEvent sqsEvent)
    {
        sqsEvent.PrintInConsole(); //log request payload
        return ServiceProvider.RouteIncomingSqsQueueEventAsync(
            queue: "router-queue-name",
            sqsEvent: sqsEvent
        );
    }
}