using System.Text.Json.Serialization;

namespace Trifted.Points.Task.Config.Models;

public class AppSettings
{
    [JsonPropertyName("serviceBaseUrl")] public ServiceBaseUrl ServiceBaseUrl { get; set; }

    [JsonPropertyName("allowedCorsOrigin")]
    public string AllowedCorsOrigin { get; set; } = string.Empty;

    [JsonPropertyName("awsAccessKeyId")] public string AwsAccessKeyId { get; set; } = string.Empty;

    [JsonPropertyName("awsAccessSecretKey")]
    public string AwsAccessSecretKey { get; set; } = string.Empty;

    [JsonPropertyName("databaseNamespace")]
    public string DatabaseNamespace { get; set; } = string.Empty;

    [JsonPropertyName("awsRegion")] public string AwsRegion { get; set; } = string.Empty;

    [JsonPropertyName("runEtlPackage")] public bool RunEtlPackage { get; set; }
}