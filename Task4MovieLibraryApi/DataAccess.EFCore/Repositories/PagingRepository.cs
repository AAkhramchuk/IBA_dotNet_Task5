
using Domain.Entities;
using Domain.Interfaces;
using Domain.Entities.Wrappers;

namespace DataAccess.EFCore.Repositories
{
    /// <summary>
    /// Used to implement paging logic
    /// </summary>
    public class PagingRepository : IPagingRepository<Movie>
    {
        private readonly IUriService _uriService;

        public PagingRepository(IUriService uriService)
        {
            _uriService = uriService;
        }

        /// <summary>
        /// Paginate logic
        /// </summary>
        /// <typeparam name="Movie">Data model</typeparam>
        /// <param name="pagedData">A portion of data on the page</param>
        /// <param name="validFilter">Filter deffinition</param>
        /// <param name="totalRecords">The total number of records</param>
        /// <param name="uriService">URI service functionality</param>
        /// <param name="route">A route</param>
        /// <returns>Paged response data</returns>
        public PagedResponse<List<Movie>> CreatePagedReponse<Movie>(List<Movie> pagedData
                                                                    , PagingFilter validFilter
                                                                    , int totalRecords
                                                                    , IUriService uriService
                                                                    , string route)
        {
            var response = new PagedResponse<List<Movie>>(pagedData
                                                          , validFilter.PageNumber
                                                          , validFilter.PageSize);
            // Calculates the total page number
            int totalPages = (totalRecords + validFilter.PageSize - 1) / validFilter.PageSize;
            // Fills response data with the next page number
            if (validFilter.PageNumber >= 1 && validFilter.PageNumber < totalPages)
            {
                response.NextPage = _uriService.GetPageUri(new(validFilter.PageNumber + 1
                                                               , validFilter.PageSize)
                                                           , route);
            }
            else // ... assign null value instead
            {
                response.NextPage = null;
            }
            // Fills response data with the previous page number
            if (validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= totalPages)
            {
                response.PreviousPage = _uriService.GetPageUri(new(validFilter.PageNumber - 1
                                                                   , validFilter.PageSize)
                                                               , route);
            }
            else // ... assign null value instead
            {
                response.PreviousPage = null;
            }
            // Get the first page number
            response.FirstPage    = _uriService.GetPageUri(new(1, validFilter.PageSize), route);
            // Get the last page number
            response.LastPage     = _uriService.GetPageUri(new(totalPages, validFilter.PageSize), route);
            // Assign the total page number
            response.TotalPages   = totalPages;
            // Assign the number of total records on a page
            response.TotalRecords = totalRecords;

            return response;
        }
    }
}
