using DotNetCoreToolKit.Library.Web.ViewModels;
using System.Collections.Generic;

namespace DotNetCoreToolKit.Web.Models
{
    public class SuplliersListingPageViewModel  : BasePagedListingViewModel
    {
        public SuplliersListingPageViewModel()
        {
            Items = new List<SupplierViewModel>();
        }
        public IEnumerable<SupplierViewModel> Items { get; set; }   
    }

    public class SupplierViewModel
    {
        public int SupplierKey { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }    
    }
}
