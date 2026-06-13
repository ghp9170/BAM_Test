using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Domain.Entities
{
    [Table("AstronautDuty")]
    public class AstronautDuty
    {
        public int Id { get; set; }
        public int AstronautId { get; set; }

        public string Rank { get; set; } = string.Empty;

        public string DutyTitle { get; set; } = string.Empty;

        public DateTime DutyStartDate { get; set; }

        public DateTime? DutyEndDate { get; set; }

        public bool IsCurrent { get; set; }

        public virtual AstronautDetail Astronaut { get; set; }
    }

    public class AstronautDutyConfiguration : IEntityTypeConfiguration<AstronautDuty>
    {
        public void Configure(EntityTypeBuilder<AstronautDuty> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(z => z.Astronaut).WithMany(z => z.AstronautDuties).HasForeignKey(z => z.AstronautId);
        }
    }
}
