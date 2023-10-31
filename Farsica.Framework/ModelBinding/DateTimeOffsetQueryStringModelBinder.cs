namespace Farsica.Framework.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Farsica.Framework.Core;
    using Farsica.Framework.TimeZone;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.DependencyInjection;

    public class DateTimeOffsetQueryStringModelBinder : IModelBinder
    {
        public Task BindModelAsync([NotNull] ModelBindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext, nameof(bindingContext));

            var values = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
            List<DateTimeOffset> lst = new(values.Length);
            foreach (var item in values)
            {
                var dateTime = item.ValueOf<DateTime?>();
                if (!dateTime.HasValue)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    bindingContext.ModelState.AddModelError(bindingContext.FieldName, string.Format(Resources.GlobalResource.Validation_AttemptedValueIsInvalidAccessor, item, bindingContext.ModelName));
                    return Task.CompletedTask;
                }

                if (dateTime.Value.Kind == DateTimeKind.Unspecified)
                {
                    lst.Add(new DateTimeOffset(dateTime.Value, GetTimeSpan()));
                }
                else
                {
                    lst.Add(item.ValueOf<DateTimeOffset>());
                }

                TimeSpan GetTimeSpan()
                {
                    var timeZoneId = bindingContext.HttpContext?.User.FindFirstValue(Constants.TimeZoneIdClaim) ?? Constants.IranTimeZoneId;
                    return bindingContext.HttpContext?.RequestServices.GetRequiredService<ITimeZoneProvider>().GetTimeZones()?.FirstOrDefault(t => t.Id == timeZoneId)?.BaseUtcOffset ?? Constants.IranBaseUtcOffset;
                }
            }

            var isEnumerable = typeof(System.Collections.IEnumerable).IsAssignableFrom(bindingContext.ModelType);
            if (lst.Count > 0)
            {
                bindingContext.Result = isEnumerable ? ModelBindingResult.Success(lst) : ModelBindingResult.Success(lst[0]);
            }

            return Task.CompletedTask;
        }
    }
}
