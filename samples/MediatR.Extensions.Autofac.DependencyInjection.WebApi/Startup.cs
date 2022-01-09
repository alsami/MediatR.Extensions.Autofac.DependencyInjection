using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Commands;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Repositories;
using MediatR.Extensions.Autofac.DependencyInjection.WebApi.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MediatR.Extensions.Autofac.DependencyInjection.WebApi;

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

        builder.RegisterMediatR(typeof(CustomerAddCommand).Assembly);
    }
}