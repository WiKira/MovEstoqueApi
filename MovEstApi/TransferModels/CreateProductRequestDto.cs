using System.ComponentModel.DataAnnotations;

namespace MovEstApi.TransferModels;
public class CreateProductRequestDto
{
    [Required]
    [MinLength(3)]
    public string? Sku { get; set; }
    [Required]
    [MinLength(3)] 
    public string? Description { get; set; }  
}
