namespace WebApiTemplate.Models;

public record HealthDefinition
{
    public string Endpoint { get; set; }
    public string Name { get; set; }
    public string HealthyMessage { get; set; }
    public string[] Tags { get; set; }
}
