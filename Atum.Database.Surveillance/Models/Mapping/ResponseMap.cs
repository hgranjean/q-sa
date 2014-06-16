using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atum.Domain.SurveyManagement;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class ResponseMap : EntityTypeConfiguration<ResponseEntry>
    {
        public ResponseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(128);
            
            // Table & Column Mappings
            this.ToTable("Responses");
            this.Property(t => t.Id).HasColumnName("Id");

            // Relationships
            this.HasOptional(t => t.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);
        }
    }
}
