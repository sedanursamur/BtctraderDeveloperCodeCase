using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace RecurringBitcoinPurchaseInstructions.Data
{
    public class InstructionDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public InstructionDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("InstructionServiceDatabase")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User(1, "Sedanur SAMUR"),
                new User(2, "Alperen SAMUR"),
                new User(3, "Şenay TUNCEL"),
                new User(4, "Özlem EKİNCİ")
                );
        }
    }
}