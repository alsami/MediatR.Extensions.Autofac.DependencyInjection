using System;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Services;

public interface IDateTimeConverterService
{
    DateTime ConvertDateTime(DateTime sourceDateTime);
}