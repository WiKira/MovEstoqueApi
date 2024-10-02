using Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using MovEstApi.Filters;
using MovEstApi.TransferModels;
using Service;

namespace MovEstApi.Controllers
{
    [ApiController]
    public class MovementsController : ControllerBase
    {
        private readonly ILogger<MovementsController> _logger;
        private readonly MovementService _movementService;
        public MovementsController(ILogger<MovementsController> logger, 
            MovementService movementService){
            _logger = logger;
            _movementService = movementService;
        }


        [HttpGet]
        [Route("/getallmovements")]
        public IEnumerable<Movement> GetAllMovements(){
            return _movementService.GetMovementsForFeed();
        }

        [HttpPost]
        [ValidateModel]
        [Route("/createmovement")]
        public ResponseDto CreateProduct([FromBody] CreateMovementRequestDto dto){
            return new ResponseDto(){
                Message = "Movement created successfully",
                ResponseData = _movementService.CreateMovement(dto.Product_Sku, 
                                                            dto.MovementType,
                                                            dto.Quantity)
            };            
        }

        [HttpDelete]
        [ValidateModel]
        [Route("/deletemovement/{movement_id}/{product_sku}")]
        public ResponseDto DeleteMovement([FromRoute] Guid movement_id,
                                          [FromRoute] string product_sku){
            return new ResponseDto(){
                Message = "Movement deleted successfully",
                ResponseData = _movementService.DeleteMovement(movement_id, product_sku)
            };            
        }
    }
}