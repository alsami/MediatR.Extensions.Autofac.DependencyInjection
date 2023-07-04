using System;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Services;

public class UtcDateTimeConverterService : IDateTimeConverterService
{
    public DateTime ConvertDateTime(DateTime sourceDateTime)
    {
        return sourceDateTime.ToUniversalTime();
    }
}