using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class TerminationEntityConfiguration : IEntityTypeConfiguration<TerminationEntity>
{
    public void Configure(EntityTypeBuilder<TerminationEntity> builder)
    {
        throw new NotImplementedException();
    }
}