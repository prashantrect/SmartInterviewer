using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace DemoBot.Models
{
    [Serializable]
    public class InterviewCandidate
    {
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        
        public List<Skill> Skills;

        public static IForm<InterviewCandidate> BuildForm()
        {
            return new FormBuilder<InterviewCandidate>()
                .OnCompletion(async (context, profileForm) =>
                {

                    // Save  Candidate Data
                    try
                    {
                        var userName = context.UserData.GetValue<string>("Name");

                        List<Skill> skills = profileForm.Skills;
                        var candidateSkills = skills.Select(s => s.ToString()).ToList();

                        var candidate = new Candidate
                        {
                            Name = userName,
                            Email = profileForm.Email,
                            Phone = profileForm.PhoneNumber,
                            Skills = String.Join(",", candidateSkills)
                        };

                        using (var dbContext = new InterviewDataContext())
                        {
                            candidate = dbContext.Candidates.Add(candidate);
                            dbContext.SaveChanges();
                        }
                        context.UserData.SetValue<int>("CandidateId", candidate.Id);

                        await context.PostAsync("");
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }

                    // Tell the user that the form is complete
                    
                })
                .Build();
        }

       
    }
}