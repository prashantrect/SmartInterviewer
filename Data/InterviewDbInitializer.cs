using System.Collections.Generic;
using System.Data.Entity;
using Data.Models;

namespace Data
{
    public class InterviewDbInitializer : CreateDatabaseIfNotExists<InterviewDataContext>
    {
        protected override void Seed(InterviewDataContext context)
        {

            IList<Skill> defaultSkills = new List<Skill>();

            defaultSkills.Add(new Skill
            {
                Id = 1,
                Name = "SAPOER",
                Description = "SAPOER",
                Topics = new List<Topic>
                {
                    new Topic
                    {
                        Id = 1,
                        Name = "Parameter",
                        Description = "Parameter",
                        Questions = new List<Question>()
                        {
                            new Question()
                            {
                                Id = 1,
                                Text = @"Is name value pair allowed in Info Paramter",
                                Answer = "Yes",
                                LuisMatchRequired = false
                            },
                            new Question()
                            {
                                Id = 2,
                                Text = @"Use of Indexing in Control Parameters",
                                Answer = "multiple entries",
                                LuisMatchRequired = true
                            }
                        }
                    },
                    new Topic
                    {
                        Id = 2,
                        Name = "TrackingID",
                        Description = "Tracking ID",
                        Questions = new List<Question>()
                        {
                            new Question()
                            {
                                Id = 3,
                                Text = @"Are multiple TrackingIDs possible in OER",
                                Answer = "Yes",
                                LuisMatchRequired = false
                            }
                        }
                    },
                    new Topic
                    {
                        Id = 3,
                        Name = "Archiving",
                        Description = "Archiving",
                        Questions = new List<Question>()
                        {
                            new Question()
                            {
                                Id = 4,
                                Text = @"What is Tcode for Archiving in OER",
                                Answer = "SARA",
                                LuisMatchRequired = false
                            }
                        }
                    },
                    new Topic
                    {
                        Id = 4,
                        Name = "Alert Framework",
                        Description = "Alert Framework",
                        Questions = new List<Question>()
                        {
                            new Question()
                            {
                                Id = 5,
                                Text = @"What is Tcode for Alert Category Definition in OER",
                                Answer = "ALRTCATDEF",
                                LuisMatchRequired = false
                            }
                        }
                    }
                }
            });

            defaultSkills.Add(
                new Skill
                {
                    Id = 2,
                    Name = "JAVA",
                    Description = "JAVA",
                    Topics = new List<Topic>
                    {
                        new Topic
                        {
                            Id = 5,
                            Name = "Datatype",
                            Description = "Data Type",
                            Questions = new List<Question>()
                            {
                                new Question()
                                {
                                    Id = 6,
                                    Text = @"What is default value of byte datatype in Java",
                                    Answer = "0",
                                    LuisMatchRequired = false
                                },
                                
                            }
                        },
                        new Topic
                        {
                            Id = 6,
                            Name = "Oops",
                            Description = "OOPs",
                            Questions = new List<Question>()
                            {
                                new Question()
                                {
                                    Id = 7,
                                    Text = @"Define Inheritence",
                                    Answer = "Acquires properties",
                                    LuisMatchRequired = true
                                }
                            }
                        },
                        new Topic
                        {
                            Id = 7,
                            Name = "AbstractClass",
                            Description = "Abstract Class",
                            Questions = new List<Question>()
                            {
                                new Question()
                                {
                                    Id = 8,
                                    Text = @"Which class cannot be istantiated but can be inherited",
                                    Answer = "Abstract",
                                    LuisMatchRequired = false
                                }
                            }
                        },
                        new Topic
                        {
                            Id = 8,
                            Name = "Interface",
                            Description = "Interface",
                            Questions = new List<Question>()
                            {
                                new Question()
                                {
                                    Id = 9,
                                    Text = @"Wht do you call a collection of Abstract methods",
                                    Answer = "Interface",
                                    LuisMatchRequired = false
                                }
                            }
                        },
                        new Topic
                        {
                            Id = 9,
                            Name = "Applet",
                            Description = "Applet",
                            Questions = new List<Question>()
                            {
                                new Question()
                                {
                                    Id = 10,
                                    Text = @"An Applet extends which class",
                                    Answer = "java.applet.Applet",
                                    LuisMatchRequired = false
                                }
                            }
                        }
                    }
                });
            

            foreach (Skill skill in defaultSkills)
                context.Skills.Add(skill);

            base.Seed(context);
        }
    }
}