using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Paging filter parameters
    /// </summary>
    public class PagingFilter
    {
        // The number of a page in total
        public int PageNumber { get; set; }
        // The number of records on the page
        public int PageSize { get; set; }

        // Default value
        public PagingFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public PagingFilter(int pageNumber, int pageSize)
        {
            // A page number could be not less than 1
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            // A page size could be not bigger than 100 records
            PageSize = pageSize > 100 ? 100 : pageSize;
        }
    }
}
