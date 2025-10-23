using Microsoft.EntityFrameworkCore;

namespace ParkhausAPI.Context
{
    public class ParkhausContext: DbContext
    {
        public ParkhausContext(DbContextOptions<ParkhausContext> options) : base(options)
        {
        }
        public DbSet<Models.Tickets> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Models.Tickets>(entity =>
            {
                entity.HasKey(e => e.TicketID);

                // Diese Zeile hinzufügen:
                entity.Property(e => e.TicketID).ValueGeneratedOnAdd();

                entity.Property(e => e.Einfahrtszeit).IsRequired();
                entity.Property(e => e.Bezahlt).IsRequired().HasDefaultValue(false);
            });
        }

    }

}
