using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands
{
    public class SampleNotification : INotification
    {
        public string Message { get; }

        public SampleNotification(string message)
        {
            Message = message;
        }
    }
}