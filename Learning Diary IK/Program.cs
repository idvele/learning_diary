using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Learning_Diary_IK.Models;
using ClassLibrary1;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;

namespace Learning_Diary_IK
{
    class Program
    {
        public static int GlobalID = 1;
        public static Dictionary<int, Topic> diaryEntrys = new Dictionary<int, Topic>();
        public static Dictionary<int, Models.Topic> diaryEntrysModels = new Dictionary<int, Models.Topic>();
        public static Dictionary<string, int> IdTitlePairs = new Dictionary<string, int>();

        static async Task Main(string[] args)
        {
            //Tähän tulee databasen lataus asynkronisesti

            new Thread(() =>
            {
                using (var LearningDiary = new LearningDiaryContext())
                {
                    var taulu = LearningDiary.Topics.Max(topikki => topikki.Id);

                    //Lasketaan missä ID:ssä mennään
                    GlobalID = taulu;
                    GlobalID++;

                    //Kaikki tietokannassa olevat objektit lisätään diaryEntrysModels-dictionaryyn
                    var kaikki =LearningDiary.Topics;

                    foreach (var topic in kaikki)
                    {
                        diaryEntrysModels.Add(topic.Id, topic);
                        IdTitlePairs.Add(topic.Title, topic.Id);

                    }
                }
            }).Start();
                
            
            

           

                


                Console.WriteLine("----------------------------------------");
            Console.WriteLine("`^*Welcome to Ilari's learning diary^*`´");
            Console.WriteLine("----------------------------------------");

            string input;

            
            do
            {


                //miksi vitsin jälkeen printtautuu kahdesti?
                Console.WriteLine("Press 1 to add an item to diary\n" +
                    "Press 2 to print whole diary\n" +
                    "Press 3 to search subject via ID or title\n" +
                    "Press 4 to save and exit\n"+
                    "Press 5 to hear a joke");


                {
                    input = Console.ReadLine();

                    switch (input)
                    {
                        //add to class
                        case "1":
                        diaryEntrys.Add(GlobalID, kysymykset());
                        
                            GlobalID++;

                            break;
                        
                            
                            //Print all
                        case "2":
                            Console.Clear();


                            //print from database
                            foreach (int i in diaryEntrysModels.Keys)
                            {
                                //call classLibrary1
                                Class1 aikataulu = new Class1();

                                Models.Topic haku = null;

                                diaryEntrysModels.TryGetValue(i, out haku);

                                Console.WriteLine("---------------------------------------------");

                                Console.WriteLine("ID = {0}\n" +
                                "Title = {1}\n"
                                + "Description = {2}\n"
                                + "ETA to master = {3}\n"
                                + "Time Spent = {4}",

                                haku.Id, haku.Title, haku.Description, haku.TimeToMaster, haku.TimeSpent);
                               
                                // use classLibary1 DLL
                                aikataulu.IsLate(haku.TimeToMaster, haku.TimeSpent);

                                Console.WriteLine("Source = {0}\n"
                                    + "Start time = {1}\n"
                                    + "In progress = {2}\n"
                                , haku.Source, haku.StartLearningDate.ToString(), haku.InProgres);

                                if (haku.InProgres == false)
                                    Console.WriteLine("CompletionDate = " + haku.Completion.ToString());







                            }
                            //print from current session
                                foreach (int s in diaryEntrys.Keys)
                            {

                                 //call classLibrary1
                                Class1 aikataulu = new Class1();

                                Topic haku = null;

                                diaryEntrys.TryGetValue(s, out haku);

                                Console.WriteLine("---------------------------------------------");

                                Console.WriteLine("ID = {0}\n" +
                                "Title = {1}\n"
                                + "Description = {2}\n"
                                + "ETA to master = {3}\n"
                                + "Time Spent = {4}",

                                haku.Id, haku.Title, haku.Description, haku.EstimatedTimeToMaster, haku.TimeSpent);
                               
                                // use classLibary1 DLL
                                aikataulu.IsLate(haku.EstimatedTimeToMaster, haku.TimeSpent);

                                Console.WriteLine("Source = {0}\n"
                                    + "Start time = {1}\n"
                                    + "In progress = {2}\n"
                                , haku.Source, haku.StartLearningDate.ToString(), haku.inProgress);

                                if (haku.inProgress == false)
                                    Console.WriteLine("CompletionDate = " + haku.CompletionDate.ToString());


                                writeToFile(haku.Id, haku.Title, haku.Description, haku.EstimatedTimeToMaster, haku.TimeSpent
                                    , haku.Source, haku.StartLearningDate, haku.inProgress, haku.CompletionDate);

                                

                            }

                            Console.ReadKey();
                            break;
                        
                            
                            //Search by Id or by Title
                        case "3":

                            //tallenna tällä kerrala luodut titlet databaseen ja hae sieltä
                            using (var LearningDiary = new LearningDiaryContext())
                            {
                                //Jokaiselle tällä kerralla luodulle topicille tehdään tallennus
                                foreach (var item in diaryEntrys)
                                {
                                    Models.Topic s = new Models.Topic();
                                    Topic y = null;
                                    diaryEntrys.TryGetValue(item.Key, out y);

                                    s.Id = y.Id;
                                    s.Title = y.Title;
                                    s.Description = y.Description;
                                    s.TimeToMaster = y.EstimatedTimeToMaster;
                                    s.TimeSpent = y.TimeSpent;
                                    s.Source = y.Source;
                                    s.StartLearningDate = y.StartLearningDate;
                                    s.InProgres = y.inProgress;
                                    s.Completion = y.CompletionDate;



                                    //uusi lisäys
                                    LearningDiary.Topics.Add(s);

                                }
                                //tee tallennus databaseen
                               
                                
                                //Tallennus tehdään asynkronisesti
                                LearningDiary.SaveChangesAsync();
                                //tyhjennä diaryEntrys dictionary
                                diaryEntrys.Clear();
                            }

                                Console.WriteLine("Do you want to search by 1:ID or 2:Title");
                            string input2 = Console.ReadLine();

                            switch (input2)
                            {
                                case "1":

                                    Console.WriteLine("Enter subject ID");
                                    bool s;

                                    do
                                    {
                                        s = int.TryParse(Console.ReadLine(), out int search);
                                        if (s)
                                        {
                                            searchById(search);

                                            s = false;
                                        }

                                        else
                                            Console.WriteLine("ERROR: enter a number");


                                    } while (s == true);
                                    break;


                                case "2":
                                    Console.WriteLine("Enter subject title");
                                    searchByTitle(Console.ReadLine());



                                    break;

                            }



                           


                            break;

                        
                            
                            //Save and exit

                        case "4":

                            

                            using (var LearningDiary = new LearningDiaryContext())
                            {
                                //Jokaiselle tällä kerralla luodulle topicille tehdään tallennus
                                foreach (var item in diaryEntrys)
                                {
                                    Models.Topic s = new Models.Topic();
                                    Topic y = null;
                                    diaryEntrys.TryGetValue(item.Key, out y);

                                    s.Id = y.Id;
                                    s.Title = y.Title;
                                    s.Description = y.Description;
                                    s.TimeToMaster = y.EstimatedTimeToMaster;
                                    s.TimeSpent = y.TimeSpent;
                                    s.Source = y.Source;
                                    s.StartLearningDate = y.StartLearningDate;
                                    s.InProgres = y.inProgress;
                                     s.Completion = y.CompletionDate;
                                    
                                    



                                    //uusi lisäys
                                    LearningDiary.Topics.Add(s);
                                    

                                    //update

                                }
                                LearningDiary.SaveChanges();
                                
                            }


                            break;

                        case "5":

                            await TellAJoke();
                            System.Threading.Thread.Sleep(1000);


                            break;


                    }
                }

            } while (input != "4");



        }

        public static Topic kysymykset()
        {

            


            Topic mytopic = new Topic();

         
            mytopic.Id = GlobalID;

            

            Console.WriteLine("Enter Title: ");
            mytopic.Title = Console.ReadLine();

            IdTitlePairs.Add(mytopic.Title, GlobalID);

            Console.WriteLine("Enter Description: ");
            mytopic.Description = Console.ReadLine();

            bool e;
            Console.WriteLine("Enter ETA to masterin hours: ");
            do
            {
                e = double.TryParse(Console.ReadLine(), out double a);
                if (e) mytopic.EstimatedTimeToMaster = a;
                else Console.WriteLine("ERROR: Enter a number");

            } while (e == false);


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

            bool t = false;
            //tarkastaa oikean inputin
            do
            {
                string answer = Console.ReadLine();

                DateTime date1 = new DateTime(2222, 2, 2);
                

                if (answer == "2")
                {
                    mytopic.inProgress = false;

                    bool p = false;
                    Console.WriteLine("Enter completion date as dd,mm,yyyy: ");
                    while (p == false)
                    {
                        try
                        {
                            date1 = Convert.ToDateTime(Console.ReadLine());

                            Console.WriteLine(date1.ToShortDateString());
                            mytopic.CompletionDate = date1;
                            p = true;
                            t = true;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("ERROR: Enter correct format");

                        }
                    }

                }

                else if (answer == "1")
                {
                    mytopic.CompletionDate = date1;
                    mytopic.inProgress = true;
                    t = true;
                }
            } while (t == false);
                
            

            


            return mytopic;

        }


        public static void writeToFile(int Id, string title, string Description, double EstimatedTimeToMaster,
            double TimeSpent, string Source, DateTime StartLearningDate, bool inProgress,
            DateTime CompletionDate)
        {

            string path = @"C:\Users\ilari\source\repos\AW academy kurssitehtävät\learning diary Ik\teksti.txt";


            string print = "------------------" +
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

            File.AppendAllText(path, print + completionDate + Environment.NewLine + Environment.NewLine);


        }

        public static void searchById(int search)
        {

            //Search with an ID
            Models.Topic s = new Models.Topic();
            diaryEntrysModels.TryGetValue(search, out s);

            //call classLibrary1
            Class1 aikataulu = new Class1();

            // write the result
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Tässä haetun Id:n {0}, mukaiset tiedot", search);
            Console.WriteLine("ID = {0}\n" +
                "Title = {1}\n"
                + "Description = {2}\n"
                + "ETA to master = {3}\n"
                + "Time Spent = {4}",
               
                s.Id, s.Title, s.Description, s.TimeToMaster, s.TimeSpent);
            // use classLibary1 DLL
            aikataulu.IsLate(s.TimeToMaster, s.TimeSpent);

            Console.WriteLine("Source = {0}\n"
                + "Start time = {1}\n"
                + "In progress = {2}\n"         
            , s.Source, s.StartLearningDate.ToString(), s.InProgres);

            if (s.InProgres == false)
                Console.WriteLine("CompletionDate = " + s.Completion.ToString());



            //Edit or remove object after search
            Console.Write("Press 1 to edit topic and 2 to delete a topic: ");
            
            var inputti = Console.ReadKey();
            if (inputti.KeyChar == '1')
            {
                Console.Clear();
                Console.WriteLine("Enter a subject to change:");
                Console.WriteLine(
            "Title: 1\n"
            + "Description: 2\n"
            + "ETA to master: 3\n"
            + "Time Spent: 4\n"
            + "Source: 5\n"
            + "Start time: 6\n"
            + "In progress: 7\n"
            + "CompletionDate: 8");
                int subject = int.Parse(Console.ReadLine());

                switch (subject)
                {
                    //case 0:
                    //    Console.Write("Enter a new ID: ");
                    //    int newID = int.Parse(Console.ReadLine());
                    //    s.Id = newID;

                    //    using (var LearningDiary = new LearningDiaryContext())
                    //    {
                    //        LearningDiary.Topics.Update(s);
                    //        LearningDiary.SaveChanges();

                    //    }
                    //    break;

                    case 1:
                        Console.WriteLine("Enter a new title: ");
                        string newTitle = Console.ReadLine();
                        s.Title = newTitle;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(s);
                            LearningDiary.SaveChanges();

                        }
                        break;
                        
                    case 2:
                        Console.WriteLine("Enter a new description: ");
                        string newDesc = Console.ReadLine();
                        s.Description = newDesc;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(s);
                            LearningDiary.SaveChanges();

                        }
                        break;
                        
                    case 3:
                        Console.WriteLine("Enter a eta to master: ");

                        try
                        {
                            int newETA = int.Parse(Console.ReadLine());
                            s.TimeToMaster = newETA;
                            using (var LearningDiary = new LearningDiaryContext())
                            {
                                LearningDiary.Topics.Update(s);
                                LearningDiary.SaveChanges();

                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Not cool!");
                            
                        }
                       
                        break;
                        
                    case 4:
                        try
                        {
                            Console.WriteLine("Enter a new Time spent: ");
                            int newTimeSpent = int.Parse(Console.ReadLine());
                            s.TimeSpent = newTimeSpent;
                            using (var LearningDiary = new LearningDiaryContext())
                            {
                                LearningDiary.Topics.Update(s);
                                LearningDiary.SaveChanges();

                            }
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Wrong format");
                        }

                        break;
                        
                    case 5:
                        Console.WriteLine("Enter a new Source: ");
                        string newSource = Console.ReadLine();
                        s.Source = newSource;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(s);
                            LearningDiary.SaveChanges();

                        }
                        break;
                        
                    case 6:
                        Console.WriteLine("Enter a new Start time: ");
                        try
                        {
                            
                            DateTime newStart = DateTime.Parse(Console.ReadLine());
                            s.StartLearningDate = newStart;
                            using (var LearningDiary = new LearningDiaryContext())
                            {
                                LearningDiary.Topics.Update(s);
                                LearningDiary.SaveChanges();

                            }
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Wrong format!");
                        }
                     
                        break;
                        
                    case 7:
                        try
                        {
                            Console.WriteLine("Enter a new value to in progress: ");
                            bool newInProg = bool.Parse(Console.ReadLine());
                            s.InProgres = newInProg;
                            using (var LearningDiary = new LearningDiaryContext())
                            {
                                LearningDiary.Topics.Update(s);
                                LearningDiary.SaveChanges();

                            }
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Wrong format");
                        }
                        
                        break;
                        
                    case 8:
                        Console.WriteLine("Enter a new completion date: ");
                        try
                        {
                            DateTime newEnd = DateTime.Parse(Console.ReadLine());
                            s.Completion = newEnd;
                            using (var LearningDiary = new LearningDiaryContext())
                            {
                                LearningDiary.Topics.Update(s);
                                LearningDiary.SaveChanges();

                            }
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Wrong format");
                        }
                       
                        break;


                }
                }

            if (inputti.KeyChar == '2')
            {
                diaryEntrys.Remove(search);
                diaryEntrysModels.Remove(search);
                using (var LearningDiary = new LearningDiaryContext())
                {
                    LearningDiary.Topics.Remove(s);
                    LearningDiary.SaveChanges();
                }
            }
            
                
        }
        //search with title
        public static void searchByTitle(string search)
        {
           
            IdTitlePairs.TryGetValue(search, out int s);

            


            Models.Topic t = new Models.Topic();
            diaryEntrysModels.TryGetValue(s, out t);

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Tässä haetun Titlen {0}, mukaiset tiedot", search);
            Console.WriteLine("ID = {0}\n" +
                "Title = {1}\n"
                + "Description = {2}\n"
                + "ETA to master = {3}\n"
                + "Time Spent = {4}\n"
                + "Source = {5}\n"
                + "Start time = {6}\n"
                + "In progress = {7}\n"

                , t.Id, t.Title, t.Description, t.TimeToMaster, t.TimeSpent
                , t.Source, t.StartLearningDate.ToString(), t.InProgres);

            if (t.InProgres == false)
                Console.WriteLine( "CompletionDate = " + t.Completion.ToString());

            //Edit or remove object after search
            Console.Write("Press 1 to edit topic and 2 to delete a topic: ");

            var inputti = Console.ReadKey();
            if (inputti.KeyChar == '1')
            {
                Console.Clear();
                Console.WriteLine("Enter a subject to change:");
                Console.WriteLine(
            "Title: 1\n"
            + "Description: 2\n"
            + "ETA to master: 3\n"
            + "Time Spent: 4\n"
            + "Source: 5\n"
            + "Start time: 6\n"
            + "In progress: 7\n"
            + "CompletionDate: 8");
                int subject = int.Parse(Console.ReadLine());

                

                switch (subject) { 
                //{
                //    case 0:

                //        Console.Write("Enter a new ID: ");
                //        int newID = int.Parse(Console.ReadLine());
                //        t.Id = newID;
                        
                //        using (var LearningDiary = new LearningDiaryContext())
                //        {
                //            LearningDiary.Topics.Update(t);
                //            LearningDiary.SaveChanges();

                //        }


                //        break;

                    case 1:
                        Console.WriteLine("Enter a new title: ");
                        string newTitle = Console.ReadLine();
                        t.Title = newTitle;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                        break;

                    case 2:
                        Console.WriteLine("Enter a new description: ");
                        string newDesc = Console.ReadLine();
                        t.Description = newDesc;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                        break;

                    case 3:
                        Console.WriteLine("Enter a eta to master: ");

                    try
                    {
                        int newETA = int.Parse(Console.ReadLine());
                        t.TimeToMaster = newETA;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("wrong format!");
                    }
                        
                        break;

                    case 4:
                        Console.WriteLine("Enter a new Time spent: ");

                    try
                    {
                        int newTimeSpent = int.Parse(Console.ReadLine());
                        t.TimeSpent = newTimeSpent; using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("Wrong format!");
                    } 
                       
                        break;

                    case 5:
                        Console.WriteLine("Enter a new Source: ");

                    try
                    {
                        string newSource = Console.ReadLine();
                        t.Source = newSource; using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("Wrong format!");
                    }
                        
                        break;

                    case 6:
                        Console.WriteLine("Enter a new Start time: ");

                    try
                    {
                        DateTime newStart = DateTime.Parse(Console.ReadLine());
                        t.StartLearningDate = newStart; using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("Wrong Format!");
                    }
                        
                        break;

                    case 7:
                        Console.WriteLine("Enter a new value to in progress: ");
                    try
                    {
                        bool newInProg = bool.Parse(Console.ReadLine());
                        t.InProgres = newInProg;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("Wrong format!");
                    }
                        
                        break;

                    case 8:
                        Console.WriteLine("Enter a new completion date: ");
                    try
                    {
                        DateTime newEnd = DateTime.Parse(Console.ReadLine());
                        t.Completion = newEnd;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                    }
                    catch (Exception)
                    {

                        Console.WriteLine("Wrong format!");
                    }
                        
                        break;


                }
            }

            if (inputti.KeyChar == '2')
            {
                diaryEntrys.Remove(s);
                diaryEntrysModels.Remove(s);
                using (var LearningDiary = new LearningDiaryContext())
                {
                    LearningDiary.Topics.Remove(t);
                    LearningDiary.SaveChanges();
                }
            }



            Console.ReadLine();

        }

      
      public static async Task TellAJoke()
        {
            Console.WriteLine("---------------------------\n");
            Joke randomJoke = await GetJoke();
            if (randomJoke.type == "single")
            {
                Console.WriteLine(randomJoke.joke);
            }
            else
            {
                Console.WriteLine(randomJoke.setup);
                System.Threading.Thread.Sleep(3000);
                Console.WriteLine(randomJoke.delivery);
            }
            Console.Read();
            Console.Clear();
        }
        public async static Task<Joke> GetJoke()
        {
            const string url = "https://v2.jokeapi.dev/joke/Any?blacklistFlags=nsfw,racist,sexist";
            Joke randomJoke;

            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(url);
                randomJoke = JsonConvert.DeserializeObject<Joke>(json);
            }

            return randomJoke;


        }


        }
    }
    

