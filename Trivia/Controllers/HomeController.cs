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
      private readonly ITriviaAPIService _triviaAPI;
      private readonly ITriviaDBService _triviaDB;

      public HomeController(ILogger<HomeController> logger, ITriviaAPIService triviaAPI, ITriviaDBService triviaDB)
      {
         _logger = logger;
         _triviaAPI = triviaAPI;
         _triviaDB = triviaDB;
      }

      public async Task<IActionResult> Index()
      {
            HomeViewModel model = new HomeViewModel();
            // Load distinct categories from DB and bind them to the viewModel
            model.Categories = await _triviaDB.GetAllCategories();

            // Load distinct difficulties from DB and bind them to the viewModel
            model.Difficulties = await _triviaDB.GetDistinctDifficulties();

            // Initially, bring all questions
            model.Results = await _triviaDB.SearchQuestions(null,null,null);
            model.Message = $"{model.Results.Count} {(model.Results.Count > 1 ? "Questions" : "Question")} retrieved...";

            return View(model);
      }

        public async Task<IActionResult> Search(string? QuestionTerm, int? SelectedCategory, string? SelectedDifficulty)
        {
            HomeViewModel model = new HomeViewModel();
            // Load distinct categories from DB and bind them to the viewModel
            model.Categories = await _triviaDB.GetAllCategories();
            
            // Load distinct difficulties from DB and bind them to the viewModel
            model.Difficulties = await _triviaDB.GetDistinctDifficulties();

            model.QuestionTerm = QuestionTerm;
            model.SelectedCategory = SelectedCategory;
            model.SelectedDifficulty = SelectedDifficulty;

            // Search the database and retrieve the results, based on filters
            model.Results = await _triviaDB.SearchQuestions(QuestionTerm, SelectedCategory, SelectedDifficulty);
            model.Message = $"{model.Results.Count} {(model.Results.Count > 1 ? "Questions" : "Question")} retrieved...";

            return View("Index",model);
        }

        public async Task<IActionResult> ShowQuestion(string Id)
        {
            // Retrieve the question by Id as send to the model
            var q = await _triviaDB.GetQuestionById(Id);
            if(q==null)
            {
                // If no question retrieved, return to the index view
                return RedirectToAction("Index");
            } 
            else
            {
                // Else, we have a question, show the question page...
                QuestionViewModel model = new QuestionViewModel();
                model.question = q;
                return View("Question", model);
            }
            
        }

        public async Task<IActionResult> AnswerQuestion(string SelectedAnswer, string Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var q = await _triviaDB.GetQuestionById(Id);
                QuestionViewModel model = new QuestionViewModel();
                model.question = q;
                if (SelectedAnswer == null)
                {
                    model.Message = "You must select an answer";
                } else
                {
                    if(SelectedAnswer.ToLower().Equals(q.CorrectAnswer.ToLower()))
                    {
                        model.Message = "CORRECT!!! - Wow, You are a mastermind :) ";
                    } else
                    {
                        model.Message = "Wrong answer... please try again :( ";
                    }
                }

                return View("Question", model);
            }
            
        }

      public async Task<IActionResult> Populate()
      {
         // Clean the database
         _triviaDB.CleanDB();

         // Populate a list of categories from the Trivia API
         var categories = _triviaAPI.PopulateCategories().Result;
         var questions = _triviaAPI.PopulateQuestions().Result;

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