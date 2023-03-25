using Trivia.Models;

namespace Trivia.Services
{
    public interface ITriviaDBService
    {
        Task SaveCategories(ICollection<Category> categories);
        Task SaveQuestions(ICollection<Question> questions);
        Task<Category> GetCategoryByName(string catName);
        void CleanDB();
    }
}
