using System.ComponentModel.DataAnnotations;
using Infrastructure.DataModels;
using Infrastructure.Repositories;

namespace Service;

public class MovementService
{
  private readonly MovementRepository _movementRepository;
  private readonly ProductService _productService;
  public MovementService(MovementRepository movementRepository, ProductService productService){
    _movementRepository = movementRepository;
    _productService = productService;
  }

  public IEnumerable<Movement> GetMovementsForFeed(){
    return _movementRepository.GetMovementsForFeed();
  }

  public object CreateMovement(string? _sku, int? _type, decimal? _quantity)
    {
      if (string.IsNullOrEmpty(_sku) || _type == null || _quantity == null)
        throw new ValidationException("SKU, Type and Quantity are required.");

      var product = _productService.GetProductsBySku(_sku);

      if(product == null)
        throw new KeyNotFoundException("SKU does not exists.");

      product.Balance += _type == 0 ? _quantity.Value : (_quantity.Value * -1);
      
      if(_quantity <= 0)
        throw new ArgumentException("Quantity must be greater than zero.");

      if (product.Balance < 0)
        throw new ArgumentException("Insufficient balance.");

      var move_date = DateTime.Now;
      _movementRepository.CreateMovement(product.Id, 
                                        _type.Value, 
                                        _quantity.Value, 
                                        move_date);

      product.MoveDate = move_date;

      _productService.UpdateProduct(product.Id, product.Balance, product.MoveDate);
      return product;
    }

    public object DeleteMovement(Guid movement_id, string product_sku)
    {
        var product = _productService.GetProductsBySku(product_sku);
        if(product == null) throw new KeyNotFoundException("Product not found.");

        var movement = _movementRepository.GetLastMovementByProd(product.Id);
        
        if (movement == null) throw new KeyNotFoundException("No movement found.");
        if (movement.Id != movement_id) throw new ArgumentException("The informed movement isn't the last.");

        product.Balance += movement.MovementType == 0 ? (-1 * movement.Quantity) : movement.Quantity;

        _movementRepository.DeleteMovement(movement_id);

        movement = _movementRepository.GetLastMovementByProd(product.Id);

        product.MoveDate = movement == null? DateTime.MinValue : movement.MovementDate;

        _productService.UpdateProduct(product.Id, product.Balance, product.MoveDate);
        return product;
    }
}
