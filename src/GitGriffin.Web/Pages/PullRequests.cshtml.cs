using GitGriffin.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;

namespace GitGriffin.Web.Pages;

public class PullRequests(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : PageModel
{
    public readonly List<PullRequest> GithubPullRequests = [];

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User);

        if (user is null)
        {
            return RedirectToPage("/Index");
        }

        var config = dbContext.GithubConfig.FirstOrDefault(c => c.UserId == user.Id);

        if (config is null)
        {
            return Page();
        }

        var client = new GitHubClient(new ProductHeaderValue("GitGriffin"))
        {
            Credentials = new Credentials(config.AccessToken)
        };

        foreach (var repo in config.Repositories)
        {
            var spl = repo.Split("/");
            if (spl.Length < 2)
            {
                continue;
            }

            var owner = spl[0];
            var repoName = spl[1];
            var pullRequests = await client.PullRequest.GetAllForRepository(owner, repoName);

            GithubPullRequests.AddRange(pullRequests);
        }

        return Page();
    }
}
