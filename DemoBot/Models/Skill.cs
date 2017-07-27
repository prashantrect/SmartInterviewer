using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;

namespace DemoBot.Models
{
    [Serializable]
    public enum Skill
    {
        [Terms(new string[] { ".NET", "DOT NET"})]
        DOTNET = 1,
        JAVA,
        [Terms(new string[] { "SAP", "OER", "SAP OER" })]
        SAPOER
    }
}