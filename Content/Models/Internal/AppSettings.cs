namespace WebApiTemplate.Models;

public record AppSettings
{
    public string ConnectionString { get; init; }
    public string AuthorizationServer { get; init; }
    public string AuthenticationName { get; init; }
    public string ServiceName { get; init; }
    public string Version { get; init; }
    public string Policy { get; init; }
    public HealthDefinition HealthDefinition { get; init; }
}
