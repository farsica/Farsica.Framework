namespace Farsica.Framework.Powershell
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class RouteCollection
    {
        private readonly List<(string PathTemplate, IPowershellDispatcher Dispatcher)> dispatchers = new();

        public void Add(string pathTemplate, IPowershellDispatcher dispatcher)
        {
            if (pathTemplate == null)
            {
                throw new ArgumentNullException(nameof(pathTemplate));
            }

            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            dispatchers.Add(new(pathTemplate, dispatcher));
        }

        public Tuple<IPowershellDispatcher, Match> FindDispatcher(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = "/";
            }
            else if (path.Length > 1)
            {
                path = path.TrimEnd('/');
            }

            for (int i = 0; i < dispatchers.Count; i++)
            {
                var pattern = dispatchers[i].PathTemplate;

                if (!pattern.StartsWith("^", StringComparison.OrdinalIgnoreCase))
                {
                    pattern = "^" + pattern;
                }

                if (!pattern.EndsWith("$", StringComparison.OrdinalIgnoreCase))
                {
                    pattern += "$";
                }

                var match = Regex.Match(path, pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (match.Success)
                {
                    return new Tuple<IPowershellDispatcher, Match>(dispatchers[i].Dispatcher, match);
                }
            }

            return null;
        }
    }
}