namespace Farsica.Framework.Mvc.ViewFeatures
{
    using System;
    using Microsoft.AspNetCore.Mvc.Rendering;

    internal static class NameAndIdProvider
    {
        private static readonly object PreviousNameAndIdKey = typeof(PreviousNameAndId);

        public static string? CreateSanitizedId(ViewContext? viewContext, string? fullName, string? invalidCharReplacement)
        {
            if (viewContext is null)
            {
                throw new ArgumentNullException(nameof(viewContext));
            }

            if (invalidCharReplacement is null)
            {
                throw new ArgumentNullException(nameof(invalidCharReplacement));
            }

            if (string.IsNullOrEmpty(fullName))
            {
                return string.Empty;
            }

            // Check cache to avoid whatever TagBuilder.CreateSanitizedId() may do.
            var items = viewContext.HttpContext.Items;
            PreviousNameAndId? previousNameAndId = null;
            if (items.TryGetValue(PreviousNameAndIdKey, out object? previousNameAndIdObject) && (previousNameAndId = (PreviousNameAndId?)previousNameAndIdObject) != null &&
                string.Equals(previousNameAndId.FullName, fullName, StringComparison.Ordinal))
            {
                return previousNameAndId?.SanitizedId;
            }

            var sanitizedId = TagBuilder.CreateSanitizedId(fullName, invalidCharReplacement);

            if (previousNameAndId is null)
            {
                // Do not create a PreviousNameAndId when TagBuilder.CreateSanitizedId() only examined fullName.
                if (string.Equals(fullName, sanitizedId, StringComparison.Ordinal))
                {
                    return sanitizedId;
                }

                previousNameAndId = new PreviousNameAndId();
                items[PreviousNameAndIdKey] = previousNameAndId;
            }

            previousNameAndId.FullName = fullName;
            previousNameAndId.SanitizedId = sanitizedId;

            return previousNameAndId.SanitizedId;
        }

        public static void GenerateId(ViewContext viewContext, TagBuilder tagBuilder, string? fullName, string? invalidCharReplacement)
        {
            if (viewContext is null)
            {
                throw new ArgumentNullException(nameof(viewContext));
            }

            if (tagBuilder is null)
            {
                throw new ArgumentNullException(nameof(tagBuilder));
            }

            if (invalidCharReplacement is null)
            {
                throw new ArgumentNullException(nameof(invalidCharReplacement));
            }

            if (string.IsNullOrEmpty(fullName))
            {
                return;
            }

            if (!tagBuilder.Attributes.ContainsKey("id"))
            {
                var sanitizedId = CreateSanitizedId(viewContext, fullName, invalidCharReplacement);

                // Duplicate check for null or empty to cover the corner case where fullName contains only invalid
                // characters and invalidCharReplacement is empty.
                if (!string.IsNullOrEmpty(sanitizedId))
                {
                    tagBuilder.Attributes["id"] = sanitizedId;
                }
            }
        }

        public static string? GetFullHtmlFieldName(ViewContext? viewContext, string? expression)
        {
            var htmlFieldPrefix = viewContext?.ViewData.TemplateInfo.HtmlFieldPrefix;
            if (string.IsNullOrEmpty(expression))
            {
                return htmlFieldPrefix;
            }

            if (string.IsNullOrEmpty(htmlFieldPrefix))
            {
                return expression;
            }

            // Need to concatenate. See if we've already done that.
            var items = viewContext?.HttpContext.Items;
            PreviousNameAndId? previousNameAndId = null;
            if (items.TryGetValue(PreviousNameAndIdKey, out object? previousNameAndIdObject) && (previousNameAndId = (PreviousNameAndId?)previousNameAndIdObject) is not null &&
                string.Equals(previousNameAndId.HtmlFieldPrefix, htmlFieldPrefix, StringComparison.Ordinal) && string.Equals(previousNameAndId.Expression, expression, StringComparison.Ordinal))
            {
                return previousNameAndId?.OutputFullName;
            }

            if (previousNameAndId is null)
            {
                previousNameAndId = new PreviousNameAndId();
                items[PreviousNameAndIdKey] = previousNameAndId;
            }

            previousNameAndId.HtmlFieldPrefix = htmlFieldPrefix;
            previousNameAndId.Expression = expression;
            if (expression.StartsWith("[", StringComparison.Ordinal))
            {
                // The expression might represent an indexer access, in which case  with a 'dot' would be invalid.
                previousNameAndId.OutputFullName = htmlFieldPrefix + expression;
            }
            else
            {
                previousNameAndId.OutputFullName = htmlFieldPrefix + "." + expression;
            }

            return previousNameAndId.OutputFullName;
        }

        private class PreviousNameAndId
        {
            // Cached ambient input for NameAndIdProvider.GetFullHtmlFieldName(). TemplateInfo.HtmlFieldPrefix may
            // change during the lifetime of a ViewContext.
            public string? HtmlFieldPrefix { get; set; }

            // Cached input for NameAndIdProvider.GetFullHtmlFieldName().
            public string? Expression { get; set; }

            // Cached return value for NameAndIdProvider.GetFullHtmlFieldName().
            public string? OutputFullName { get; set; }

            // Cached input for NameAndIdProvider.CreateSanitizedId(). Since IHtmlHelper.GenerateIdFromName() is
            // available to all, there is no guarantee this is equal to OutputFullName when CreateSanitizedId() is
            // called.
            public string? FullName { get; set; }

            // Cached return value for NameAndIdProvider.CreateSanitizedId().
            public string? SanitizedId { get; set; }
        }
    }
}
