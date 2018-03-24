using DotNetCoreToolKit.Library.Abstractions;
using DotNetCoreToolKit.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DotNetCoreToolKit.Web.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IPagedResultSet _pagedResultSet;

        public DatabaseController(IConfiguration configuration, IPagedResultSet pagedResultSet)
        {
            _configuration = configuration;
            _pagedResultSet = pagedResultSet;
        }
        public async Task<IActionResult > Suppliers(int? pageNumber, int? pageSize,
            string sortDir = "ASC", string searchTerms = "", string sortCol = "Supplier")    
        {
            string query = _configuration.GetValue<string>("Queries:SuppliersQuery");
            var result = await _pagedResultSet.GetPagedList<SupplierViewModel>(query, pageNumber, pageSize, searchTerms,sortCol, sortDir);
            var model = new SuplliersListingPageViewModel { Items = result.items, PaginationData = result.paginationData };
            model.PaginationData.ControllerName = "Database";
            model.PaginationData.ControllerAction = "Suppliers";

            return View(model);
        }
    }
}