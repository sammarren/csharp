using Microsoft.EntityFrameworkCore;

namespace BeltExam.Models.Contexts
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        
        public DbSet<User> Users { get; set; }

        public DbSet<DojoActivity> Activities {get;set;}
        public DbSet<Participant> Participants {get;set;}
    }
}