using Microsoft.AspNetCore.Mvc.Rendering;

namespace Trivia.Models
{
    public class HomeViewModel
    {
        public ICollection<Category> Categories { get; set; }
        public ICollection<string?> Difficulties { get; set; }
        public ICollection<Question> Results { get; set; }

        public string? QuestionTerm { get; set; }
        public string? SelectedDifficulty { get; set; }
        public int? SelectedCategory { get; set; }

        public string Message { get; set; }

        public HomeViewModel() { 
            Categories = new List<Category>();
            Difficulties = new List<string?>();
            Results = new List<Question>();
        }

        public SelectList GetCategoriesSelectList()
        {
            SelectList results = new SelectList(Categories,"Id","Name",SelectedCategory);
            return results;
        }

        public SelectList GetDifficultiesSelectList()
        {
            SelectList results = new SelectList(Difficulties, SelectedDifficulty);
            return results;
        }
    }
}
