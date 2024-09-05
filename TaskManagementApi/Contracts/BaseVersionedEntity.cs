using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementApi.Contracts
{
    public abstract class BaseVersionedEntity : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long Version { get; set; }

        [NotMapped]
        public abstract IList<string> VersionedFields { get; }
    }
}
