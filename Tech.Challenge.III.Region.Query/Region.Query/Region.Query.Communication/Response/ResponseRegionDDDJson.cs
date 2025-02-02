namespace Region.Query.Communication.Response;

public class ResponseRegionDDDJson(
    Guid id,
    int dDD,
    string region)
{
    public Guid Id { get; set; } = id;
    public int DDD { get; set; } = dDD;
    public string Region { get; set; } = region;
}
