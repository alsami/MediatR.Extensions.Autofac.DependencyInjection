using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;
using MediatR.Pipeline;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.ExceptionHandler;

public class NonSpecificExceptionHandler : IRequestExceptionHandler<CommandThatThrowsArgumentException, object, Exception>
{
    public static int CallCount = 0;
    public static DateTime CallTime;
        
    public Task Handle(
        CommandThatThrowsArgumentException request,
        Exception exception,
        RequestExceptionHandlerState<object> state,
        CancellationToken cancellationToken)
    {
        CallTime = DateTime.UtcNow;
        CallCount++;
        state.SetHandled(new object());
        return Task.CompletedTask;
    }
}