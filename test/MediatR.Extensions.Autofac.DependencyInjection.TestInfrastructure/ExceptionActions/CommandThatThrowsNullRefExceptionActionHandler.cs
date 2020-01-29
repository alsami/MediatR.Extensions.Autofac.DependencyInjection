using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.Commands;
using MediatR.Pipeline;

namespace MediatR.Extensions.Autofac.DependencyInjection.TestInfrastructure.ExceptionActions
{
    public class CommandThatThrowsNullRefExceptionActionHandler : IRequestExceptionAction<CommandThatThrowsNullRefException,NullReferenceException>
    {
        public static int CallCount = 0;
        

        public Task Execute(CommandThatThrowsNullRefException request, NullReferenceException exception,
            CancellationToken cancellationToken)
        {
            CallCount++;
            return Task.CompletedTask;
        }
    }
}