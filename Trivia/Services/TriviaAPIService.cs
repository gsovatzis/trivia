using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Trivia.Models;

namespace Trivia.Services
{
    /// <summary>
    /// This service class implements API functionality related to the trivia api
    /// </summary>
   public class TriviaAPIService : ITriviaAPIService
   {
      private readonly IHttpClientFactory _httpClientFactory;
      
      public TriviaAPIService(IHttpClientFactory httpClientFactory)
      {
         _httpClientFactory = httpClientFactory;
      }

      /// <summary>
      /// This method performs the http request for questions on the trivia API,
      /// and maps each JSON object with the correct Question model
      /// 
      /// We default to 50 questions fetch!
      /// </summary>
      /// <returns>A collection of Question objects</returns>
      public async Task<ICollection<Question>> PopulateQuestions()
      {
         var httpClient = _httpClientFactory.CreateClient("Trivia");
         var httpResponseMessage = await httpClient.GetAsync("api/questions?limit=50");

         ICollection<Question> results = new List<Question>();
         if (httpResponseMessage.IsSuccessStatusCode)
         {
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var questions = JsonNode.Parse(responseContent)!.AsArray();

            if (questions != null)
            {
                foreach (var question in questions.Root.AsArray())
                {
                    if (question != null)
                    {
                        if (question["id"] != null) // Check if ID coming from API is not null, because it is our primary key
                        {
                            Question myQuestion = new Question
                            {
                                Id = question["id"].ToString(),
                                Name = question["question"] != null ? question["question"].ToString() : "",
                                CorrectAnswer = question["correctAnswer"] != null ? question["correctAnswer"].ToString() : "",
                                IncorrectAnswers = question["incorrectAnswers"] != null ? String.Join(":", question["incorrectAnswers"].AsArray()) : "",
                                Difficulty = question["difficulty"] != null ? question["difficulty"].ToString() : "",
                                Tags = question["tags"] != null ? String.Join(":", question["tags"].AsArray()) : "",
                                Category = question["category"] != null ? new Category { Name = question["category"].ToString() } : null
                            };
                            results.Add(myQuestion);
                        }
                    }
                }
            }
         }

         return results;
      }

      /// <summary>
      /// This method performs the http request for categories on the trivia API,
      /// and maps each JSON object with the correct Category model
      /// </summary>
      /// <returns>A collection of Category objects</returns>
      public async Task<ICollection<Category>> PopulateCategories() {
         var httpClient = _httpClientFactory.CreateClient("Trivia");
         var httpResponseMessage = await httpClient.GetAsync("api/categories");

         ICollection<Category> results = new List<Category>();

         if (httpResponseMessage.IsSuccessStatusCode)
         {
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var categories = JsonNode.Parse(responseContent)!.AsObject();
            if(categories!=null) { 
                foreach(var cat in categories)
                {
                    Category myCat = new Category { Name = cat.Key, Tags = String.Join(":", cat.Value!.AsArray()) };
                    results.Add(myCat);
                }
             }
         }

         return results;
      }
   }
}
