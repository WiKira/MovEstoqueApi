using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MovEstApi.TransferModels;

namespace MovEstApi.Filters
{
    public class ValidateModel : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;
            string errorMessages = context.ModelState.Values
            .SelectMany(i => i.Errors.Select(e => e.ErrorMessage))
            .Aggregate((i, j) => i + "," + j);

            context.Result = new JsonResult(new ResponseDto{
                Message = errorMessages
            })
            {
                StatusCode = 400
            };
        }
    }
}