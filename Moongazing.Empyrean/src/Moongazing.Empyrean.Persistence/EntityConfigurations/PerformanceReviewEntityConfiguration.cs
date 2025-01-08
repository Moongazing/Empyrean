using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class PerformanceReviewEntityConfiguration : IEntityTypeConfiguration<PerformanceReviewEntity>
{
    public void Configure(EntityTypeBuilder<PerformanceReviewEntity> builder)
    {
        throw new NotImplementedException();
    }
}
