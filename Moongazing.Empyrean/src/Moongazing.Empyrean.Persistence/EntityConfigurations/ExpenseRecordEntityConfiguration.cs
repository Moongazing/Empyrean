using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class ExpenseRecordEntityConfiguration : IEntityTypeConfiguration<ExpenseRecordEntity>
{
    public void Configure(EntityTypeBuilder<ExpenseRecordEntity> builder)
    {
        throw new NotImplementedException();
    }
}
