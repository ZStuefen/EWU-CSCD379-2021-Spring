using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext = SecretSanta.Data.DbContext;

namespace SecretSanta.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext()
            : base(new DbContextOptionsBuilder<DbContext>().UseSqlite("Data Source=main.db").Options)
        { }

        public DbSet<Group> Groups => Set<Group>();
        public DbSet<User> Users => Set<User>();
        public DbSet<GroupUser> GroupUsers => Set<GroupUser>();
        public DbSet<GroupAssignment> GroupAssignments => Set<GroupAssignment>();
        public DbSet<Assignment> Assignments => Set<Assignment>();
        public DbSet<Gift> Gifts => Set<Gift>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            //modelBuilder.Entity<User>()
            //    .HasAlternateKey(user => new { user.FirstName, user.LastName });
            modelBuilder.Entity<Group>().HasIndex(item => new { item.Name }).IsUnique();
            modelBuilder.Entity<GroupUser>().HasKey(item => new { item.GroupId, item.UserId });
            modelBuilder.Entity<GroupAssignment>().HasKey(item => new { item.GroupId, item.AssignmentId });
            modelBuilder.Entity<Assignment>().HasAlternateKey(item => new { item.GiverId, item.ReceiverId });
        }
    }
}