using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class BankDetailEntityConfiguration : IEntityTypeConfiguration<BankDetailEntity>
{
    public void Configure(EntityTypeBuilder<BankDetailEntity> builder)
    {
        throw new NotImplementedException();
    }
}
