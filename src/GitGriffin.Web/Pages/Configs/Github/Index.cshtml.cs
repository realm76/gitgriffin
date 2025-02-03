using GitGriffin.Web.Data;
using GitGriffin.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GitGriffin.Web.Pages.Configs.Github;

public class Index(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public GithubConfig Config
    {
        get;
        set;
    } = new()
    {
        UserId = string.Empty,
    };

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await userManager.GetUserAsync(HttpContext.User);

        if (user is null)
        {
            return RedirectToPage("/Index");
        }

        var config = dbContext.GithubConfig.FirstOrDefault(x => x.UserId == user.Id);

        if (config is null)
        {
            config = new GithubConfig
            {
                UserId = user.Id
            };

            dbContext.GithubConfig.Add(config);

            await dbContext.SaveChangesAsync();
        }

        Config = config;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await userManager.GetUserAsync(HttpContext.User);

        if (user is null)
        {
            return RedirectToPage("/Index");
        }

        var config = dbContext.GithubConfig.FirstOrDefault(x => x.UserId == user.Id);

        if (config is null)
        {
            config = new GithubConfig
            {
                UserId = user.Id,
            };

            dbContext.GithubConfig.Add(config);

            await dbContext.SaveChangesAsync();
        }

        config.AccessToken = Config.AccessToken;
        config.Repositories = Config.Repositories;
        config.UserName = Config.UserName;

        await dbContext.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveRepositoryAsync(int? index)
    {
        if (index is null)
        {
            return RedirectToPage();
        }

        var user = await userManager.GetUserAsync(HttpContext.User);

        if (user is null)
        {
            return RedirectToPage("/Index");
        }

        var config = dbContext.GithubConfig.FirstOrDefault(x => x.UserId == user.Id);
        if (config is null)
        {
            return RedirectToPage();
        }

        if (config.Repositories.Count > index)
        {
            config.Repositories.RemoveAt(index.Value);

            await dbContext.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAddRepositoryAsync()
    {
        var user = await userManager.GetUserAsync(HttpContext.User);

        if (user is null)
        {
            return RedirectToPage("/Index");
        }

        var config = dbContext.GithubConfig.FirstOrDefault(x => x.UserId == user.Id);
        if (config is null)
        {
            return RedirectToPage();
        }

        config.Repositories.Add(string.Empty);

        await dbContext.SaveChangesAsync();

        return RedirectToPage();
    }
}
