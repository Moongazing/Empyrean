using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class LeaveRequestEntityConfiguration : IEntityTypeConfiguration<LeaveRequestEntity>
{
    public void Configure(EntityTypeBuilder<LeaveRequestEntity> builder)
    {
        throw new NotImplementedException();
    }
}
