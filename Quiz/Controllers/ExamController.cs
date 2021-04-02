using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

            var quizManager = new QuizManager(true);
            var isCreated = quizManager.IntializeRandomQuestions(10, model.Topic);
            if (isCreated)
            {
                var question = quizManager.GetQuestion();
                UpdateSession(quizManager);
                return View("Quiz", question);
            }
            else
            {
                return View("Error", new ErrorViewModel()
                {
                    ErrorMessage = "The total questions available in the specified topic are less than the required questions for the quiz"
                });
            }

        }

        [HttpPost]
        public IActionResult Quiz(string answer, string Previous, string Next, string Submit)
        {
            var quizManager = HttpContext.Session.GetObjectFromJson<QuizManager>("quizManager");
            var message = "You have successfully passed the test.";
            if (Next != null && !quizManager.IsComplete)
            {
                quizManager.SaveAnswer(answer);
                if (quizManager.MoveToNextQuestion())
                {
                    var question = quizManager.GetQuestion();
                    UpdateSession(quizManager);
                    return View(question);
                }

                quizManager.IsComplete = true;
            }
            else if (Previous != null)
            {
                quizManager.PreviosQuestion();
                var question = quizManager.GetQuestion();
                UpdateSession(quizManager);
                return View(question);
            }
            else if (Submit!= null)
            {
                quizManager.IsComplete = true;
            }
            if (quizManager.Score < 8)
            {
                message = "Unfortunately you did not pass the test. Please try again later.";
            }
            TempData["Result"] = message + " Your Score is" + quizManager.Score;
            UpdateSession(quizManager);
            UpdateResultsSession(quizManager);
            return RedirectToAction("Results");
        }

        public IActionResult Results()
        {
            List<Results> quizResultsData = HttpContext.Session.GetObjectFromJson<List<Results>>("quizResults");
            if(quizResultsData == null)
            {
                quizResultsData = new List<Results>();
            }
            return View("Results", quizResultsData.ToArray());
        }

        private void UpdateResultsSession(QuizManager quizManager)
        {
            List<Results> quizResultsData = HttpContext.Session.GetObjectFromJson<List<Results>>("quizResults");
            var resultToAdd = new Results()
            {
                Name = HttpContext.Session.GetString("Name"),
                Date = DateTime.Now,
                Score = quizManager.Score,
                Topic = HttpContext.Session.GetString("Topic")
            };
            if (quizResultsData == null)
            {
                quizResultsData = new List<Results> { resultToAdd };
            } else
            {
                quizResultsData.Add(resultToAdd);
            }

            HttpContext.Session.SetObjectAsJson("quizResults", quizResultsData);
        }

        private void UpdateSession(QuizManager quizManager)
        {
            HttpContext.Session.SetObjectAsJson("quizManager", quizManager);
            TempData["questionIndex"] = quizManager.activeQuestionIndex + 1;
        }

    }


    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
