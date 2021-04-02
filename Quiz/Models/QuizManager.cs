using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class QuizManager
    {
        List<Question> baseQuestions = new List<Question>();
        List<Question> questions = new List<Question>();
        int activeQuestionIndex = 0;
        public int Score = 0;
        static QuizManager instance;
        public bool IsComplete = false;

        private QuizManager()
        {
            for (int i = 0; i < 20; i++)
            {
                baseQuestions.Add(new Question()
                {
                    Topic = "Basics",
                    QuestionId = i,
                    QuestionDescription = "Shape of earth",
                    Option1 = "Square",
                    Option2 = "Triangle",
                    Option3 = "Round",
                    Option4 = "Trapezoid",
                    Answer = "Round"
                });
            }
        }

        public static QuizManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new QuizManager();
                return instance;

            }
        }

        public bool IntializeRandomQuestions(int count)
        {
            if (count > baseQuestions.Count)
            {
                return false;
            }
            else
            {
                Random r = new Random();
                int number;
                for (int i = 0; i < count; i++)
                {
                    do
                    {
                        number = r.Next(1, baseQuestions.Count);
                    } while (questions.Contains(baseQuestions[number]));
                    questions.Add(baseQuestions[number]);

                }

                return true;
            }
        }

        public Question GetQuestion()
        {
           
            return questions[activeQuestionIndex];
        }

        public void SaveAnswer(string answer)
        {
            var question = questions[activeQuestionIndex];
            if (question.Answer == answer)
                Score++;
        }

        public bool MoveToNextQuestion()
        {
            bool canMove = false;

            if (questions.Count()  > activeQuestionIndex + 1)
            {
                activeQuestionIndex++;
                canMove = true;
            }

            return canMove;
        }

        public bool PreviosQuestion()
        {
            bool canMove = false;

            if (activeQuestionIndex >= 1)
            {
                activeQuestionIndex--;
                canMove = true;
            }

            return canMove;
        }



    }
}
