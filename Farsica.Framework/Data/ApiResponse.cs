namespace Farsica.Framework.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public struct ApiResponse<T>
    {
        public ApiResponse()
        {
        }

        public ApiResponse(ModelStateDictionary modelState)
        {
            if (modelState.ErrorCount == 0)
            {
                return;
            }

            List<Error>? lst = new(modelState.ErrorCount);
            foreach (var key in modelState.Keys)
            {
                var value = modelState[key];
                if (value?.Errors is null)
                {
                    continue;
                }

                for (int i = 0; i < value.Errors.Count; i++)
                {
                    lst.Add(new Error
                    {
                        Value = value.RawValue,
                        Reference = key,
                        Message = value.Errors[i].ErrorMessage,
                    });
                }
            }

            Errors = lst;
        }

        public ApiResponse(IEnumerable<Error>? errors)
        {
            Errors = errors;
        }

        public T? Data { get; set; }

        public string? Message { get; set; }

        public string? Action { get; set; }

        public readonly bool Succeeded => Errors is null || Errors.Any() is false;

        public IEnumerable<Error>? Errors { get; set; }
    }
}
