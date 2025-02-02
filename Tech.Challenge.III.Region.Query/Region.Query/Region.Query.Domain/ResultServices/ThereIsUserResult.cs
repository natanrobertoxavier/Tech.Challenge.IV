using Newtonsoft.Json;

namespace Region.Query.Domain.ResultServices;
public class ThereIsUserResult
{
    [JsonProperty("thereIsUser")]
    public bool ThereIsUser { get; set; }
}
