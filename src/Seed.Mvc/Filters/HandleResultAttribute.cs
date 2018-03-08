using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Seed.Modules.Exceptions;
using Seed.Mvc.Models;

namespace Seed.Mvc.Filters
{
    /// <summary>
    /// 处理控制器返回值，将结果格式化返回
    /// </summary>
    public class HandleResultAttribute : ActionFilterAttribute
    {
        public bool AnyException { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anyException">处理任何异常</param>
        public HandleResultAttribute(bool anyException = false)
        {
            AnyException = anyException;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (context.Exception != null)
            {
                if (AnyException && !context.Exception.GetType().Equals(typeof(GeneralOperateException)))
                {
                    return;
                }

                context.Result = new ObjectResult(new ApiResult(false)
                {
                    Message = context.Exception.Message
                });

                context.Result.ExecuteResultAsync(context);
            }
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);

            if (context.Result is ObjectResult)
            {
                context.Result = new ObjectResult(new ApiResult()
                {
                    Data = ((ObjectResult)context.Result).Value
                });
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new ApiResult());
            }
        }
    }
}