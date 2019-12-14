using System;

namespace MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Exceptions
{
    public class CustomerAlreadyExistsException : Exception
    {
        public CustomerAlreadyExistsException(string message = "The customer already exists") : base(message)
        {
        }
    }
}