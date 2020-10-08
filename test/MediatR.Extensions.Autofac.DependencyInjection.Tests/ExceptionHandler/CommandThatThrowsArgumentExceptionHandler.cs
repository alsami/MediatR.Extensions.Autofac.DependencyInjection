using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;
using MediatR.Pipeline;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.ExceptionHandler
{
    public class CommandThatThrowsArgumentExceptionHandler : IRequestExceptionHandler<CommandThatThrowsArgumentException, object, ArgumentException>
    {
        public static int CallCount = 0;
        public static DateTime CallTime;

        public Task Handle(CommandThatThrowsArgumentException request, ArgumentException exception,
            RequestExceptionHandlerState<object> state, CancellationToken cancellationToken)
        {
            CallTime = DateTime.UtcNow;
            CallCount++;
            return Task.CompletedTask;
        }
    }
}