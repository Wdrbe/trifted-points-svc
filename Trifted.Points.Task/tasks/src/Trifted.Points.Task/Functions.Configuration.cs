using Kanject.Core.Api.Aws.Extensions.Vault.ParameterStore;
using Kanject.Core.Queue.Provider.AwsSqs.Abstractions.Extensions;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Trifted.Points.Task;

/// <summary>
/// A collection of sample Lambda functions that provide a REST api for doing simple math calculations. 
/// </summary>
public partial class Functions : CloudFunction
{
#if DEBUG
    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    //public Function() : base(CloudFunctionEnvironmentEnum.Production)
    // public Functions() : base(CloudFunctionEnvironment.Staging)
    public Functions() : base(CloudFunctionEnvironment.Development)
    {
    }
#else
    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Functions() : base()
    {
    }
#endif

    /// <summary>
    /// OnStartup
    /// </summary>
    public override void OnStartup(IConfigurationBuilder configurationBuilder)
    {
#if DEBUG
        configurationBuilder.AddAwsSystemManagerParameterStore(fetchSecretPathFromAppSettings: true);
#else
        configurationBuilder.AddAwsSystemManagerParameterStore();
#endif
    }

    /// <summary>
    /// Configure Services
    /// </summary>
    /// <param name="services"></param>
    public override void ConfigureServices(IServiceCollection services)
    {
        #region Queue Consumers

        services.AddAwsSqsGlobalQueueConfiguration(options =>
        {
            options.AWSAccessKey = "aws-access-key";
            options.AWSSecretKey = "aws-secret-key";
            options.AWSRegion = "aws-region";
            options.Namespace = "queue-namespace";
            options.DlqMessageRetentionPeriod = 14;
            options.UseDeadLetterQueue = true;
            options.CreateQueueIfNotExist = true;
            options.WatchQueue = true;
        });

        #endregion
    }

    /// <summary>
    /// Configure
    /// </summary>
    /// <param name="serviceProvider"></param>
    public override void Configure(IServiceProvider serviceProvider)
    {
    }
}