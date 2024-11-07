using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id= "1506ec9c-09c8-4c16-a22d-8ee1320ee8c8", Name="Employee", NormalizedName="EMPLOYEE"},
                new IdentityRole { Id = "d9467a8b-7c26-4caa-a7dd-2dd5f30885b1", Name = "Supervisor", NormalizedName = "SUPERVISOR" },
                new IdentityRole { Id = "2a918535-cabe-451f-b212-8c11c7c78dee", Name = "Administrator", NormalizedName = "ADMINISTRATOR" }
                );
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id= "2a918535-cabe-451f-b212-8c11c7c78dee",
                    Email="admin@localhost.com",
                    NormalizedEmail="ADMIN@LOCALHOST.COM",
                    NormalizedUserName="ADMIN@LOCALHOST.COM",
                    UserName="admin@localhost.com",
                    PasswordHash=hasher.HashPassword(null,"P@ssword1"),
                    EmailConfirmed=true,
                    FirstName="Default",
                    LastName="Admin",
                    DateOfBirth=new DateOnly(1980,10,10)
                });

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId= "2a918535-cabe-451f-b212-8c11c7c78dee",
                    UserId= "2a918535-cabe-451f-b212-8c11c7c78dee"
                });
        }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocation { get; set; }
        public DbSet<Period> Periods { get; set; }
    }
}
