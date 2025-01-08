using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class OvertimeEntityConfiguration : IEntityTypeConfiguration<OvertimeEntity>
{
    public void Configure(EntityTypeBuilder<OvertimeEntity> builder)
    {
        throw new NotImplementedException();
    }
}
