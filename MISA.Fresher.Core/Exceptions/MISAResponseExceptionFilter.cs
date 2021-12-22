using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Exceptions
{
    public class MISAResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is MISAResponseNotValidException httpResponseException)
            {
                var result = new
                {
                    devMsg = MISA.Fresher.Core.Properties.Resources.ExceptionDevMsgError,
                    userMsg = MISA.Fresher.Core.Properties.Resources.ExceptionUserMsgError,
                    data = httpResponseException.Value,
                    moreInfo = ""
                };

                context.Result = new ObjectResult(result)
                {
                    StatusCode = 400
                };

                context.ExceptionHandled = true;
            }

            else if(context.Exception != null)
            {
                var result = new
                {
                    devMsg = context.Exception.Message,
                    userMsg = MISA.Fresher.Core.Properties.Resources.ExceptionUserMsgError,
                    data = DBNull.Value,
                    moreInfo = ""
                };

                context.Result = new ObjectResult(result)
                {
                    StatusCode = 500
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
