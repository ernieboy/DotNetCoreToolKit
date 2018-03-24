using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DotNetCoreToolKit.Web.ViewComponents
{
    public class ListPagerViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(
           string controllerName,
           string controllerAction,
           int totalNumberOfItems,
           int pageNumber,
           int pageSize,
           int totalNumberOfPages,
           int offset,
           int offsetUpperBound,
           string sortCol,
           string sortDir,
           string searchTerms)
        {
            ViewBag.url = $"{Url.Content("~/")}{controllerName}/{controllerAction}";

            return await Task.FromResult(View());
        }
    }
}
