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
    internal class TreatmentSessionConfiguration : IEntityTypeConfiguration<TreatmentSession>
    {
        public void Configure(EntityTypeBuilder<TreatmentSession> builder)
        {
            builder.HasKey(e => e.SessionID);
            builder.Property(e => e.SessionID).ValueGeneratedOnAdd();

            builder.HasOne(e => e.TreatmentCard)
                .WithMany(e => e.TreatmentSessions)
                .HasForeignKey(e => e.TreatmentID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
