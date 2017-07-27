using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    [Serializable]
    public class Skill
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public List<Topic> Topics { get; set; }
    }
}

