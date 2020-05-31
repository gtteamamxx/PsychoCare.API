using Microsoft.EntityFrameworkCore;
using PsychoCare.DataAccess.Entities;
using PsychoCare.Logic.DataAccess;
using System;
using System.Text;

namespace PsychoCare.DataAccess
{
    public class PsychoCareContext : DbContext
    {
        public PsychoCareContext()
        {
        }

        public PsychoCareContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<EmotionalState> EmotionalStates { get; set; }

        public virtual DbSet<EnvironmentGroup> EnvironmentGroups { get; set; }

        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// Configures DB Model to use SQL Server with max 60s command timeout and tracking all queries
        /// When configuration is already provided (eg. tests) skipping
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(PsychoCareContextInitializer.ConnectionString,
                                    opt => opt.CommandTimeout(60)
                                              .MigrationsHistoryTable("__MigrationHistory"))
                              .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* Emotional state model has one-many with user and one-many with environment group
             * This makes ciricular problems with delete on SQL. So we must mark some of relation as less priority with
             * delete no action */

            modelBuilder.Entity<User>()
                .HasMany(x => x.EmotionalStates)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}