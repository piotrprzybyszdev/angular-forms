using DemoProjectCommon.Model;

namespace FormsBackendCommon.Model;

public class TaskModel : BaseEntity
{
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate { get; set; }
    public DateTime DueDate { get; set; }
    public ApplicationUser Account { get; set; } = default!;
}
