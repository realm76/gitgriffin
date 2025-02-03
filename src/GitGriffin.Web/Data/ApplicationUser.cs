using GitGriffin.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace GitGriffin.Web.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public GithubConfig? GithubConfig { get; set; }
}
