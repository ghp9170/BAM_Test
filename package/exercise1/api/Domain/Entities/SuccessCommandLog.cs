using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Domain.Entities
{
    [Table("SuccessCommandLogs")]
    public class SuccessCommandLog
    {
        public int Id { get; set; }
        public string? Parameters { get; set; }
        //todo: when/if we go production we need to track the user/ip on this
        public double CommandTime { get; set; }
    }

    public class SuccessLogConfiguration : IEntityTypeConfiguration<SuccessCommandLog>
    {
        public void Configure(EntityTypeBuilder<SuccessCommandLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            //Tells  ef core to treat this as native JSON for faster querying (not my idea, yanked it from a stackoverflow post trying to figure out how to get parameters in my exception table)
            builder.Property(x => x.Parameters).HasColumnType("jsonb");
        }
    }
}
