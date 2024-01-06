namespace Farsica.Framework.TimeZone
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Farsica.Framework.Caching;
    using Farsica.Framework.Data;
    using static Farsica.Framework.Core.Constants;

    public class TimeZoneProvider(Lazy<ICacheProvider> cacheProvider) : ITimeZoneProvider
    {
        private readonly Lazy<ICacheProvider> cacheProvider = cacheProvider;

        public IEnumerable<TimeZoneDto>? GetTimeZones()
        {
            return cacheProvider.Value.Get(TimeZoneIdClaim, () =>
                TimeZoneInfo.GetSystemTimeZones().Select(t => new TimeZoneDto
                {
                    Id = t.Id,
                    BaseUtcOffset = t.BaseUtcOffset,
                    DisplayName = t.DisplayName,
                }));
        }
    }
}
