using Microsoft.EntityFrameworkCore;
using Trivia.Data;
using Trivia.Models;

namespace Trivia.Services
{
    public class TriviaDBService : ITriviaDBService
    {
        private readonly TriviaContext _context;

        public TriviaDBService(TriviaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method that deletes all data from all tables, used by populate button
        /// </summary>
        public void CleanDB()
        {
            _context.Database.ExecuteSqlRaw("DELETE FROM Questions");
            _context.Database.ExecuteSqlRaw("DELETE FROM Categories");
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Categories',RESEED,0)");

        }
        public async Task<Category> GetCategoryByName(string catName)
        {
            Category? result = _context.Categories.FirstOrDefault(x => x.Name.Equals(catName));
            if (result == null)
            {
                // Category, doesn't exists... create it
                Category category = new Category { Name = catName };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                result = _context.Categories.FirstOrDefault(x => x.Name.Equals(catName));
            }

            return result;
        }

        /// <summary>
        /// This method saves the categories (and related tags) in the database
        /// </summary>
        /// <param name="categories">A collection of categories</param>
        /// <returns></returns>
        public async Task SaveCategories(ICollection<Category> categories)
        {
            if (categories.Count() > 0)
            {
                foreach (var cat in categories)
                {
                    _context.Add(cat);   // Persist the category
                }

                await _context.SaveChangesAsync();  // Do the inserts
            }
        }

        public async Task SaveQuestions(ICollection<Question> questions)
        {
            if (questions.Count() > 0)
            {
                foreach (var q in questions)
                {
                    // We need to check if the category already exists, if yes update the question with the related category, if not create the category first
                    q.Category = await GetCategoryByName(q.Category.Name);
                    _context.Add(q);
                }

                await _context.SaveChangesAsync();  // Do the inserts
            }
        }

    }
}
