using Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using MovEstApi.Filters;
using MovEstApi.TransferModels;
using Service;

namespace MovEstApi.Controllers;

public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly ProductService _productService;
    public ProductsController(ILogger<ProductsController> logger, 
                              ProductService productService){
        _logger = logger;
        _productService = productService;
    }
    [HttpGet]
    [Route("/getallproducts")]
    public IEnumerable<Product> GetAllProducts(){
        return _productService.GetAllProducts();
    }
    [HttpGet]
    [Route("/getproduct/{sku}")]
    public Product GetProduct([FromRoute] string sku){
        return _productService.GetProductsBySku(sku);
    }
    [HttpPost]
    [ValidateModel]
    [Route("/createproduct")]
    public ResponseDto CreateProduct([FromBody] CreateProductRequestDto dto){
        return new ResponseDto(){
            Message = "Product created successfully",
            ResponseData = _productService.CreateProduct(dto.Sku, 
                                                        dto.Description)
        };            
    }
}