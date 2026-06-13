using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Domain.Entities
{
    [Table("Exceptions")]
    public class ExceptionLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public string? Parameters { get; set; }

    }

    public class ExceptionLogConfiguration : IEntityTypeConfiguration<ExceptionLog>
    {
        public void Configure(EntityTypeBuilder<ExceptionLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            //Tells  ef core to treat this as native JSON for faster querying (not my idea, yanked it from a stackoverflow post trying to figure out how to get parameters in my exception table)
            builder.Property(x => x.Parameters).HasColumnType("jsonb");
        }
    }
}
