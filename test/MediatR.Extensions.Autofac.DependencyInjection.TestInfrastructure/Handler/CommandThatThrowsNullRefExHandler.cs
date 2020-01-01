using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands;

namespace MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Handler
{
    public class CommandThatThrowsNullRefExHandler : IRequestHandler<CommandThatThrowsNullRefException, object>
    {
        public Task<object> Handle(CommandThatThrowsNullRefException request, CancellationToken cancellationToken)
        {
            throw new NullReferenceException();
        }
    }
}