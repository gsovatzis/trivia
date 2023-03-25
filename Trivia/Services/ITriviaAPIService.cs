using Trivia.Models;

namespace Trivia.Services
{
   public interface ITriviaAPIService
   {
      Task<ICollection<Question>> PopulateQuestions();
      Task<ICollection<Category>> PopulateCategories();
   }
}
