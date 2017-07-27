using System.Data.Entity;
using Data.Models;

namespace Data
{
    
        public class InterviewDataContext : DbContext
        {
            public InterviewDataContext()
                : base("InterviewDataContextConnectionString")
            {
                Database.SetInitializer(new InterviewDbInitializer());

               

        }

        public DbSet<Candidate> Candidates { get; set; }
            public DbSet<Interview> Interviews { get; set; }
            public DbSet<Question> Questions { get; set; }
            public DbSet<Skill> Skills { get; set; }
            public DbSet<Topic> Topics { get; set; }
            public DbSet<Response> Responses { get; set; }
    }
}
