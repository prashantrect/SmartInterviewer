using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Data
{
    public class RepositoryService
    {
        private readonly InterviewDataContext context = new InterviewDataContext();

        public IQueryable<Skill> GetSkills()
        {

            IQueryable<Skill> query = context.Skills;
            return query;
        }
    }
}
