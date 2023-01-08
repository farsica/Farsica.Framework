namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using Farsica.Framework.Captcha;
    using Farsica.Framework.Cookie;
    using Farsica.Framework.Core;
    using Farsica.Framework.Core.Extensions.Collections.Generic;
    using Farsica.Framework.Resources;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Microsoft.Extensions.DependencyInjection;

    public sealed class CaptchaAttribute : ValidationAttribute, IClientModelValidator
    {
        internal const long RequestMaxAgeInSeconds = TimeSpan.TicksPerMinute * 5; // 5 mins

        public CaptchaAttribute()
        {
            ErrorMessageResourceName = nameof(GlobalResource.Validation_Expression);
        }

        public override bool IsValid(object? value)
        {
            if (!ValidationContext.GetRequiredService<ICookieProvider>().TryGetValue(SecurityExtensions.Captcha, out string? cookieData))
            {
                return false;
            }

            var hashCookie = cookieData.Split(Constants.DelimiterAlternate.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (hashCookie.Length != 2)
            {
                return false;
            }

            var ticks = Globals.ValueOf<long?>(hashCookie[0]);
            if (!ticks.HasValue)
            {
                return false;
            }

            if (Math.Abs(DateTime.UtcNow.Ticks - ticks.Value) > RequestMaxAgeInSeconds)
            {
                return false;
            }

            var hashValue = SecurityExtensions.Encrypt((string)value);

            return Equals(hashCookie[1], hashValue);
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>($"data-val-captcha", FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType.GetProperty(context.ModelMetadata.Name)))));
        }
    }
}
