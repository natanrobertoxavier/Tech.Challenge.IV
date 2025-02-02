namespace Region.Query.Communication.Response;
public class ResponseListRegionDDDJson(IEnumerable<ResponseRegionDDDJson> regionsDDD)
{
    public IEnumerable<ResponseRegionDDDJson> RegionsDDD { get; set; } = regionsDDD;
}
