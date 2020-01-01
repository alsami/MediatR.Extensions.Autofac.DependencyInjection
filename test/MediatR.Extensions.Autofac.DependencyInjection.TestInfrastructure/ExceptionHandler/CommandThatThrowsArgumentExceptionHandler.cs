using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands;
using MediatR.Pipeline;

namespace MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.ExceptionHandler
{
    public class CommandThatThrowsArgumentExceptionHandler : IRequestExceptionHandler<CommandThatThrowsArgumentException, object, ArgumentException>
    {
        public static int CallCount = 0;
        public static DateTime CallTime;

        public Task Handle(CommandThatThrowsArgumentException request, ArgumentException exception,
            RequestExceptionHandlerState<object> state, CancellationToken cancellationToken)
        {
            CommandThatThrowsArgumentExceptionHandler.CallTime = DateTime.UtcNow;
            CommandThatThrowsArgumentExceptionHandler.CallCount++;
            return Task.CompletedTask;
        }
    }
}