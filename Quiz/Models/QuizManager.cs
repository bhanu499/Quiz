using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz.Models
{
    [Serializable]
    public class QuizManager
    {
        public List<Question> baseQuestions = new List<Question>();
        public List<Question> questions = new List<Question>();
        public int activeQuestionIndex = 0;
        public int Score = 0;
        public bool IsComplete = false;

        public QuizManager(bool initializeData = false)
        {
            if (initializeData)
            {
                for (int i = 0; i < 20; i++)
                {
                    baseQuestions.Add(new Question()
                    {
                        Topic = "Basics",
                        QuestionId = i,
                        QuestionDescription = "Shape of earth",
                        Option1 = i + "",
                        Option2 = i + "",
                        Option3 = "Round",
                        Option4 = i + "",
                        Answer = "Round"
                    });
                }
            }
        }

        public bool IntializeRandomQuestions(int count, string topic = null)
        {
            var totalQuestions = baseQuestions;
            if(topic != null)
            {
                totalQuestions = totalQuestions.FindAll((question) => question.Topic == topic);
            }
            if (count > totalQuestions.Count)
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
                        number = r.Next(1, totalQuestions.Count);
                    } while (questions.Contains(totalQuestions[number]));
                    questions.Add(totalQuestions[number]);

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
