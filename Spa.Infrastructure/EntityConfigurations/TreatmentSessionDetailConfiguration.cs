using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Infrastructure.EntityConfigurations
{
    internal class TreatmentSessionDetailConfiguration : IEntityTypeConfiguration<TreatmendSessionDetail>
    {
        public void Configure(EntityTypeBuilder<TreatmendSessionDetail> builder)
        {
            builder.HasKey(e => e.TreatmendDetailID);
            builder.Property(e => e.TreatmendDetailID).ValueGeneratedOnAdd();

            builder.HasOne(e => e.TreatmentSession)
                .WithMany(e => e.TreatmendSessionDetail)
                .HasForeignKey(e => e.SessionID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Service)
                .WithMany(e => e.TreatmendSessionDetail)
                .HasForeignKey(e => e.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
