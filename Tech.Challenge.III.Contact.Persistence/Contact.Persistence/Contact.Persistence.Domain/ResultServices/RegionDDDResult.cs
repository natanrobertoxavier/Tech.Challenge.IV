using System.Text.Json.Serialization;

namespace Contact.Persistence.Domain.ResultServices;
public class RegionDDDResult
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("ddd")]
    public int DDD { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }
}
