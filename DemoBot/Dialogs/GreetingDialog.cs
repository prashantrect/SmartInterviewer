using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DemoBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync($"Hi, I am Prashant. I am your interviewer.");
            await Respond(context);
            context.Wait(MessageReceivedAsync);
        }

        private static async Task Respond(IDialogContext context)
        {
            var username = string.Empty;
            context.UserData.TryGetValue<string>("Name", out username);
            if (string.IsNullOrEmpty(username))
            {
                await context.PostAsync("What is your name");
                context.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                await context.PostAsync($"Hi {username}. Wish you good luck for this interview. Type Start when ready.");
            }
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            var activity = await result;
            var username = string.Empty;
            var getName = false;
            context.UserData.TryGetValue("Name", out username);
            context.UserData.TryGetValue("GetName", out getName);

            if (getName)
            {
                username = activity.Text;
                context.UserData.SetValue("Name", username);
                context.UserData.SetValue("GetName", false);
            }
            await Respond(context);
            context.Done(activity);
        }
    }
}