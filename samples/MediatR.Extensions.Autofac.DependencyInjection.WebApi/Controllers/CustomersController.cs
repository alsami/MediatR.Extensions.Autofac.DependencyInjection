using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Commands;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Dto;
using MediatR.Extensions.Autofac.DependencyInjection.Shared.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MediatR.Extensions.Autofac.DependencyInjection.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator mediator;

        public CustomersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /**
         * USE FOLLOWING JSON USING POSTMAN FOR INSTANCE
         * {
         *     "id": "5bc03b1b-44e3-4335-b30e-6c184055de30",
         *     "name": "google"
         * }
         */
        [HttpPost]
        public async Task<IActionResult> Create(CancellationToken cancellationToken,
            [FromBody] CustomerAddCommand command)
        {
            await this.mediator.Send(command, cancellationToken);

            return this.NoContent();
        }

        /**
         * USE FOLLOWING VALID ID: 5bc03b1b-44e3-4335-b30e-6c184055de30
         * USE FOLLOWING INVALID ID: 5bc03b1b-44e3-4335-b30e-6c184055de3c
         */
        [HttpGet("{id:Guid}")]
        public async Task<CustomerDto> Get(CancellationToken cancellationToken, Guid id)
        {
            var customer = await this.mediator.Send(new CustomerLoadQuery(id), cancellationToken);

            return customer;
        }
    }
}