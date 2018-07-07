﻿using System;

namespace MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(string message = "The customer could not be found!") : base(message)
        {
            
        }
    }
}