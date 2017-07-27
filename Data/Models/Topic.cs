using System;
using System.Collections.Generic;

namespace Data.Models
{
    [Serializable]
    public class Topic
    {
        
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }
    }
}