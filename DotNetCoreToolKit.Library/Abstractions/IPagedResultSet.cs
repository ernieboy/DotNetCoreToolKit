using DotNetCoreToolKit.Library.Models.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetCoreToolKit.Library.Abstractions
{
    interface IPagedResultSet
    {
        Task<(IEnumerable<T> items, PaginationData paginationData)> GetPagedList<T>(
            string initialSearchQuery, int? pageNumber, int? pageSize, string sortColumn, string sortDirection, string searchTermsCommaSeparated)       
           where T : class;
    }
}
