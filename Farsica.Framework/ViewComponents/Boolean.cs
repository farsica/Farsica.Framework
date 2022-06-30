namespace Farsica.Framework.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent(Name = "frb-bool")]
    public class Boolean : ViewComponent
    {
        public IViewComponentResult Invoke(bool? model) => View(model);
    }
}
