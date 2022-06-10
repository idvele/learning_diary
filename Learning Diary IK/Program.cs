using System;
using System.Collections.Generic;
using System.IO;


namespace Learning_Diary_IK
{
    class Program
    {


        static void Main(string[] args)
        {



            
            string input;

            List<Topic> diaryEntrys = new List<Topic>();
            do
            {


                Console.WriteLine("This is a learning diary");
                Console.WriteLine("Press 1 to add an item to diary, press 2 to print whole diary");


                {
                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            //tee inputcounterista automaattisesti ID-numero


                            diaryEntrys.Add(kysymykset());
                            //Console.WriteLine(inputCounter);
                            //Topic inputcounter = new Topic();

                            //Console.WriteLine("Enter topic ID: ");
                            //inputcounter.Id = int.Parse(Console.ReadLine());

                            //Console.WriteLine("Enter title");
                            //inputcounter.Title = (Console.ReadLine());

                            //diaryEntrys.Add(inputcounter);
                            //inputCounter++;
                            break;

                        case "2":
                            int inputCounter = 1;
                            foreach (var item in diaryEntrys)
                            {

                                Console.WriteLine("Tässä listan {0} kaikki inputit", inputCounter);
                                Console.WriteLine("ID = {0}\n" +
                                    "Title = {1}\n" 
                                    + "Description = {2}\n"
                                    + "ETA to master = {3}\n"
                                    + "Time Spent = {4}\n"
                                    + "Source = {5}\n"
                                    + "Start time = {6}\n"
                                    + "In progress = {7}\n"
                                    
                                    , item.Id, item.Title, item.Description, item.EstimatedTimeToMaster, item.TimeSpent
                                    ,item.Source, item.StartLearningDate.ToShortDateString(), item.inProgress);

                                    if (item.inProgress == false)
                                    Console.WriteLine(" \n" + "CompletionDate = " + item.CompletionDate.ToShortDateString()); 
                               


                                writeToFile(item.Id, item.Title, item.Description, item.EstimatedTimeToMaster, item.TimeSpent
                                    , item.Source, item.StartLearningDate, item.inProgress, item.CompletionDate);

                                inputCounter++;
                            }
                            Console.ReadKey();
                            break;

                    }
                }

            } while (input == "1");












        }

        public static Topic kysymykset()
        {

            //muuta osa kysymyksistä ohjelmallisiksi


            Topic mytopic = new Topic();

            Console.Write("Enter topic Id: ");
            mytopic.Id = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter Title: ");
            mytopic.Title = Console.ReadLine();

            Console.WriteLine("Enter Description: ");
            mytopic.Description = Console.ReadLine();

            Console.WriteLine("Enter ETA to masterin hours: ");
            mytopic.EstimatedTimeToMaster = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter Time spent in hours: ");
            mytopic.TimeSpent = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter Source: ");
            mytopic.Source = Console.ReadLine();


            Console.WriteLine("Enter when you started studying dd,mm,yyyy : ");

            bool i = false;
            while (i == false)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(Console.ReadLine());
                    mytopic.StartLearningDate = date;
                    Console.WriteLine(date.ToShortDateString());
                    i = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR: Enter correct format");
                    
                }
            }
           
            
           
            Console.WriteLine("Enter in progress?(1 = yes 2= no): ");

   

            string answer = Console.ReadLine();
            if (answer == "2")
            {
                mytopic.inProgress = false;

                bool p = false;
                Console.WriteLine("Enter completion date as dd,mm,yyyy: ");
                while (p == false)
                {
                    try
                    {
                        DateTime date1 = Convert.ToDateTime(Console.ReadLine());
                        mytopic.CompletionDate = date1;
                        Console.WriteLine(date1.ToShortDateString());
                        p = true;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("ERROR: Enter correct format");

                    }
                }
            }

            else if (answer == "1")
                mytopic.inProgress = true;
            
            else
                mytopic.inProgress = false;

            


            //var str1 = Console.ReadLine();
            //DateTime dt1;

            //var isValidDate1 = DateTime.TryParse(str1, out dt1);

            //if (isValidDate1)
            //{
            //    Console.WriteLine(mytopic.CompletionDate);
            //    mytopic.CompletionDate = dt1;
            //}

            //else
            //    Console.WriteLine($"{str1} is not a valid date string");

            
            

            return mytopic;

        }


        public static void writeToFile(int Id, string title, string Description, double EstimatedTimeToMaster, 
            double TimeSpent, string Source, DateTime StartLearningDate, bool inProgress,
            DateTime CompletionDate)
        {

            string path = @"C:\Users\ilari\source\repos\AW academy kurssitehtävät\learning diary Ik\teksti.txt";


            string print = "ID = " + Id.ToString()
                + " \n" + "Title = " + title
                  + " \n" + "Description = " + Description
                  + " \n" + "ETA to master = " + EstimatedTimeToMaster.ToString()
                  + " \n" + "Time Spent = " + TimeSpent.ToString()
                  + " \n" + "Source = " + Source
                  + " \n" + "Start learning date = " + StartLearningDate.ToShortDateString()
                  + " \n" + "inProgress = " + inProgress.ToString();
            //print the completion date only if subject is not in progress
            string completionDate;
            if (inProgress == false)
                completionDate = " \n" + "CompletionDate = " + CompletionDate.ToShortDateString();
            else completionDate = null;


            File.AppendAllText(path, print+ completionDate + Environment.NewLine+ Environment.NewLine);


        }
        }
    }
