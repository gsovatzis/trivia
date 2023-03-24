using Microsoft.EntityFrameworkCore;
using Trivia.Models;

namespace Trivia.Data
{
   public class TriviaContext : DbContext
   {
      public TriviaContext(DbContextOptions<TriviaContext> options) : base(options) 
      { 
      }

      public DbSet<Tag> Tags { get; set; }
      public DbSet<Category> Categories { get; set; }
      public DbSet<Question> Questions { get; set; }
      public DbSet<Answer> Answers { get; set; }

      /// <summary>
      /// Method that deletes all data from all tables, used by populate button
      /// </summary>
      public void CleanDB()
      {
         this.Database.ExecuteSqlRaw("DELETE FROM Tags");
         this.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Tags',RESEED,0)");
         this.Database.ExecuteSqlRaw("DELETE FROM Answers");
         this.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Answers',RESEED,0)");
         this.Database.ExecuteSqlRaw("DELETE FROM Questions");
         
         this.Database.ExecuteSqlRaw("DELETE FROM Categories");
         this.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Categories',RESEED,0)");


      }

   }
}
