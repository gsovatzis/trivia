using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Trivia.Models;

namespace Trivia.Services
{
   public class TriviaService : ITriviaService
   {
      private readonly IHttpClientFactory _httpClientFactory;

      public TriviaService(IHttpClientFactory httpClientFactory)
      {
         _httpClientFactory = httpClientFactory;
      }

      /// <summary>
      /// This method performs the http request for questions on the trivia API,
      /// and maps each JSON object with the correct Question model
      /// </summary>
      /// <returns>A collection of Question objects</returns>
      public async Task<ICollection<Question>> PopulateQuestions()
      {
         var httpClient = _httpClientFactory.CreateClient("Trivia");
         var httpResponseMessage = await httpClient.GetAsync("api/questions");

         ICollection<Question> results = new List<Question>();
         if (httpResponseMessage.IsSuccessStatusCode)
         {
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var questions = JsonNode.Parse(responseContent)!.AsArray();
            foreach(var x in questions.Root.AsArray())
            {
               Debug.WriteLine(x);
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
            //var dynamicObject = JsonSerializer.Deserialize<dynamic>(responseContent);

            foreach(var cat in categories)
            {
               Category myCat = new Category { Name = cat.Key };
               
               foreach(var tag in cat.Value!.AsArray())
               {
                  myCat.Tags!.Add(new Tag { Name = tag!.ToString() });
               }

               results.Add(myCat);
            }
         }

         return results;
      }
   }
}
