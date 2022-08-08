using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using IdentityModel;
using Microsoft.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using Task5MovieApp.HttpHandler;
using Task5MovieApp.Services;
using Domain.Interfaces;
using DataAccess.EFCore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Task5MovieApp.Data;
using Task5MovieApp.Models;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("MovieAppContext");
// Add database context
builder.Services.AddDbContext<MovieDBContext>(options =>
    options.UseSqlServer(connectionString
        ?? throw new InvalidOperationException("Connection string 'MovieAppContext' not found.")));

// Add services to the container.
builder.Services
    .AddControllersWithViews();

builder.Services
    .AddScoped<IMovieService, MovieService>()
    .AddSingleton<UserDataViewModel>(); ;

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.Authority = "https://localhost:5001";

        options.ClientId = "movieMVC";
        options.ClientSecret = "secret";
        options.ResponseType = "code id_token";

        options.Scope.Add("openid");
        options.Scope.Add("profile");
        //options.Scope.Add("movieAPI");
        options.Scope.Add("movieMVC");
        options.Scope.Add("address");
        options.Scope.Add("email");
        options.Scope.Add("roles");

        options.ClaimActions.DeleteClaim("sid");
        options.ClaimActions.DeleteClaim("idp");
        options.ClaimActions.DeleteClaim("s_hash");
        options.ClaimActions.DeleteClaim("auth_time");
        options.ClaimActions.MapUniqueJsonKey("role", "role");

        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = JwtClaimTypes.GivenName,
            RoleClaimType = JwtClaimTypes.Role
        };
    });

builder.Services
    .AddTransient<AuthenticationDelegatingHandler>();

// Create an HttpClient used for accessing the Movie API through API Gateway
builder.Services
    .AddHttpClient("APIgatewayClient", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5004/");
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    })
    .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

// Create an HttpClient used for accessing the Identity Server
builder.Services
    .AddHttpClient("IdentityServerClient", client =>
    {
        client.BaseAddress = new Uri("https://localhost:5001/");
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    });

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    using (var context = scope.ServiceProvider.GetService<MovieDBContext>())
    {
        context.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();