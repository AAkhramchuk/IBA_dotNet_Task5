using Domain.Entities.Wrappers;

namespace DataAccess.EFCore.Repositories
{
    /// <summary>
    /// Used to create two response instances of any type
    /// </summary>
    /// <typeparam name="T">Any type</typeparam>
    public static class ResponseFactory<T>
    {
        public static Response<T> Create(IServiceProvider serviceProvider)
        {
            return new Response<T>();
        }
    }
}
