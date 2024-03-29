﻿using MediatR.Extensions.Autofac.DependencyInjection.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MediatR.Extensions.Autofac.DependencyInjection.WebApi.Filter;

public class CustomerNotFoundExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (!(context.Exception is CustomerNotFoundException))
        {
            return;
        }

        context.ExceptionHandled = true;

        context.Result = new NotFoundObjectResult(new
        {
            context.Exception.Message,
            context.Exception.StackTrace
        });
    }
}