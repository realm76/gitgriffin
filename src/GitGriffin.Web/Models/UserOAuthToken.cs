using GitGriffin.Web.Data;

namespace GitGriffin.Web.Models;

public class UserOAuthToken
{
    public long Id { get; init; }
    public required string UserId { get; init; }
    public required string Provider { get; init; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset? Expires { get; init; }

    public ApplicationUser? User { get; init; }
}
