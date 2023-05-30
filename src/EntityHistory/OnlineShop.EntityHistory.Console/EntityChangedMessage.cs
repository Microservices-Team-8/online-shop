using System.ComponentModel.DataAnnotations;

namespace OnlineShop.EntityHistory.Console;

public class EntityChangedMessage
{
    [Key]
    public int Id { get; set; }
    public string EntityName { get; set; }
    public int EntityId { get; set; }
    public EntityChangeType ChangeType { get; set; }
    public string? NewValue { get; set; }
}