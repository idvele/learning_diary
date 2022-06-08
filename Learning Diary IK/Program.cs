using System;
using System.Collections.Generic;
using System.IO;


namespace Learning_Diary_IK
{
    class Program
    {


        static void Main(string[] args)
        {

            int inputCounter = 0;
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

                            Console.WriteLine(inputCounter);
                            Topic inputcounter = new Topic();

                            Console.WriteLine("Enter topic ID: ");
                            inputcounter.Id = int.Parse(Console.ReadLine());

                            Console.WriteLine("Enter title");
                            inputcounter.Title = (Console.ReadLine());

                            diaryEntrys.Add(inputcounter);
                            inputCounter++;
                            break;

                        case "2":
                            inputCounter = 1;
                            foreach (var item in diaryEntrys)
                            {

                                Console.WriteLine("Tässä listan {0} kaikki inputit", inputCounter);
                                Console.WriteLine("ID = {0}\n" +
                                    "Title = {1}\n", item.Id, item.Title);

                                string path = @"C:\Users\ilari\source\repos\AW academy kurssitehtävät\learning diary Ik\teksti.txt";


                                string print = "ID = "+ item.Id.ToString() + " \n" +"Title = " + item.Title;

                                File.AppendAllText(path, print + Environment.NewLine);

                                inputCounter++;
                            }
                            Console.ReadKey();
                            break;

                    }
                }

            } while (input == "1");

            //TODO: Objekteista tehdään List jossa jokaisella Topic-objektilla on indeksin mukainen paikka
            // Kysymykset on nyt metodina, sen tulokset voisi tallentaa ArrayListiin josta saa tehtyä metodin, jolla tulokset on helppoa printata
            //tee main menu jossa lisätään uusia osia topicceihin tai printataan topicit i määrittää ...











        }
    
            //public static Topic kysymykset()
            //{



                
            //    Topic mytopic = new Topic();

            //    Console.Write("Enter topic Id: ");
            //    mytopic.Id = int.Parse(Console.ReadLine());

            //    Console.WriteLine("Enter Title: ");
            //    mytopic.Title = Console.ReadLine();

            //    Console.WriteLine("Enter Description: ");
            //    mytopic.Description = Console.ReadLine();

            //    Console.WriteLine("Enter ETA to master: ");
            //    mytopic.EstimatedTimeToMaster = double.Parse(Console.ReadLine());

            //    Console.WriteLine("Enter Time spent: ");
            //    mytopic.TimeSpent = double.Parse(Console.ReadLine());

            //    Console.WriteLine("Enter Source: ");
            //    mytopic.Source = Console.ReadLine();


            //    Console.WriteLine("Enter start learning date as dd/mm/yyyy: ");
            //    var str = Console.ReadLine();
            //    DateTime dt;

            //    var isValidDate = DateTime.TryParse(str, out dt);

            //    if (isValidDate)
            //    {
            //        Console.WriteLine(mytopic.StartLearningDate);
            //        mytopic.StartLearningDate = dt;
            //    }

            //    else
            //        Console.WriteLine($"{str} is not a valid date string");

            //    Console.WriteLine("Enter in progress?(1 = yes 2= no : ");

            //    string answer = Console.ReadLine();
            //    if (answer == "1")
            //        mytopic.inProgress = true;
            //    else if (answer == "2")
            //        mytopic.inProgress = false;
            //    else
            //        mytopic.inProgress = false;

            //    Console.WriteLine("Enter completion date as dd/mm/yyyy: ");
            //    var str1 = Console.ReadLine();
            //    DateTime dt1;

            //    var isValidDate1 = DateTime.TryParse(str, out dt);

            //    if (isValidDate)
            //    {
            //        Console.WriteLine(mytopic.CompletionDate);
            //        mytopic.CompletionDate = dt;
            //    }

            //    else
            //        Console.WriteLine($"{str} is not a valid date string");



            //    return mytopic;

            //}


        }
    }
