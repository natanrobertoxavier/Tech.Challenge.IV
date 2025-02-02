using Newtonsoft.Json;

namespace Contact.Persistence.Domain.ResultServices;
public class ThereIsUserResult
{
    [JsonProperty("thereIsUser")]
    public bool ThereIsUser { get; set; }
}
