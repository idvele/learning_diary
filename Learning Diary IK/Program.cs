using System;
using System.Collections.Generic;
using System.IO;


namespace Learning_Diary_IK
{
    class Program
    {


        static void Main(string[] args)
        {

            Console.WriteLine("----------------------------------------");
            Console.WriteLine("`^*Welcome to Ilari's learning diary^*`´");
            Console.WriteLine("----------------------------------------");

            string input;

            List<Topic> diaryEntrys = new List<Topic>();
            do
            {


                
                Console.WriteLine("Press 1 to add an item to diary\nPress 2 to print whole diary\nPress 3 to quit ");


                {
                    input = Console.ReadLine();

                    switch (input)
                    {
                        case "1":
                            //tee inputcounterista automaattisesti ID-numero


                            diaryEntrys.Add(kysymykset());
                            
                            break;

                        case "2":
                            Console.Clear();

                            int inputCounter = 1;
                            foreach (var item in diaryEntrys)
                            {

                                Console.WriteLine("---------------------------------------------");
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
                        case "3":
                            break;

                    }
                }

            } while (input != "3");












        }

        public static Topic kysymykset()
        {

            //muuta osa kysymyksistä ohjelmallisiksi


            Topic mytopic = new Topic();

            bool t;
            Console.Write("Enter topic Id: ");
            do 
            {   t = int.TryParse(Console.ReadLine(), out int a);
                if (t) mytopic.Id = a;
                else Console.WriteLine("ERROR: Enter a number");
            } while (t==false);
            
            


            Console.WriteLine("Enter Title: ");
            mytopic.Title = Console.ReadLine();

            Console.WriteLine("Enter Description: ");
            mytopic.Description = Console.ReadLine();
           
            bool e;
            Console.WriteLine("Enter ETA to masterin hours: ");
            do
            {
                e = double.TryParse(Console.ReadLine(), out double a);
                if (e) mytopic.EstimatedTimeToMaster = a;
                else Console.WriteLine("ERROR: Enter a number");

            } while (e==false);

                                    
            bool h;
            Console.WriteLine("Enter Time spent in hours: ");
            do
            {
                h = double.TryParse(Console.ReadLine(), out double a);
                if (h) mytopic.TimeSpent = a;
                else Console.WriteLine("ERROR: Enter a number");

            } while (h == false);

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
                  
           
            return mytopic;

        }


        public static void writeToFile(int Id, string title, string Description, double EstimatedTimeToMaster, 
            double TimeSpent, string Source, DateTime StartLearningDate, bool inProgress,
            DateTime CompletionDate)
        {

            string path = @"C:\Users\ilari\source\repos\AW academy kurssitehtävät\learning diary Ik\teksti.txt";


            string print = "------------------"+
                "\n" + "ID = " + Id.ToString()
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
