using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Data.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
              new IdentityRole { Id = "1506ec9c-09c8-4c16-a22d-8ee1320ee8c8", Name = "Employee", NormalizedName = "EMPLOYEE" },
              new IdentityRole { Id = "d9467a8b-7c26-4caa-a7dd-2dd5f30885b1", Name = "Supervisor", NormalizedName = "SUPERVISOR" },
              new IdentityRole { Id = "2a918535-cabe-451f-b212-8c11c7c78dee", Name = "Administrator", NormalizedName = "ADMINISTRATOR" }
              );
        }
    }
}
