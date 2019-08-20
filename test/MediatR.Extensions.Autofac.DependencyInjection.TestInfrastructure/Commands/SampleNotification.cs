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