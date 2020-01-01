using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands;

namespace MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Handler
{
    public class CommandThatThrowsExceptionHandler : IRequestHandler<CommandThatThrowsArgumentException, object>
    {
        public Task<object> Handle(CommandThatThrowsArgumentException request, CancellationToken cancellationToken)
        {
            throw new ArgumentException();
        }
    }
}