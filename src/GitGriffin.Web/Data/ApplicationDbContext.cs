using GitGriffin.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GitGriffin.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<UserOAuthToken> UserOAuthTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var oauthToken = modelBuilder.Entity<UserOAuthToken>();

        oauthToken.ToTable("user_oauth_tokens");

        oauthToken.HasOne(x => x.User)
            .WithMany(x => x.OAuthTokens)
            .HasForeignKey(x => x.UserId);

        oauthToken.HasKey(x => x.Id);

        base.OnModelCreating(modelBuilder);
    }
}
