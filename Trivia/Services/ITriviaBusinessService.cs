using Trivia.Models;

namespace Trivia.Services
{
    public interface ITriviaBusinessService
    {
        public Task<bool> IsCorrectAnswer(string answer, Question question);
    }
}
