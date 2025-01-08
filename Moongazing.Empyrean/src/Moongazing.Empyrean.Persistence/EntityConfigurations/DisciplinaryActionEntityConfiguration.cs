using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class DisciplinaryActionEntityConfiguration : IEntityTypeConfiguration<DisciplinaryActionEntity>
{
    public void Configure(EntityTypeBuilder<DisciplinaryActionEntity> builder)
    {
        throw new NotImplementedException();
    }
}
