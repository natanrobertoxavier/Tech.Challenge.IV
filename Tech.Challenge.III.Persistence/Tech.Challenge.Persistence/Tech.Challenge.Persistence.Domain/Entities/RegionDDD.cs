namespace Tech.Challenge.Persistence.Domain.Entities;
public class RegionDDD(
    Guid id,
    DateTime registrationDate,
    int dDD,
    string region,
    Guid userId) : BaseEntity(id, registrationDate)
{
    public int DDD { get; set; } = dDD;
    public string Region { get; set; } = region;
    public Guid UserId { get; set; } = userId;

    public RegionDDD() : this(
        Guid.Empty,
        DateTime.MinValue,
        0,
        string.Empty,
        Guid.Empty)
    { }
}
