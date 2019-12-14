using Autofac;
using MediatR.Extensions.Autofac.DepdencyInjection.Samples.WebApi.Filter;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Commands;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MediatR.Extensions.Autofac.DepdencyInjection.Samples.WebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add(typeof(CustomerNotFoundExceptionFilter)))
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(builder => builder.MapControllers());
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<CustomersRepository>()
                .As<ICustomersRepository>()
                .SingleInstance();

            builder.AddMediatR(typeof(CustomerAddCommand).Assembly);
        }
    }
}