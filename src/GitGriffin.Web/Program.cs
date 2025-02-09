using System.Security.Claims;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using GitGriffin.Web.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/access-denied";
    options.SlidingExpiration = true;
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "Github";
    })
    .AddCookie()
    .AddGitHub(options =>
    {
        options.ClientId = builder.Configuration["GitHub:ClientId"] ?? throw new Exception("missing github clientId");
        options.ClientSecret = builder.Configuration["GitHub:ClientSecret"] ?? throw new Exception("missing github clientSecret");
        options.CallbackPath = new PathString("/signin-github");

        options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
        options.TokenEndpoint = "https://github.com/login/oauth/access_token";
        options.UserInformationEndpoint = "https://api.github.com/user";

        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
        options.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
        options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

        options.Scope.Add("read:user");
        options.Scope.Add("read:org");
        options.Scope.Add("offline_access");
        options.Scope.Add("repo");

        options.SaveTokens = true;

        options.Events.OnCreatingTicket = async (context) =>
        {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", context.AccessToken);

            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

            context.RunClaimActions(user.RootElement);
        };
    });

builder.Services.AddHttpClient();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();
