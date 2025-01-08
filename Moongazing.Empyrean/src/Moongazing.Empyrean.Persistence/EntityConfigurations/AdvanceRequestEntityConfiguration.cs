using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class AdvanceRequestEntityConfiguration : IEntityTypeConfiguration<AdvanceRequestEntity>
{
    public void Configure(EntityTypeBuilder<AdvanceRequestEntity> builder)
    {
        throw new NotImplementedException();
    }
}
