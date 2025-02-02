using System.Text.Json.Serialization;

namespace Contact.Persistence.Domain.ResultServices;
public class ThereIsContactResult
{
    [JsonPropertyName("thereIsContact")]
    public bool ThereIsContact { get; set; }
}
