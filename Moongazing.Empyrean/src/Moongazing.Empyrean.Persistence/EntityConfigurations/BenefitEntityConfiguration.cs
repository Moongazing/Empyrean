using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class BenefitEntityConfiguration : IEntityTypeConfiguration<BenefitEntity>
{
    public void Configure(EntityTypeBuilder<BenefitEntity> builder)
    {
        throw new NotImplementedException();
    }
}
