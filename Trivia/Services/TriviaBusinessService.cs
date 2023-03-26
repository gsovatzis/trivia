using Trivia.Models;

namespace Trivia.Services
{
    public class TriviaBusinessService : ITriviaBusinessService
    {
        /// <summary>
        /// Method that gets a question and an answer and returns where the answer is correct or not
        /// </summary>
        /// <param name="answer">The answer string</param>
        /// <param name="question">The question model object</param>
        /// <returns>True or false</returns>
        public async Task<bool> IsCorrectAnswer(string answer, Question question)
        {
            if (answer.ToLower().Equals(question.CorrectAnswer.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
