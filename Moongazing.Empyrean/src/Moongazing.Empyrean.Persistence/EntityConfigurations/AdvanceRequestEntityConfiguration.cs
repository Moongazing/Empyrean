using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class AdvanceRequestEntityConfiguration : IEntityTypeConfiguration<AdvanceRequestEntity>
{
    public void Configure(EntityTypeBuilder<AdvanceRequestEntity> builder)
    {
        throw new NotImplementedException();
    }
}
