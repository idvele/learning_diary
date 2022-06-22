using System;
using System.Collections.Generic;

#nullable disable

namespace Learning_Diary_IK.Models
{
    public partial class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double? TimeToMaster { get; set; }
        public double? TimeSpent { get; set; }
        public string Source { get; set; }
        public DateTime? StartLearningDate { get; set; }
        public DateTime? Completion { get; set; }
        public bool? InProgres { get; set; }
    }
}
