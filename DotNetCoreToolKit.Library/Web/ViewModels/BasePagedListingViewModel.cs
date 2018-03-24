using DotNetCoreToolKit.Library.Models.Pagination;

namespace DotNetCoreToolKit.Library.Web.ViewModels
{
    public abstract class BasePagedListingViewModel
    {
        public PaginationData PaginationData { get; set; }
    }
}
