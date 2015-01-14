using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atum.Domain;
using System.Data.Entity.ModelConfiguration;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class EventUserMap //: EntityTypeConfiguration<EventUser>
    {
        //public EventUserMap()
        //{
        //    // Primary key
        //    this.HasKey(t => new { t.EventId, t.UserId });

        //    // Properties
        //    this.k(t => t.EventId)
        //        .IsRequired()
        //        .HasMaxLength(128);

        //    this.k(t => t.UserId)
        //        .IsRequired()
        //        .HasMaxLength(128);
            

        //    // Table & Column Mappings
        //    this.ToTable("EventUsers");
        //    this.k(t => t.EventId).HasColumnName("EventId");
        //    this.k(t => t.UserId).HasColumnName("UserId");

        //    // Relationships
        //    this.HasRequired(t => t.Event)
        //        .WithMany()
        //        .HasForeignKey(d => d.EventId);

        //    this.HasRequired(t => t.User)
        //        .WithMany()
        //        .HasForeignKey(d => d.UserId);
        //}
    }
}
