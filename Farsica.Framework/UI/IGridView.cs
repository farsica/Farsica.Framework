namespace Farsica.Framework.UI
{
    using System.Collections.Generic;

    public interface IGridView<TModel, TSearch>
        where TModel : class
        where TSearch : class
    {
        TSearch Search { get; set; }

        IEnumerable<TModel> Model { get; set; }
    }
}
