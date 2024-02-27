using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JournalAPI.Database
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<MainGroup> MainGroups { get; set; }
        public DbSet<GroupWithVersion> GroupsWithVersion { get; set; }
        public DbSet<Subgroup> Subgroups { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<LogMessage> LogMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Mark>()
                .HasOne(m => m.Relation)
                .WithMany(s => s.Marks)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }

        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
    }
}
