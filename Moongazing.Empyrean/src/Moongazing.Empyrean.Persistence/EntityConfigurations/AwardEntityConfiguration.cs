using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class AwardEntityConfiguration : IEntityTypeConfiguration<AwardEntity>
{
    public void Configure(EntityTypeBuilder<AwardEntity> builder)
    {
        throw new NotImplementedException();
    }
}
