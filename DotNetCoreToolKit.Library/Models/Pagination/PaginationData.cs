namespace DotNetCoreToolKit.Library.Models.Pagination
{
    public class PaginationData
    {
        public int OffsetFromZero { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int OffsetUpperBound { get; set; }

        public int TotalNumberOfRecords { get; set; }

        public int TotalNumberOfPages { get; set; }

        public string SearchTermsCommaSeparated { get; set; }

        public string[] SortColumns { get; set; }   

        public string[] SortDirections { get; set; }  
    }
}
