namespace Tech.Challenge.Persistence.Infrasctructure.Queue;
public static class RabbitMqConstants
{
    public const string ContactPersistenceExchange = $"contact.persistence.exchange";
    public const string RegionPersistenceExchange = $"region.persistence.exchange";
    public const string UserPersistenceExchange = $"user.persistence.exchange";

    public const string RegisterContactQueueName = "contact.persistence.on-register-contact";
    public const string RegisterContactRoutingKey = "on-register-contact";

    public const string DeleteContactQueueName = "contact.persistence.on-delete-contact";
    public const string DeleteContactRoutingKey = "on-delete-contact";

    public const string RegisterRegionQueueName = "region.persistence.on-register-region";
    public const string RegisterRegionRoutingKey = "on-register-region";

    public const string RegisterUserQueueName = "user.persistence.on-register-user";
    public const string RegisterUserRoutingKey = "on-register-user";
}
