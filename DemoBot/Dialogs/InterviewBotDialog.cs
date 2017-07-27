using System;
using System.Threading.Tasks;
using DemoBot.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace DemoBot.Dialogs
{
    [Serializable]
    public class InterviewBotDialog: IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Welcome to Microsoft Online Interview Portal");
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result; // We've got a message!
            if (message.Text.ToLower().Contains("hi") || message.Text.ToLower().Contains("hello"))
            {
                context.Call(new GreetingDialog(), this.ResumeAfterGreetingDialog);
            }
        }

        private async Task ResumeAfterGreetingDialog(IDialogContext context, IAwaitable<string> result)
        {
            var myForm = new FormDialog<InterviewCandidate>(new InterviewCandidate(), InterviewCandidate.BuildForm, FormOptions.PromptInStart, null);
            context.Call(myForm, this.ResumeAfterInterviewDialog);
        }
        private async Task ResumeAfterInterviewDialog(IDialogContext context, IAwaitable<InterviewCandidate> result)
        {
            context.Call(new QuizDialog(), this.AfterInterviewContinuation);
        }

        private async Task AfterInterviewContinuation(IDialogContext context, IAwaitable<string> result)
        {
            var output = await result;
            switch(output)
            {
                case "NoQuestions":
                    await context.PostAsync($"Sorry, we do not have questions for your skill set.");
                    break;
                case "NoSkillMatch":
                    await context.PostAsync($"Sorry, your skill set does not match with the required skill sets at the moment. We have saved  your skillset with your contact details to contact you in future.");
                    break;
                case "Complete":
                    break;
            }
            
            await context.PostAsync($"Thank you! Your interview is completed. You can go ahead and close the chat window.");

            var msg = context.MakeMessage();
            var stateClient = new StateClient(new Uri(msg.ServiceUrl));
            stateClient.BotState.DeleteStateForUser(msg.ChannelId, msg.From.Id);
            
            context.Wait(MessageReceivedAsync);
        }

        //private async Task Reset(IDialogContext context)
        //{
        //    var activity = context.Activity;
        //    activity.GetStateClient().BotState.DeleteStateForUser(activity.ChannelId, activity.From.Id);
        
       
        //}

    }
}