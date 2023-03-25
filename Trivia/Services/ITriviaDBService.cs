using Trivia.Models;

namespace Trivia.Services
{
    public interface ITriviaDBService
    {
        Task SaveCategories(ICollection<Category> categories);
        Task SaveQuestions(ICollection<Question> questions);
        Task<Category> GetCategoryByName(string catName);
        Task<ICollection<Category>> GetAllCategories();
        Task<ICollection<string?>> GetDistinctDifficulties();
        Task<ICollection<Question>> SearchQuestions(string? QuestionTerm, int? SelectedCategory, string? SelectedDifficulty);
        
        Task<Question> GetQuestionById(string questionId);
        void CleanDB();
    }
}
