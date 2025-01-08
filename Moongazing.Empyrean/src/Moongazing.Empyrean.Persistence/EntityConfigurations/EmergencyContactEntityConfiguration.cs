using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class EmergencyContactEntityConfiguration : IEntityTypeConfiguration<EmergencyContactEntity>
{
    public void Configure(EntityTypeBuilder<EmergencyContactEntity> builder)
    {
        throw new NotImplementedException();
    }
}
