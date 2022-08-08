using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Entities.Wrappers;
using Domain.Interfaces;
using DataAccess.EFCore.Repositories;
using DataAccess.EFCore;
using DataAccess.EFCore.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using DataAccess.EFCore.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

// Enables JWT-Bearer authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

// Authorization policy
builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy("movieAPI", policy =>
            //policy.RequireClaim("scope", "movieAPI"));
            policy.RequireClaim("client_id", "movieAPI", "movieMVC"));
    });

// Enables DB context service
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();

// Repositories
#region Repositories
builder.Services
    // Enables URI service for specific HTTP request determination
    .AddTransient<IUriService>(p =>
    {
        IHttpContextAccessor accessor = p.GetRequiredService<IHttpContextAccessor>();
        HttpRequest request = accessor.HttpContext.Request;
        string uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());

        return new UriService(uri);
    })
    // Some repository functions for work with Movie table
    .AddTransient<IGenericRepository<Movie>, GenericRepository<Movie>>()
    // Add two separate services for the response includes only single record
    .AddTransient<Response<Movie>>(ResponseFactory<Movie>.Create)
    // ... and another one for the list of records
    .AddTransient<Response<List<Movie>>>(ResponseFactory<List<Movie>>.Create)
    // Paging repository functions
    .AddTransient<IPagingRepository<Movie>, PagingRepository>()
    // Unit of work repository pattern helps to create a separate database context for each transaction
    .AddTransient<IUnitOfWork, UnitOfWork>();
#endregion

#if DEBUG
builder.Services.AddSwaggerGen();
#endif

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

#if DEBUG
    app.UseSwagger();
    app.UseSwaggerUI();
#endif
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Add endpoint in the midleware for the controllers, require authorization
app.UseEndpoints(endpoints => endpoints.MapControllers().RequireAuthorization("movieAPI"));

app.Run();