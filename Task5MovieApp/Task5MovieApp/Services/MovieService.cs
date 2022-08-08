using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Domain.Entities;
using Task5MovieApp.Models;
using IdentityServer4.Models;
using DataAccess.EFCore.Services;
using Domain.Interfaces;
using Domain.Entities.Wrappers;
using System.Net.Http.Headers;
using System.Text;

namespace Task5MovieApp.Services
{
    public class MovieService : IMovieService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovieService(IHttpClientFactory httpClientFactory
                            , IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<IEnumerable<Movie>?> GetMovies()
        {
            var content = await GetResponse("/api/movies/", HttpMethod.Get);
            var response = JsonConvert.DeserializeObject<PagedResponse<List<Movie>>>(content);
            if (response.Succeeded)
            {
                return response.Data;
            }
            return null;
            /*
            //////////////////////////
            //// WAY 1 :
            //
            //var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
            //
            //var request = new HttpRequestMessage(
            //    HttpMethod.Get,
            //    "/movies");
            //
            //var response = await httpClient.SendAsync(
            //    request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            //
            //response.EnsureSuccessStatusCode();
            //
            //var content = await response.Content.ReadAsStringAsync();
            //var movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            //return movieList;

            //////////////////////// //////////////////////// ////////////////////////
            // WAY 2 :

            // 1. "retrieve" our api credentials. This must be registered on Identity Server!
            var apiClientCredentials = new ClientCredentialsTokenRequest
            {
                Address = "https://localhost:5001/connect/token",

                ClientId = "movieMVC",
                ClientSecret = "secret",
                GrantType = "hybrid",

                // This is the scope our Protected API requires. 
                Scope = "movieMVC"
            };

            // creates a new HttpClient to talk to our IdentityServer (localhost:5001)
            var client = new HttpClient();

            // just checks if we can reach the Discovery document. Not 100% needed but..
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                return null; // throw 500 error
            }

            // 2. Authenticates and get an access token from Identity Server
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
            if (tokenResponse.IsError)
            {
                return null;
            }

            // Another HttpClient for talking now with our Protected API
            var apiclient = new HttpClient();

            // 3. Set the access_token in the request Authorization: Bearer <token>
            apiclient.SetBearerToken(tokenResponse.AccessToken);

            // 4. Send a request to our Protected API
            var response = await apiclient.GetAsync(uri);// "https://localhost:5002/api/movies");
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            var movieList = JsonConvert.DeserializeObject<PagedResponse<List<Movie>>>(content);
            //var response = _uriService.ParseResponse<PagedResponse<List<Movie>>>(content);
            return movieList.Data;
            */
        }

        public async Task<Movie?> GetMovie(int id)
        {
            var content = await GetResponse($"/api/movies/{id}", HttpMethod.Get);
            var response = JsonConvert.DeserializeObject<Response<Movie>>(content);
            if (response.Succeeded)
            {
                return response.Data;
            }
            return null;
        }

        public async Task<Movie?> CreateMovie(Movie movie)
        {
            var content = await GetResponse($"/api/movies", HttpMethod.Post, movie);
            var response = JsonConvert.DeserializeObject<Response<Movie>>(content);
            if (response.Succeeded)
            {
                return response.Data;
            }
            return null;
        }

        public async Task<Movie?> UpdateMovie(int id, Movie movie)
        {
            var content = await GetResponse($"/api/movies/{id}", HttpMethod.Put, movie);
            var response = JsonConvert.DeserializeObject<Response<Movie>>(content);
            if (response.Succeeded)
            {
                return response.Data;
            }
            return null;
        }

        public async Task<Movie?> DeleteMovie(int id)
        {
            var content = await GetResponse($"/api/movies/{id}", HttpMethod.Delete);
            var response = JsonConvert.DeserializeObject<Response<Movie>>(content);
            if (response.Succeeded)
            {
                return response.Data;
            }
            return null;
        }
        //PagedResponse<List<Movie>>
        public async Task<string> GetResponse(string route, HttpMethod method, Movie? movie = null)
        {
            // 1. "retrieve" our api credentials. This must be registered on Identity Server!
            var apiClientCredentials = new ClientCredentialsTokenRequest
            {
                Address = "https://localhost:5001/connect/token",

                ClientId = "movieMVC",
                ClientSecret = "secret",
                GrantType = "hybrid",

                // This is the scope our Protected API requires. 
                Scope = "movieMVC"
            };

            // creates a new HttpClient to talk to our IdentityServer (localhost:5001)
            using (var httpClient = _httpClientFactory.CreateClient("MovieAPIClient"))
            {
                // just checks if we can reach the Discovery document. Not 100% needed but..
                var disco = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");
                if (disco.IsError)
                {
                    return null; // throw 500 error
                }

                // 2. Authenticates and get an access token from Identity Server
                var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(apiClientCredentials);
                if (tokenResponse.IsError)
                {
                    return null;
                }

                httpClient.SetBearerToken(tokenResponse.AccessToken);

                // Serialize Movie for the request content data
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                httpClient.DefaultRequestHeaders.Accept.Add(contentType);
                var stringData = JsonConvert.SerializeObject(movie);
                StringContent contentData = new(stringData, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(
                    method,//HttpMethod.Delete,
                    "https://localhost:5002" + route)
                {
                    Content = contentData
                };
                //{ Content = new FormUrlEncodedContent(movie) }; // /api/movies/{id}");

                var response = await httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                //var movieList = JsonConvert.DeserializeObject<PagedResponse<List<Movie>>>(content);
                return content;
            };
        }

        public async Task<UserInfoViewModel> GetUserInfo()
        {
            var idpClient = _httpClientFactory.CreateClient("IdentityServerClient");

            var metaDataResponse = await idpClient.GetDiscoveryDocumentAsync();

            if (metaDataResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while requesting the access token");
            }

            var accessToken = await _httpContextAccessor
                .HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var userInfoResponse = await idpClient.GetUserInfoAsync(
               new UserInfoRequest
               {
                   Address = metaDataResponse.UserInfoEndpoint,
                   Token = accessToken
               });

            if (userInfoResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while getting user info");
            }

            var userInfoDictionary = new Dictionary<string, string>();

            foreach (var claim in userInfoResponse.Claims)
            {
                userInfoDictionary.Add(claim.Type, claim.Value);
            }

            return new UserInfoViewModel(userInfoDictionary);
        }
    }
}