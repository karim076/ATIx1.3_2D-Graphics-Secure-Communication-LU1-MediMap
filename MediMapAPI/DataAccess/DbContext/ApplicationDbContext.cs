using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, 
    ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Add DbSet properties here

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("MediMap");
        builder.Entity<ApplicationUser>(entity => entity.ToTable("User").HasKey(pk => pk.Id));
        builder.Entity<ApplicationRole>(entity => entity.ToTable("Roles").HasKey(pk => pk.Id));
        builder.Entity<ApplicationUserRole>(entity => entity.ToTable("UserRoles").HasKey(e => new { e.UserId, e.RoleId }));
        builder.Entity<ApplicationUserClaim>(entity => entity.ToTable("UserClaims").HasKey(pk => pk.Id));
        builder.Entity<ApplicationUserLogin>(entity => entity.ToTable("UserLogins").HasKey(pk => new { pk.LoginProvider, pk.ProviderKey }));
        builder.Entity<ApplicationRoleClaim>(entity => entity.ToTable("RoleClaims").HasKey(pk => pk.Id));
        builder.Entity<ApplicationUserToken>(entity => entity.ToTable("UserTokens").HasKey(e => new { e.UserId, e.LoginProvider, e.Name }));

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Ignore(u => u.EmailConfirmed);
            entity.Ignore(u => u.PhoneNumberConfirmed);
            entity.Ignore(u => u.TwoFactorEnabled);
            entity.Ignore(u => u.LockoutEnd);
            entity.Ignore(u => u.LockoutEnabled);
            entity.Ignore(u => u.AccessFailedCount);
            entity.Ignore(u => u.NormalizedEmail);
        });

        builder.Entity<ApplicationUser>(b =>
        {
            b.Property(u => u.Name).HasMaxLength(128);
            b.Property(u => u.NormalizedUserName).HasMaxLength(128);
            b.Property(u => u.Email).HasMaxLength(128);
            b.Property(u => u.NormalizedEmail).HasMaxLength(128);
        });

        builder.Entity<ApplicationUserToken>(b =>
        {
            b.Property(u => u.LoginProvider).HasMaxLength(128);
            b.Property(u => u.Name).HasMaxLength(128);
        });

        builder.Entity<ApplicationUser>(b =>
        {
            b.HasMany(a => a.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);

            b.HasMany(a => a.Logins)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);

            b.HasMany(a => a.Tokens)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);

            b.HasMany(a => a.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired().OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ApplicationRole>(b =>
        {
            b.HasMany(a => a.UserRoles)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired().OnDelete(DeleteBehavior.Cascade);

            b.HasMany(a => a.RoleClaims)
            .WithOne(e => e.Role)
            .HasForeignKey(rc => rc.RoleId)
            .IsRequired().OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ApplicationUserRole>(b =>
        {
            b.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired().OnDelete(DeleteBehavior.Cascade);

            b.HasOne(ur => ur.User)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(e => e.UserId)
            .IsRequired().OnDelete(DeleteBehavior.Cascade);
        });
    }
}
