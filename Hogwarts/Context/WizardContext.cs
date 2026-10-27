using Hogwarts.Model;
using Microsoft.EntityFrameworkCore;

namespace Hogwarts.Context
{
    public class WizardContext : DbContext
    {
        public DbSet<Wizard> Wizards { get; set; }

        // Az SQLite fájl helye a futtatási könyvtárhoz (bin/Debug/net10.0-windows) képest.
        private readonly string _defaultConnectionString = "Data Source=../../../DB/wizards.db";

        // Tesztekhez: kívülről adható meg a kapcsolat (pl. memóriában futó SQLite).
        public WizardContext(DbContextOptions<WizardContext> options) : base(options)
        {
        }

        public WizardContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_defaultConnectionString);
            }
        }
    }
}
