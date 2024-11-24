using BCMT_Associates.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BCMT_Associates.Context
{
    public class AppDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, int,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                // Each User can have many UserClaims
                b.HasIndex(e => e.RoleId);

                // Each User can have many UserLogins
                b.Property(e => e.Id)
                    .IsRequired();

                // Each User can have many UserTokens
                b.Property(e => e.Username).IsRequired();

                // Each User can have many entries in the UserRole join table
                b.Property(e => e.Password).IsRequired();
            });

            modelBuilder.Entity<RoleMenu>(entity =>
            {
                entity.HasIndex(e => e.MenusId);

                entity.HasIndex(e => e.RolesId);

                entity.Property(e => e.Id).IsRequired();

                entity.Property(e => e.MenusId).IsRequired();

                entity.Property(e => e.RolesId).IsRequired();

                entity.HasOne(d => d.Menus)
                    .WithMany(p => p.RoleMenu);

                entity.HasOne(d => d.Roles)
                    .WithMany(p => p.RoleMenu)
                    .HasForeignKey(d => d.RolesId);
            });

            modelBuilder.Entity<Menus>(entity =>
            {
                entity.ToTable("menus");

                entity.Property(e => e.Id);

                entity.Property(e => e.Icon);

                entity.Property(e => e.Name)
                    .IsRequired();

                entity.Property(e => e.ParentId)
                    .HasColumnName("ParentId");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description);

                entity.Property(e => e.Title);
            });
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ApplicationUser>(b =>
        //    {
        //        // Each User can have many UserClaims
        //        b.HasMany(e => e.Claims)
        //            .WithOne(e => e.User)
        //            .HasForeignKey(uc => uc.UserId)
        //            .IsRequired();

        //        // Each User can have many UserLogins
        //        b.HasMany(e => e.Logins)
        //            .WithOne(e => e.User)
        //            .HasForeignKey(ul => ul.UserId)
        //            .IsRequired();

        //        // Each User can have many UserTokens
        //        b.HasMany(e => e.Tokens)
        //            .WithOne(e => e.User)
        //            .HasForeignKey(ut => ut.UserId)
        //            .IsRequired();

        //        // Each User can have many entries in the UserRole join table
        //        b.HasMany(e => e.UserRoles)
        //            .WithOne(e => e.User)
        //            .HasForeignKey(ur => ur.UserId)
        //            .IsRequired();
        //    });

        //    modelBuilder.Entity<ApplicationRole>(b =>
        //    {
        //        // Each Role can have many entries in the UserRole join table
        //        b.HasMany(e => e.UserRoles)
        //            .WithOne(e => e.Role)
        //            .HasForeignKey(ur => ur.RoleId)
        //            .IsRequired();

        //        // Each Role can have many associated RoleClaims
        //        b.HasMany(e => e.RoleClaims)
        //            .WithOne(e => e.Role)
        //            .HasForeignKey(rc => rc.RoleId)
        //            .IsRequired();
        //    });
        //}
        
        public DbSet<User> Users { get; set; }

        public DbSet<ResetPassword> ResetPassword { get; set; }
        public DbSet<Roles> Roles { get; set; }

        public virtual DbSet<RoleMenu> RoleMenu { get; set; }
        public virtual DbSet<RoleMenus> RoleMenus { get; set; }
        public virtual DbSet<Menus> Menus { get; set; }


        public DbSet<Course> CoursesA { get; set; }
        public DbSet<Publication> Publications { get; set; }






    }
}
