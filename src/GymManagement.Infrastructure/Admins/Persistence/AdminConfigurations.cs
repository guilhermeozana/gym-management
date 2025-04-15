using GymManagement.Domain.Admins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Admins.Persistence;

public class AdminConfigurations : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasData(new Admin(
            userId: Guid.Parse("1f3b4e24-8c9d-4b26-8a64-2a4469b6ddee"),
            id: Guid.Parse("2150e333-8fdc-42a3-9474-1a3956d46de8")));
    }
}
