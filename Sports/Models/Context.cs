using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            var footballId = Guid.NewGuid();
            var basketballId = Guid.NewGuid();
            modelBuilder.Entity<Sport>().HasData(new Sport { Id = footballId, Name = "Футбол" });
            modelBuilder.Entity<EventType>().HasData(new EventType { Id = Guid.NewGuid(), SportId = footballId, Name = "Гол", IsDualPlayer = false });
            modelBuilder.Entity<EventType>().HasData(new EventType { Id = Guid.NewGuid(), SportId = footballId, Name = "Пас", IsDualPlayer = true });
            modelBuilder.Entity<EventType>().HasData(new EventType { Id = Guid.NewGuid(), SportId = footballId, Name = "Пенальти", IsDualPlayer = false });
            modelBuilder.Entity<EventType>().HasData(new EventType { Id = Guid.NewGuid(), SportId = footballId, Name = "Карточка", IsDualPlayer = false });
            modelBuilder.Entity<Sport>().HasData(new Sport { Id = basketballId, Name = "Баскетбол" });
            modelBuilder.Entity<EventType>().HasData(new EventType { Id = Guid.NewGuid(), SportId = basketballId, Name = "Гол", IsDualPlayer = false });
            modelBuilder.Entity<EventType>().HasData(new EventType { Id = Guid.NewGuid(), SportId = basketballId, Name = "Пас", IsDualPlayer = true });
            modelBuilder.Entity<EventType>().HasData(new EventType { Id = Guid.NewGuid(), SportId = basketballId, Name = "Штрафной", IsDualPlayer = false });
            modelBuilder.Entity<EventType>().HasData(new EventType { Id = Guid.NewGuid(), SportId = basketballId, Name = "Нарушение", IsDualPlayer = false });
            
        }

        public DbSet<Sport> Sports { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<EventType> EventTypes { get; set; }

        public DbSet<EventSinglePlayer> EventsSinglePlayer { get; set; }

        public DbSet<EventDualPlayer> EventsDualPlayer { get; set; }

        public DbSet<Player> Players { get; set; }
    }
}
