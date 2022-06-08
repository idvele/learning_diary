using System;


namespace Learning_Diary_IK
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double EstimatedTimeToMaster { get; set; }
        public double TimeSpent { get; set; }
        public string Source { get; set; }
        public DateTime StartLearningDate { get; set; }
        public bool inProgress { get; set; }
        public DateTime CompletionDate { get; set; }


        //override mahdollistaa komennon tekemisen classin nimellä
       public string Write()
        {
            string entrys = String.Format("Id {0}, Title {1}, Description {2}, " +
                "Estimated time to master {3}, Time Spent {4}", Id, Title, Description, EstimatedTimeToMaster, TimeSpent);

            return entrys;
         }

    }
}
