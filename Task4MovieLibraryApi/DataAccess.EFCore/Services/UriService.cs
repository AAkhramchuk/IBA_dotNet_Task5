using Microsoft.AspNetCore.WebUtilities;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Entities.Wrappers;

namespace DataAccess.EFCore.Services
{
    /// <summary>
    /// URI service repository
    /// </summary>
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        /// <summary>
        /// URI constructor
        /// </summary>
        /// <param name="filter">URI filter parameters</param>
        /// <param name="route">URI route</param>
        /// <returns>URI unique sequence</returns>
        public Uri GetPageUri(PagingFilter filter, string route)
        {
            Uri endpointUri = new (string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(endpointUri.ToString()
                                                          , "pageNumber"
                                                          , filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri
                                                      , "pageSize"
                                                      , filter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
        /*
        public object ParseResponse<T>(Dictionary<string, object> record)
        {
            object obj = Activator.CreateInstance(typeof(T));

            foreach (var pair in record)
            {
                var property = typeof(T).GetProperty(pair.Key);
                if (property == null) continue;

                object? value = pair.Value;
                
                //if (value is Dictionary<string, TValue> dictionary)
                //{
                //    value = ParseResponse<string, TValue> (dictionary);
                //}

                property.SetValue(obj, value, null);
            }
            return obj;
        }
        */
    }
}
