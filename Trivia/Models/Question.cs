namespace Trivia.Models
{
   public class Question
   {
      public string Id { get; set; }
      public string? Name { get; set; }
      public string? Difficulty { get; set; }
      public Category? Category { get; set; }
      public string? CorrectAnswer { get; set; }
      public string? IncorrectAnswers { get; set; }
      public string? Tags { get; set; }
      
   }
}
