using Microsoft.EntityFrameworkCore;
using Smart_Event_Management_and_Ticketing_System.Models;

namespace Smart_Event_Management_and_Ticketing_System.Data
{
    /// <summary>
    /// Database context for the Smart Event Management System
    /// Manages all database operations using Entity Framework Core
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets represent tables in the database
        public DbSet<Member> Members { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }

    /// <summary>
    /// Configure entity relationships and constraints
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set default schema for all tables
        modelBuilder.HasDefaultSchema("EVENT_MGMT");

        // Configure Member entity
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique(); // Ensure unique emails
        });

            // Configure Event entity
            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Price).HasPrecision(18, 2);
            });

            // Configure Booking relationships
            modelBuilder.Entity<Booking>(entity =>
            {
                // One Member can have many Bookings
                entity.HasOne(b => b.Member)
                    .WithMany(m => m.Bookings)
                    .HasForeignKey(b => b.MemberId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                // One Event can have many Bookings
                entity.HasOne(b => b.Event)
                    .WithMany(e => e.Bookings)
                    .HasForeignKey(b => b.EventId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // Configure Review relationships
            modelBuilder.Entity<Review>(entity =>
            {
                // One Member can have many Reviews
                entity.HasOne(r => r.Member)
                    .WithMany(m => m.Reviews)
                    .HasForeignKey(r => r.MemberId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

                // One Event can have many Reviews
                entity.HasOne(r => r.Event)
                    .WithMany(e => e.Reviews)
                    .HasForeignKey(r => r.EventId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        /// <summary>
        /// Seeds sample data for testing and demonstration
        /// </summary>
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Events
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    EventId = 1,
                    EventName = "Metropolitan Orchestra: Symphony Night",
                    Category = "Music",
                    EventDate = new DateTime(2025, 6, 15, 19, 30, 0),
                    Venue = "Grand Concert Hall",
                    Price = 45.00m,
                    TotalSeats = 500,
                    Description = "An enchanting evening of classical music featuring renowned orchestra performers."
                },
                new Event
                {
                    EventId = 2,
                    EventName = "Contemporary Art Exhibition",
                    Category = "Art",
                    EventDate = new DateTime(2025, 5, 20, 10, 0, 0),
                    Venue = "City Art Gallery",
                    Price = 15.00m,
                    TotalSeats = 200,
                    Description = "Explore modern art from local and international artists."
                },
                new Event
                {
                    EventId = 3,
                    EventName = "Shakespeare's Hamlet",
                    Category = "Theater",
                    EventDate = new DateTime(2025, 7, 10, 20, 0, 0),
                    Venue = "Metropolitan Theater",
                    Price = 35.00m,
                    TotalSeats = 350,
                    Description = "A dramatic performance of the classic tragedy by William Shakespeare."
                },
                new Event
                {
                    EventId = 4,
                    EventName = "Jazz Night Live",
                    Category = "Music",
                    EventDate = new DateTime(2025, 5, 30, 21, 0, 0),
                    Venue = "Blue Note Jazz Club",
                    Price = 25.00m,
                    TotalSeats = 150,
                    Description = "Smooth jazz performances by award-winning musicians."
                },
                new Event
                {
                    EventId = 5,
                    EventName = "Cultural Dance Festival",
                    Category = "Dance",
                    EventDate = new DateTime(2025, 8, 5, 18, 0, 0),
                    Venue = "City Cultural Center",
                    Price = 30.00m,
                    TotalSeats = 400,
                    Description = "A celebration of diverse cultural dance traditions from around the world."
                },
                new Event
                {
                    EventId = 6,
                    EventName = "Photography Workshop",
                    Category = "Workshop",
                    EventDate = new DateTime(2025, 6, 1, 14, 0, 0),
                    Venue = "Community Arts Space",
                    Price = 50.00m,
                    TotalSeats = 30,
                    Description = "Learn advanced photography techniques from professional photographers."
                }
            );
        }
    }
}
