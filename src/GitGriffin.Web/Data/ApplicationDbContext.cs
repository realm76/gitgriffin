using GitGriffin.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GitGriffin.Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<GithubConfig> GithubConfig { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ghConfigModel = modelBuilder.Entity<GithubConfig>();

        ghConfigModel
            .HasOne(x => x.User)
            .WithOne(x => x.GithubConfig)
            .HasForeignKey<GithubConfig>(x => x.UserId);

        ghConfigModel.HasKey(x => x.Id);

        ghConfigModel.Property(x => x.UserId)
            .IsRequired()
            .HasMaxLength(40);

        ghConfigModel.Property(x => x.AccessToken)
            .HasMaxLength(50);

        ghConfigModel.Property(x => x.UserName)
            .HasMaxLength(100);

        base.OnModelCreating(modelBuilder);
    }
}
