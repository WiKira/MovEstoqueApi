namespace Infrastructure.DataModels;

public class Product
{
    public Guid Id { get; }
    public string? Sku { get; set; } 
    public string? Description { get; set; } 
    public DateTime RegistrationDate { get; set; }
    public DateTime MoveDate { get; set; }
    public decimal Balance { get; set; } 
}