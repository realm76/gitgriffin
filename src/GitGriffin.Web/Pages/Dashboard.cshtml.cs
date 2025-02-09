using GitGriffin.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Octokit;
using GhPullRequest = Octokit.PullRequest;
using PullRequest = GitGriffin.Web.Entities.PullRequest;

namespace GitGriffin.Web.Pages;

public class Dashboard(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : PageModel
{
    public readonly List<PullRequest> PullRequests = [];

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User);

        if (user is null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        var token = await dbContext.UserOAuthTokens.FirstOrDefaultAsync(t => t.UserId == user.Id && t.Provider == "GitHub");

        if (token is null)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        var client = new GitHubClient(new ProductHeaderValue("GitGriffin"))
        {
            Credentials = new Credentials(token.AccessToken)
        };

        var repos = (await client.Repository.GetAllForCurrent()).ToList();

        foreach (var repo in repos)
        {
            var pullRequests = await client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);

            PullRequests.AddRange(pullRequests.Select(pr => new PullRequest(pr.Title, pr.User.Login, repo.Name, pr.HtmlUrl)));
        }

        return Page();
    }
}
