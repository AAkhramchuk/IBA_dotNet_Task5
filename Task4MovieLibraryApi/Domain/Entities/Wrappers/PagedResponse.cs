namespace Domain.Entities.Wrappers
{
    /// <summary>
    /// Based on common response types this one was created to show
    /// a response supplemented with information about the received page
    /// </summary>
    /// <typeparam name="T">Any data type</typeparam>
    public class PagedResponse<T> : Response<T>
    {
        // The number of a page in total
        public int PageNumber { get; set; }
        // The number of records on the page
        public int PageSize { get; set; }
        // The first page number
        public Uri? FirstPage { get; set; }
        // The last page number
        public Uri? LastPage { get; set; }
        // Total number of pages
        public int TotalPages { get; set; }
        // Total number of records 
        public int TotalRecords { get; set; }
        // The next page number
        public Uri? NextPage { get; set; }
        // The previous page number
        public Uri? PreviousPage { get; set; }

        /// <summary>
        /// Paged response pattern
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            Message = null;
            Succeeded = true;
            Errors = null;
        }
    }
}
