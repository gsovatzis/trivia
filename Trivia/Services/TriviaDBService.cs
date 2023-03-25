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

        /// <summary>
        /// Method that retrieves all available categories
        /// </summary>
        /// <returns>A collection of categories</returns>
        public async Task<ICollection<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        /// <summary>
        /// This method either retrieves a category by name, or creates a new one (if it doesn't exists)
        /// </summary>
        /// <param name="catName">The name of the category to be searched</param>
        /// <returns>The category</returns>
        public async Task<Category> GetCategoryByName(string catName)
        {
            Category? result = await _context.Categories.FirstOrDefaultAsync(x => x.Name.Equals(catName));
            if (result == null)
            {
                // Category, doesn't exists... create it
                Category category = new Category { Name = catName };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                result = await _context.Categories.FirstOrDefaultAsync(x => x.Name.Equals(catName));
            }

            return result;
        }

        /// <summary>
        /// Method that retrieves distinct difficulties
        /// </summary>
        /// <returns>A collection of difficulties</returns>
        public async Task<ICollection<string?>> GetDistinctDifficulties()
        {
            return await _context.Questions.Select(x => x.Difficulty).Distinct().ToListAsync();
        }

        /// <summary>
        /// This method saves the categories (and related tags) in the database
        /// </summary>
        /// <param name="categories">A collection of categories</param>
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

        /// <summary>
        /// This method saves the questions in the database
        /// </summary>
        /// <param name="questions">A collection of questions</param>
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

        /// <summary>
        /// This method performs a query on Questions
        /// </summary>
        /// <param name="QuestionTerm">The search term</param>
        /// <param name="SelectedCategory">Filter by question category</param>
        /// <param name="SelectedDifficulty">Filter by question difficulty</param>
        /// <returns>A Collection of Questions, based on the query</returns>
        public async Task<ICollection<Question>> SearchQuestions(string? QuestionTerm, int? SelectedCategory, string? SelectedDifficulty)
        {
            // Select all questions first
            var results = from q in _context.Questions select q;
            
            // Apply Question term filter
            if(!String.IsNullOrEmpty(QuestionTerm))
            {
                results = results.Where(s => s.Name!.ToLower().Contains(QuestionTerm.ToLower()));
            }

            // Apply Question difficulty filter
            if (!String.IsNullOrEmpty(SelectedDifficulty))
            {
                results = results.Where(s => s.Difficulty!.ToLower().Contains(SelectedDifficulty.ToLower()));
            }

            // Apply Question category filter
            if (SelectedCategory.HasValue)
            {
                results = results.Where(s => s.Category!.Id == SelectedCategory);
            }

            return await results.ToListAsync();
        }
    }
}
