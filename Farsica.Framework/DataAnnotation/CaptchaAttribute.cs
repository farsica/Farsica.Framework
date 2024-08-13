namespace Farsica.Framework.DataAnnotation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val", "true"));

            var msg = FormatErrorMessage(Globals.GetLocalizedDisplayName(context.ModelMetadata.ContainerType?.GetProperty(context.ModelMetadata.Name)));
            _ = context.Attributes.AddIfNotContains(new KeyValuePair<string, string>("data-val-captcha", Data.Error.FormatMessage(msg)));
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (validationContext?.GetRequiredService<ICookieProvider>().TryGetValue(SecurityExtensions.Captcha, out string? cookieData) != true)
            {
                return new ValidationResult(ErrorMessage);
            }

            var hashCookie = cookieData?.Split(Constants.DelimiterAlternate.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (hashCookie?.Length != 2)
            {
                return new ValidationResult(ErrorMessage);
            }

            var ticks = Globals.ValueOf<long?>(hashCookie[0]);
            if (!ticks.HasValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            if (Math.Abs(DateTime.UtcNow.Ticks - ticks.Value) > RequestMaxAgeInSeconds)
            {
                return new ValidationResult(ErrorMessage);
            }

            var hashValue = SecurityExtensions.Encrypt((string?)value);

            return Equals(hashCookie[1], hashValue) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}
