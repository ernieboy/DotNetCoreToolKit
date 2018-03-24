using DotNetCoreToolKit.Library.Abstractions;
using DotNetCoreToolKit.Library.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetCoreToolKit.Library.Implementations
{
    public class PagedResultSet : IPagedResultSet
    {
        private readonly string _connectionString;

        public PagedResultSet(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<(IEnumerable<T> items, PaginationData paginationData)> GetPagedList<T>(
            string initialSearchQuery, int? pageNumber, int? pageSize, string sortColumn, string sortDirection, string searchTermsCommaSeparated) where T : class
        {
            var query = initialSearchQuery;
         
            int pageIndex = pageNumber ?? 1;
            int sizeOfPage = pageSize ?? 10;
            if (pageIndex < 1) pageIndex = 1;
            if (sizeOfPage < 1) sizeOfPage = 5;

            var pagingPart = $" OFFSET {sizeOfPage} * ({pageIndex} - 1) ROWS FETCH NEXT {sizeOfPage} ROWS ONLY OPTION (RECOMPILE);";
            query += pagingPart;

            var countQuery = BuildCountQueryFromInitialQuery(initialSearchQuery, searchTermsCommaSeparated);
            int totalRecords = 0;

            int offsetFromStart = (pageIndex - 1) * sizeOfPage + 1;
            int offsetUpperBound = offsetFromStart + (sizeOfPage - 1);
            if (offsetUpperBound > totalRecords) offsetUpperBound = totalRecords;

            IEnumerable <T> list = null;
            using (var dbCon = new SqlConnection(_connectionString))
            {
                totalRecords = await dbCon.ExecuteScalarAsync<int>(countQuery);
                list = await dbCon.QueryAsync<T>(query);
            }
            var paginationData = new PaginationData
            {
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageSize = sizeOfPage,
                PageNumber = pageIndex,
                OffsetFromZero = offsetFromStart,
                OffsetUpperBound = offsetUpperBound,
                SearchTermsCommaSeparated = string.Join(",", searchTermsCommaSeparated.Select(i => i.ToString())),
                TotalNumberOfPages = (int)Math.Ceiling((double)totalRecords / sizeOfPage),
                TotalNumberOfRecords = totalRecords
            };
            return (list, paginationData);
        }

        private string BuildCountQueryFromInitialQuery(string initialSearchQuery, string searchTermsCommaSeparated)
        {


            var match = Regex.Match(initialSearchQuery, @"(?<=from\s)(.*)(?=\sorder\sby)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
            if (match.Success)
            {
                var countQuery   = $"SELECT COUNT(*) FROM {match.Captures[0].ToString().Trim()} " ;
                return countQuery;
            }
            throw new ArgumentException("Failed to build record cound query.");
        }
    }
}
