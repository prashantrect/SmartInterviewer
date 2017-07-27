using System;

namespace Data.Models
{
    [Serializable]
    public class Interview
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public float Score { get; set; }
        public bool IsPass { get; set; }

    }
}