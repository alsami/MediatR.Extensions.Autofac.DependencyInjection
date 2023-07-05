using System;
using System.Runtime.InteropServices;

namespace MediatR.Extensions.Autofac.DependencyInjection.Tests.Services;


public class EstDateTimeConverterService : IDateTimeConverterService
{
    public DateTime ConvertDateTime(DateTime sourceDateTime)
    {
        if (sourceDateTime.Kind == DateTimeKind.Unspecified)
        {
            throw new ArgumentException("The kind of the source date time is unspecified");
        }

        TimeZoneInfo estZone;
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            estZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            estZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
        }
        else
        {
            throw new PlatformNotSupportedException("The current platform is not supported");
        }

        DateTime targetDateTime;

        if (sourceDateTime.Kind == DateTimeKind.Utc)
        {
            targetDateTime = TimeZoneInfo.ConvertTimeFromUtc(sourceDateTime, estZone);
        }
        else
        {
            var utcDateTime = sourceDateTime.ToUniversalTime();
            targetDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, estZone);
        }

        return targetDateTime;
    }
}