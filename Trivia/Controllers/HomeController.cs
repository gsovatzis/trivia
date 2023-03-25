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
      private readonly ITriviaAPIService _triviaService;
      private readonly ITriviaDBService _triviaDB;

      public HomeController(ILogger<HomeController> logger, ITriviaAPIService triviaService, ITriviaDBService triviaDB)
      {
         _logger = logger;
         _triviaService = triviaService;
         _triviaDB = triviaDB;
      }

      public IActionResult Index()
      {
         return View();
      }

      public async Task<IActionResult> Populate()
      {
         // Clean the database
         _triviaDB.CleanDB();

         // Populate a list of categories from the Trivia service
         var categories = _triviaService.PopulateCategories().Result;
         var questions = _triviaService.PopulateQuestions().Result;

         // If we have a list of categories retrieved, save them into the DB
         await _triviaDB.SaveCategories(categories);

         // If we have a list of questions retrieved, save them into the DB
         await _triviaDB.SaveQuestions(questions);
                  
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
      
   }
}