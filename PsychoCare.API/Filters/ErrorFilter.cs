using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PsychoCare.API.Models;
using PsychoCare.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PsychoCare.API.Filters
{
    public class ErrorFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Catches all exception throwed by services
        ///
        /// When throwed exception is of type MessageException then it's a message for user
        /// any other is an error
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (context.Exception is MessageException)
            {
                context.Result = new JsonResult(new ErrorFilterResponse()
                {
                    IsError = false,
                    Message = context.Exception.Message
                });
            }
            else
            {
                context.Result = new JsonResult(new ErrorFilterResponse()
                {
                    IsError = true,
                    Error = context.Exception?.ToString()
                });
            }

            context.ExceptionHandled = true;
        }
    }
}