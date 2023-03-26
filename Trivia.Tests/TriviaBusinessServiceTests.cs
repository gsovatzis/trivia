using Trivia.Models;
using Trivia.Services;

namespace Trivia.Tests
{
    [TestClass]
    public class TriviaBusinessServiceTests
    {
        [TestMethod]
        public async Task TestAnswerQuestionCorrect()
        {
            // Arrange
                var x = new TriviaBusinessService();
                var q = new Question { 
                    Name = "Which actor played the role of Captain Jack Sparrow in Pirates of the Caribbean?",
                    Difficulty = "easy",
                    CorrectAnswer = "Johnny Depp",
                    IncorrectAnswers = "John Cusack:Harrison Ford:Sean Penn"
                };

            // Act
                var result = await x.IsCorrectAnswer("Johnny Depp", q);

            // Assert
                Assert.IsTrue(result);

        }

        [TestMethod]
        public async Task TestAnswerQuestionIgnoreCaseCorrect()
        {
            // Arrange
            var x = new TriviaBusinessService();
            var q = new Question
            {
                Name = "Which actor played the role of Captain Jack Sparrow in Pirates of the Caribbean?",
                Difficulty = "easy",
                CorrectAnswer = "JoHnNy DEPP",
                IncorrectAnswers = "John Cusack:Harrison Ford:Sean Penn"
            };

            // Act
            var result = await x.IsCorrectAnswer("Johnny Depp", q);

            // Assert
            Assert.IsTrue(result);

        }

        [TestMethod]
        public async Task TestAnswerQuestionNotCorrect()
        {
            // Arrange
            var x = new TriviaBusinessService();
            var q = new Question
            {
                Name = "Which actor played the role of Captain Jack Sparrow in Pirates of the Caribbean?",
                Difficulty = "easy",
                CorrectAnswer = "Harrison Ford",
                IncorrectAnswers = "John Cusack:Harrison Ford:Sean Penn"
            };

            // Act
            var result = await x.IsCorrectAnswer("Johnny Depp", q);

            // Assert
            Assert.IsFalse(result);

        }
    }
}