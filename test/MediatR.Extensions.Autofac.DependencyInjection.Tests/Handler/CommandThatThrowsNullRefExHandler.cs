using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Tests.Commands;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Handler;

public class CommandThatThrowsNullRefExHandler : IRequestHandler<CommandThatThrowsNullRefException, object>
{
    public Task<object> Handle(CommandThatThrowsNullRefException request, CancellationToken cancellationToken)
    {
        throw new NullReferenceException();
    }
}