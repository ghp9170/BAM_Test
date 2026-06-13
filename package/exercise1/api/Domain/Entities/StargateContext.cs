using Microsoft.EntityFrameworkCore;
using System.Data;

namespace StargateAPI.Domain.Entities
{
    public class StargateContext : DbContext
    {
        public IDbConnection Connection => Database.GetDbConnection();
        //made virtual so we can MOQ out without creating a repository and interface, keeps the handlers lean
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<AstronautDetail> AstronautDetails { get; set; }
        public virtual DbSet<AstronautDuty> AstronautDuties { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public virtual DbSet<SuccessCommandLog> SuccessCommandLog { get; set; }
        public StargateContext(DbContextOptions<StargateContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StargateContext).Assembly);

            //SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            var johnDoe = new Person
            {
                Id = 1,
                Name = "John Doe"
            };
            var johnDoeAstronaut = new AstronautDetail
            {
                Id = 1,
                PersonId = 1,
                CurrentRank = "1LT",
                CurrentDutyTitle = "Commander",
                CareerStartDate = DateTime.Now
            };
            //add seed data
            modelBuilder.Entity<Person>()
                .HasData(
                    johnDoe,
                    new Person
                    {
                        Id = 2,
                        Name = "Jane Doe"
                    }
                );

            modelBuilder.Entity<AstronautDetail>()
                .HasData(johnDoeAstronaut);

            modelBuilder.Entity<AstronautDuty>()
                .HasData(
                    new AstronautDuty
                    {
                        Id = 1,
                        AstronautId = johnDoeAstronaut.Id,
                        DutyStartDate = DateTime.Now,
                        DutyTitle = "Commander",
                        Rank = "1LT",
                        IsCurrent = true
                    }
                );
        }
    }
}
