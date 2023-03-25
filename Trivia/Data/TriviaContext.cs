using Microsoft.EntityFrameworkCore;
using Trivia.Models;

namespace Trivia.Data
{
   public class TriviaContext : DbContext
   {
      public TriviaContext(DbContextOptions<TriviaContext> options) : base(options) 
      { 
      }

      public DbSet<Category> Categories { get; set; }
      public DbSet<Question> Questions { get; set; }

   }
}
