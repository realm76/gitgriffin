using GitGriffin.Web.Data;

namespace GitGriffin.Web.Models;

public class GithubConfig
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public string? AccessToken { get; set; }
    public string? UserName { get; set; }

    public List<string> Repositories { get; set; } = [];

    public ApplicationUser? User { get; set; }
}

public class Repository
{
    public string Url { get; set; } = string.Empty;
}
