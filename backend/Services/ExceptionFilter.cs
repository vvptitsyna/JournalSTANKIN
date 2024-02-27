using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JournalAPI.Services
{
    public class ExceptionFilter:IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new JsonResult(new { Error = context.Exception.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            
        }
    }
}
