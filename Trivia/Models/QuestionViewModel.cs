namespace Trivia.Models
{
    public class QuestionViewModel
    {
        public Question question { get; set; }

        public string Message { get; set; }

        public string? SelectedAnswer { get; set; }

        /// <summary>
        /// We want to retrieve all the possible answers, including the correct one in a random position :)
        /// </summary>
        /// <returns>The list of possible answers</returns>
        public List<string> GetPossibleAnswers()
        {
            List<string> results = question!.IncorrectAnswers!.Split(":").ToList();

            Random rnd = new Random();
            int pos = rnd.Next(results.Count);
            results.Insert(pos, question!.CorrectAnswer!);

            return results;
        }
    }
}
