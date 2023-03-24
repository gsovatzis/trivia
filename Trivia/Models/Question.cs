namespace Trivia.Models
{
   public class Question
   {
      public string Id { get; set; }
      public string? Name { get; set; }
      public string? Difficulty { get; set; }
      public Category? Category { get; set; }
      public ICollection<Answer>? Answers { get; set; }
      public ICollection<Tag>? Tags { get; set; }
           
      public Question()
      {
         Answers = new List<Answer>();
         Tags = new List<Tag>();
      }
   }
}
