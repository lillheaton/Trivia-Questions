using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TriviaQuestion2.Models;

namespace TriviaQuestion2
{
    public class Trivia2Context : DbContext
    {
        public Trivia2Context() : base("Trivia2Context")
        {
        }

        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
