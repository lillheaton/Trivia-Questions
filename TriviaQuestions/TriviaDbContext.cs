using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

using TriviaQuestions.Models;

namespace TriviaQuestions
{
    public class TriviaDbContext : DbContext
    {
        public TriviaDbContext() : base("TriviaContext")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
