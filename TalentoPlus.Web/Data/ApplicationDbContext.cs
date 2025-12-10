using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TalentoPlus.Web.Entities;

namespace TalentoPlus.Web.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Rename Identity Tables
        builder.Entity<IdentityUser>(entity => entity.ToTable("users"));
        builder.Entity<IdentityRole>(entity => entity.ToTable("roles"));
        builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("user_roles"));
        builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("user_claims"));
        builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("user_logins"));
        builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("role_claims"));
        builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("user_tokens"));

        // Rename Custom Tables
        builder.Entity<Employee>(entity => entity.ToTable("employees"));
        builder.Entity<Department>(entity => entity.ToTable("departments"));

        // Additional configuration
        builder.Entity<Employee>()
            .HasIndex(e => e.Email)
            .IsUnique();
    }
}
