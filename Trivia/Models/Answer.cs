using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Trivia.Models
{
   public class Answer
   {
      [Key]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int Id { get; set; }
      public string? Name { get; set; }
      public bool IsCorrect { get; set; }

   }
}
