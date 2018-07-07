using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using MediatR.Extensions.Autofac.DepdencyInjection.Samples.WebApi.Filter;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Samples.Shared.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Extensions.Autofac.DepdencyInjection.Samples.WebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.Filters.Add(typeof(CustomerNotFoundExceptionFilter)));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<CustomersRepository>()
                .As<ICustomersRepository>()
                .SingleInstance();
            
            builder.AddMediatR();
        }
    }
}