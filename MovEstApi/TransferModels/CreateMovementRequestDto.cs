using System.ComponentModel.DataAnnotations;

namespace MovEstApi.TransferModels
{
    public class CreateMovementRequestDto
    {
        [Required]
        [MinLength(3)]
        public string? Product_Sku { get; set; }
        [Required]
        public int MovementType { get; set; }
        [Required]
        public decimal Quantity { get; set; } 
    }
}