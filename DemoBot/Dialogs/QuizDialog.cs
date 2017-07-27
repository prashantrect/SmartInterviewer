using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Data;
using Data.Models;
using DemoBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace DemoBot.Dialogs
{
    [Serializable]
    public class QuizDialog : IDialog<string>
    {
        List<Response> _responses = new List<Response>();
        private List<Question> _questions;
        int _questionAsked;
        private string _reply;

        public async Task StartAsync(IDialogContext context)
        {
            var user = context.UserData.GetValue<string>("Name");
            var skills = GetMatchingSkills(context);
            if (skills.Count == 0)
            {
                _reply = "NoSkillMatch";
                context.Done(_reply);
            }
            else
            {

                _questions = GetQuestionaireFromSkills(skills);
                if (_questions.Count == 0)
                {
                    _reply = "NoQuestions";
                    context.Done(_reply);
                }
                else
                {
                    context.PrivateConversationData.SetValue("Questions", _questions);
                    await context.PostAsync("Let's start technical Q & A");
                    await AskQuestion(context);
                }
            }
            
        }

        private async Task AskQuestion(IDialogContext context)
        {

            if (_questionAsked != _questions.Count)
            {
                context.PrivateConversationData.SetValue("CurrentQuestion", _questions[_questionAsked].Id);
                await context.PostAsync(_questions[_questionAsked].Text);
                _questionAsked++;
                context.Wait(AnswerReceivedAsync);
            }
            else
            {
                await CalculateResult(context);
                _reply = "Complete";
                context.Done(_reply);
            }
        }

        private async Task CalculateResult(IDialogContext context)
        {
            var totalScore = _responses.Sum(x => x.Score);
            bool isPass;
            if (totalScore >= 3)
            {
                isPass = true;
                await context.PostAsync("Congratulations, You have cleared intial screening round.");
            }
            else
            {
                isPass = false;
                await context.PostAsync("Sorry, You have not cleared the intial screening round.");
            }
            using (var dbContext = new InterviewDataContext())
            {
                dbContext.Responses.AddRange(_responses);

                dbContext.Interviews.Add(new Interview
                {
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Score = totalScore,
                    IsPass = isPass
                });
                dbContext.SaveChanges();
            }
        }

        private async Task AnswerReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result;
            var questionId = context.PrivateConversationData.GetValue<int>("CurrentQuestion");
            if (!string.IsNullOrEmpty(activity.Text))
            {
                var response = activity.Text;
                var score = await GetScore(questionId, response, context);
                _responses.Add(new Response
                {
                    QuestionId = questionId,
                    CandidateId = context.UserData.GetValue<int>("CandidateId"),
                    AnswerText = response,
                    Score = score
                });
                await AskQuestion(context);
            }
            else
            {
                await context.PostAsync("Response is not recognized.");
                context.Wait(AnswerReceivedAsync);
            }
        }

        private async Task<float> GetScore(int questionId, string response, IDialogContext context)
        {
            List<Question> questions = context.PrivateConversationData.GetValue<List<Question>>("Questions");
            var currentQuestion = questions.First(x => x.Id == questionId);
            var correctAnswer = currentQuestion.Answer;
            if (currentQuestion.LuisMatchRequired)
            {
                var isMatch = await GetLuisMatch(currentQuestion, response);
                return isMatch ? 1 : 0;
            }
            return response.IndexOf(correctAnswer, StringComparison.CurrentCultureIgnoreCase) > -1 ? 1 : 0;
        }

        private async Task<bool> GetLuisMatch(Question question, string response)
        {
            LuisDataModel ldm = await GetIntentFromLuis(response);
            if (ldm.entities != null && ldm.entities.Length > 0)
            {
                return string.Compare(ldm.entities[0].entity, question.Answer,
                           StringComparison.CurrentCultureIgnoreCase) == 0;
            }
            return true;
        }

        private async Task<LuisDataModel> GetIntentFromLuis(string response)
        {
            LuisDataModel data = new LuisDataModel();
            var query = Uri.EscapeDataString(response);
            using (HttpClient client = new HttpClient())
            {
                string requestUri =
                    "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/2e4b6b0c-7d24-441a-9d86-ce4c2681e2e5?subscription-key=dfc477d9a49f47199401e3061ff8e1b2&timezoneOffset=330&verbose=true&q=" +
                    query;
                HttpResponseMessage msg = await client.GetAsync(requestUri);

                if (msg.IsSuccessStatusCode)
                {
                    var jsonDataResponse = await msg.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<LuisDataModel>(jsonDataResponse);
                }
            }
            return data;
        }

        private List<Question> GetQuestionaireFromSkills(List<Data.Models.Skill> skills)
        {
            List<Question> questionairre = new List<Question>();
                foreach (var skill in skills)
                {
                    var questions = skill.Topics.Select(x => x.Questions);

                    questionairre.AddRange(questions.SelectMany(x => x).ToList());
                }
            

            return questionairre;
        }

        private List<Data.Models.Skill> GetMatchingSkills(IDialogContext context)
        {
            using (var dbContext = new InterviewDataContext())
            {
                var candidateId = context.UserData.GetValue<int>("CandidateId");
                var candidate = dbContext.Candidates.First(x => x.Id == candidateId);

                var skills = candidate.Skills.Split(',');

                var test = dbContext.Skills.Include("Topics.Questions").ToList();
                var matchingSkills = test
                    .Where(x => skills.Contains(x.Name, StringComparer.OrdinalIgnoreCase)).ToList();
                return matchingSkills;
            }
        }
    }
}