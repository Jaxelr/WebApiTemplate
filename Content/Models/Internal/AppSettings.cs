namespace WebApiTemplate.Models;

public class AppSettings
{
    public string ConnectionString { get; set; }
    public string AuthorizationServer { get; set; }
    public string AuthenticationName { get; set; }
    public string ServiceName { get; set; }
    public string Version { get; set; }
    public string Policy { get; set; }
    public HealthDefinition HealthDefinition { get; set; }
}
