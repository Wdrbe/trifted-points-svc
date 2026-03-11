using System.Text.Json.Serialization;

namespace Trifted.Points.Tasks.Config.Models;

public class ServiceBaseUrl
{
    [JsonPropertyName("identityServiceBaseUrl")]
    public string IdentityServiceBaseUrl { get; set; } = string.Empty;

    [JsonPropertyName("schoolManagementServiceBaseUrl")]
    public string SchoolManagementServiceBaseUrl { get; set; } = string.Empty;


    [JsonPropertyName("configurationServiceBaseUrl")]
    public string ConfigurationServiceBaseUrl { get; set; } = string.Empty;
}