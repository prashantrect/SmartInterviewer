using System;

namespace Data.Models
{
    [Serializable]
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public string Answer { get; set; }
        public bool LuisMatchRequired { get; set; }
    }
}