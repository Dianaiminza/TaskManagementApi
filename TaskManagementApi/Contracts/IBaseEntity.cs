using System.Security.Principal;

namespace TaskManagementApi.Contracts
{
    public interface IBaseEntity : IEntity
    {
        DateTimeOffset CreatedOn { get; set; }

        DateTimeOffset? LastModifiedOn { get; set; }
    }
}
