namespace FTELMarket.Data.Entities;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}