namespace OnlineShop.EntityHistory.Console;

public class EntityChange
{
    public string EntityName { get; set; }
    public int EntityId { get; set; }
    public EntityChangeType ChangeType { get; set; }
    public string? NewValue { get; set; }
}