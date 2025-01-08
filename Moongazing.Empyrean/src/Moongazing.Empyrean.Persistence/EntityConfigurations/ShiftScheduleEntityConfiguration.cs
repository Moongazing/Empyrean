using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class ShiftScheduleEntityConfiguration : IEntityTypeConfiguration<ShiftScheduleEntity>
{
    public void Configure(EntityTypeBuilder<ShiftScheduleEntity> builder)
    {
        throw new NotImplementedException();
    }
}
