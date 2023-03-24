using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Trivia.Data;
using Trivia.Models;
using Trivia.Services;

namespace Trivia.Controllers
{
   public class HomeController : Controller
   {
      private readonly ILogger<HomeController> _logger;
      private readonly ITriviaService _triviaService;
      private readonly TriviaContext _context;

      public HomeController(ILogger<HomeController> logger, ITriviaService triviaService, TriviaContext context)
      {
         _logger = logger;
         _triviaService = triviaService;
         _context = context;
      }

      public IActionResult Index()
      {
         return View();
      }

      public async Task<IActionResult> Populate()
      {
         // Clean the database
         _context.CleanDB();

         // Populate a list of categories from the Trivia service
         var categories = _triviaService.PopulateCategories().Result;
         var questions = _triviaService.PopulateQuestions().Result;

         // If we have a list of categories retrieved, save them
         //await SaveCategories(categories);
                  
         return RedirectToAction("Index");
      }

      public IActionResult Privacy()
      {
         return View();
      }

      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }

      /// <summary>
      /// This method saves the categories (and related tags) in the database
      /// </summary>
      /// <param name="categories">A collection of categories</param>
      /// <returns></returns>
      private async Task SaveCategories(ICollection<Category> categories)
      {
         if (categories.Count() > 0)
         {
            foreach (var cat in categories)
            {
               foreach (var tag in cat.Tags!)
               {
                  _context.Add(tag);   // Persist the related tag first to avoid FK conflicts
               }
               _context.Add(cat);   // Persist the category
            }

            await _context.SaveChangesAsync();  // Do the inserts
         }
      }
   }
}