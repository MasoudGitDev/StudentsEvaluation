using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Files.Exceptions;
using Shared.Files.Models;

namespace Server.MainApp.Attributes;

public class ResultExceptionAttribute : ExceptionFilterAttribute {
    public override void OnException(ExceptionContext context) {
        if(context.Exception is CustomException customException) {
            context.Result = new JsonResult(Result.Failed(customException.Description));    
        }
        else {
            context.Result = new JsonResult(Result.Failed(context.Exception.Message));
        }
    }
}
