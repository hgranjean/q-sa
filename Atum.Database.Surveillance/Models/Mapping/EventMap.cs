using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Database.Surveillance.Models.Mapping
{   
    public partial class EventMap : EntityTypeConfiguration<Event>
    {
        public EventMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Start)
                .IsRequired();
            
            this.Property(t => t.End);
            
            this.Property(t => t.Url);
            
            // Table & Column Mappings
            this.ToTable("Events");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Start).HasColumnName("Start");
            this.Property(t => t.End).HasColumnName("End");
            this.Property(t => t.Url).HasColumnName("Url");
        }
    }
    
}
