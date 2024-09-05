namespace TaskManagementApi.Contracts
{
    public abstract class BaseEntity : IBaseEntity, IEntity
    {
        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? LastModifiedOn { get; set; }

        public BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
        }
    }
}
