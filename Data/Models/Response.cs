using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    [Serializable]
    public class Response
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }

        public String AnswerText { get; set; }

        public float Score { get; set; }
    }
}
