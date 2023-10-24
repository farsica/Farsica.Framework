namespace Farsica.Framework.TimeZone
{
    using System.Collections.Generic;
    using Farsica.Framework.Data;
    using Farsica.Framework.DataAnnotation;

    [Injectable]
    public interface ITimeZoneProvider
    {
        IEnumerable<TimeZoneDto>? GetTimeZones();
    }
}
