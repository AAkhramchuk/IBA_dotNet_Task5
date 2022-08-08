using Domain.Entities;
using Domain.Entities.Wrappers;

namespace Domain.Interfaces
{
    public interface IPagingRepository<T> where T : class
    {
        PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData
                                                     , PagingFilter validFilter
                                                     , int totalRecords
                                                     , IUriService uriService
                                                     , string route);
    }
}
