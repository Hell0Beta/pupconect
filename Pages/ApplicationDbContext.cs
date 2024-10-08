using Microsoft.EntityFrameworkCore;
using pupconect.Data;

namespace pupconect.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetReport> PetReports { get; set;}
    }

    public class Pet
    {
        public int Id { get; set;}
        public string PetName { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
    }
    public class PetReport
    {
        public int Id { get; set;}
        public string ReportType { get; set; }
        public string Breed { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public string ContactInfo { get; set; }
        public string AdditionalInfo { get; set; }
    }
}