using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class ApplicationDbContext: IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
            :base(options)
        {
        }
        public DbSet<Car> Cars {get; set;}
        public DbSet<Brand> Brands {get; set;}
        public DbSet<Combustion> Combustions {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().ToTable("asp_net_users");
            modelBuilder.Entity<IdentityRole>().ToTable("asp_net_roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("asp_net_user_role");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("asp_net_role_claims");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("asp_net_user_claims");
        }

    }
}