using System.Text.Json.Serialization;

namespace Contact.Persistence.Domain.ResultServices;
public class ContactResult
{
    [JsonPropertyName("contactId")]
    public Guid ContactId { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string LastName { get; set; }

    [JsonPropertyName("ddd")]
    public int DDD { get; set; }

    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}
