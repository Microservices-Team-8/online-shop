namespace OnlineShop.Domain;

public class BasketItem
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public decimal Price { get; set; }

    public int ItemCount { get; set; }
}
