using System.ComponentModel.DataAnnotations;
using Infrastructure.DataModels;
using Infrastructure.Repositories;

namespace Service;

public class ProductService
{
  private readonly ProductRepository _productRepository;
  public ProductService(ProductRepository productRepository){
    _productRepository = productRepository;
  }

  public IEnumerable<Product> GetAllProducts(){
    return _productRepository.GetAllProducts();
  }
  
  public Product GetProductsBySku(string sku){
    
    if (!_productRepository.SkuExists(sku)) 
      throw new KeyNotFoundException($"Product with SKU {sku} not found.");

    return _productRepository.GetProductsBySku(sku);
  }

    public object CreateProduct(string? _sku, string? _description)
    {
      if (string.IsNullOrEmpty(_sku) || string.IsNullOrEmpty(_description))
        throw new ValidationException("SKU and Description are required.");

      if(_productRepository.SkuExists(_sku))
        throw new ValidationException("SKU already exists.");

      _productRepository.CreateProduct(_sku, _description);
      return _productRepository.GetProductsBySku(_sku);
    }

    internal void UpdateProduct(Guid product_id, decimal product_balance, DateTime product_move_date)
    {
        _productRepository.UpdateProduct(product_id, product_balance, product_move_date);
        return;
    }

    internal Product GetProductById(Guid idProduct)
    {
      return _productRepository.GetProductById(idProduct);
    }
}
