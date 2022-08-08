using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUriService
    {
        Uri GetPageUri(PagingFilter filter, string route);

        //object ParseResponse<T>(Dictionary<string, object> record);
    }
}