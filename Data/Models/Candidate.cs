using System;
using System.Collections.Generic;

namespace Data.Models
{
    [Serializable]
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public string Skills { get; set; }
    }
}