using IdentityServer;
using IdentityServer.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityDbContextConnection' not found.");

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddControllersWithViews();
/*
// Identity
builder.Services
    .AddDbContext<IdentityDbContext>(options =>
        options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly))
    );

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();
*/
builder.Services
    // Enables Identity server
    .AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseSuccessEvents = true;
    })
    // Enables Client model
    .AddInMemoryClients(Config.Clients)
    // Enables API scope model
    .AddInMemoryApiScopes(Config.ApiScopes)
    // Enables API resource model
    .AddInMemoryApiResources(Config.ApiResources())
    // Enables Identity resource model
    .AddInMemoryIdentityResources(Config.IdentityResources)
    // Enables Test users model
    .AddTestUsers(Config.TestUsers)
    // Enables signing credential
    .AddDeveloperSigningCredential(persistKey: false);
    //.AddAspNetIdentity<ApplicationUser>();// Identity

builder.Services
    // add CORS policy for non-IdentityServer endpoints
    .AddCors(options =>
    {
        options.AddPolicy("api", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    })
    // demo versions (never use in production)
    .AddTransient<IRedirectUriValidator, DemoRedirectValidator>()
    .AddTransient<ICorsPolicyService, DemoCorsPolicy>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("api");

app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();;
app.UseAuthorization();

/*
ConfigurationManager configuration = builder.Configuration;
IServiceProvider serviceProvider = builder.Services.BuildServiceProvider();

//initializing custom roles 
var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
string[] roleNames = { "Admin", "User", "Guest" };
IdentityResult roleResult;

foreach (var roleName in roleNames)
{
    var roleExist = await roleManager.RoleExistsAsync(roleName);
    if (!roleExist)
    {
        //create the roles and seed them to the database: Question 1
        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    // Creates default users 
    var defaultUser = new ApplicationUser
    {
        UserName = configuration[$"AppSettings:{roleName}Name"],
        Email = configuration[$"AppSettings:{roleName}Email"],
    };
    string userPWD = configuration[$"AppSettings:{roleName}Password"];
    var user = await userManager.FindByEmailAsync(configuration[$"AppSettings:{roleName}Email"]);
    if (user == null)
    {
        var createDefaultUser = await userManager.CreateAsync(defaultUser, userPWD);
        if (createDefaultUser.Succeeded)
        {
            //here we tie the new user to the role
            await userManager.AddToRoleAsync(defaultUser, roleName);
        }
    }
}
*/

// Adds endpoints for controler actions
app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

app.Run();
