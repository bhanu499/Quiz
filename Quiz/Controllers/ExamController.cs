using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quiz.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quiz.Controllers
{
    public class ExamController : Controller
    {

        public IActionResult Start()
        {
            return View("StartQuiz");
        }

        [HttpPost]
        public IActionResult Start(StartQuiz model)
        {
            if (ModelState.IsValid)
            {
            HttpContext.Session.SetString("Name", model.Name);
            HttpContext.Session.SetString("Topic", model.Topic);
            }

            var isCreated = QuizManager.Instance.IntializeRandomQuestions(10);
            if (isCreated)
            {
                var question = QuizManager.Instance.GetQuestion();

                return View("Quiz", question);
            }
            else
            {
                return View("Error", "The total questions available are less than the required questions for the quiz");
            }

        }


        [HttpPost]
        public IActionResult Quiz(string answer)
        {
            if (QuizManager.Instance.IsComplete) 
                return RedirectToAction("ShowResults");

            QuizManager.Instance.SaveAnswer(answer);
            if (QuizManager.Instance.MoveToNextQuestion())
            {
                var question = QuizManager.Instance.GetQuestion();
                return View(question);
            }

            QuizManager.Instance.IsComplete = true;
            var message = "You have successfully passed the test.";
            if(QuizManager.Instance.Score < 8)
            {
                message = "Unfortunately you did not pass the test. Please try again later.";
            }
            TempData["Result"] = message + " Your Score is" + QuizManager.Instance.Score;
            return RedirectToAction("Results");

        }

        public IActionResult Results()
        {
            return View("Results", new Results[] {
           new Results{ Name = "Banu", Score = 20, Date = DateTime.Now, Topic = "Maths" },
           new Results{ Name = "Thrinadh", Score = 20, Date = DateTime.Now, Topic = "Physics" },
           new Results{ Name = "Devi", Score = 20, Date = DateTime.Now, Topic = "Chemistry" }

            });
        }

    }
}
