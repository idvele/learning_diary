using System;
using System.Collections.Generic;


namespace Learning_Diary_IK
{
    class Program
    {
        static void Main(string[] args)
        {

            //TODO: Objekteista tehdään List jossa jokaisella Topic-objektilla on indeksin mukainen paikka
            // Kysymykset on nyt metodina, sen tulokset voisi tallentaa ArrayListiin josta saa tehtyä metodin, jolla tulokset on helppoa printata
            //tee main menu jossa lisätään uusia osia topicceihin tai printataan topicit i määrittää ...
            //This is a test comment
            //This is also a test
            List<Topic> topics = new List<Topic>();

            topics.Add(kysymykset());

           

            foreach (var item in topics)
            {
                //tähän looppi arrayListille
                foreach (var item in mbox)
                {

                }
                
            }
            Console.ReadKey();

       
        }

        //Lorem ipsum this is a comment for GitHub test

        public static Topic kysymykset()
        {
            //nÄmä arraylistiin
            Topic mytopic = new Topic();

            Console.Write("Enter topic Id: ");
            mytopic.Id = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Title: ");
            mytopic.Title = Console.ReadLine();

            Console.WriteLine("Enter Description: ");
            mytopic.Description = Console.ReadLine();

            Console.WriteLine("Enter ETA to master: ");
            mytopic.EstimatedTimeToMaster = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter Time spent: ");
            mytopic.TimeSpent = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter Source: ");
            mytopic.Source = Console.ReadLine();


            Console.WriteLine("Enter start learning date as dd/mm/yyyy: ");
            var str = Console.ReadLine();
            DateTime dt;

            var isValidDate = DateTime.TryParse(str, out dt);

            if (isValidDate)
            {
                Console.WriteLine(mytopic.StartLearningDate);
                mytopic.StartLearningDate = dt;
            }

            else
                Console.WriteLine($"{str} is not a valid date string");

            Console.WriteLine("Enter in progress?(1 = yes 2= no : ");

            string answer = Console.ReadLine();
            if (answer == "1")
                mytopic.inProgress = true;
            else if (answer == "2")
                mytopic.inProgress = false;
            else
                mytopic.inProgress = false;

            Console.WriteLine("Enter completion date as dd/mm/yyyy: ");
            var str1 = Console.ReadLine();
            DateTime dt1;

            var isValidDate1 = DateTime.TryParse(str, out dt);

            if (isValidDate)
            {
                Console.WriteLine(mytopic.CompletionDate);
                mytopic.CompletionDate = dt;
            }

            else
                Console.WriteLine($"{str} is not a valid date string");

            return mytopic;

        }

        
    }
}
