using Trivia.Models;

namespace Trivia.Services
{
   public interface ITriviaService
   {
      Task<ICollection<Question>> PopulateQuestions();
      Task<ICollection<Category>> PopulateCategories();
   }
}
