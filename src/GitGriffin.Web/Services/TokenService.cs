using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using GitGriffin.Web.Data;
using GitGriffin.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace GitGriffin.Web.Services;

public class TokenService(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory)
{
    public async Task<string> RefreshGitHubTokenAsync(string userId, string loginProvider)
    {
        var client = httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token");

        Debug.Assert(configuration["GitHub:ClientId"] != null);
        Debug.Assert(configuration["GitHub:ClientSecret"] != null);

        var currentToken = dbContext.UserOAuthTokens
            .FirstOrDefault(t => t.Provider == loginProvider && t.UserId == userId);

        if (currentToken is null)
        {
            currentToken = new UserOAuthToken
            {
                UserId = userId,
                Provider = loginProvider,
            };

            dbContext.UserOAuthTokens.Add(currentToken);
        }

        var parameters = new Dictionary<string, string>
        {
            { "client_id", configuration["GitHub:ClientId"] },
            { "client_secret", configuration["GitHub:ClientSecret"] },
            { "refresh_token", currentToken.RefreshToken },
            { "grant_type", "refresh_token" }
        };

        request.Content = new FormUrlEncodedContent(parameters);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonDocument.Parse(responseContent).RootElement;

        var newAccessToken = tokenResponse.GetProperty("access_token").GetString() ?? string.Empty;
        var newRefreshToken = tokenResponse.GetProperty("refresh_token").GetString() ?? string.Empty;

        currentToken.AccessToken = newAccessToken;
        currentToken.RefreshToken = newRefreshToken;

        await dbContext.SaveChangesAsync();

        return newAccessToken;
    }
}
