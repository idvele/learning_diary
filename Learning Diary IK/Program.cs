using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Learning_Diary_IK.Models;


namespace Learning_Diary_IK
{
    class Program
    {
        public static int GlobalID = 1;
        public static Dictionary<int, Topic> diaryEntrys = new Dictionary<int, Topic>();
        public static Dictionary<int, Models.Topic> diaryEntrysModels = new Dictionary<int, Models.Topic>();
        public static Dictionary<string, int> IdTitlePairs = new Dictionary<string, int>();

        static void Main(string[] args)
        {
            //Tähän tulee databasen lataus
            using (var LearningDiary = new LearningDiaryContext())
            {
                var taulu = LearningDiary.Topics.Max(topikki => topikki.Id);


                GlobalID = taulu;
                GlobalID++;

                //Kaikki tietokannassa olevat objektit lisätään diaryEntrysModels-dictionaryyn
                var kaikki = LearningDiary.Topics;

                foreach (var topic in kaikki)
                {
                    diaryEntrysModels.Add(topic.Id, topic);
                    IdTitlePairs.Add(topic.Title, topic.Id);
                    
                }
            }

            //DiaryEntrysModels-dictionary siirretään DiaryEntrys-dictionaryyn

                


                Console.WriteLine("----------------------------------------");
            Console.WriteLine("`^*Welcome to Ilari's learning diary^*`´");
            Console.WriteLine("----------------------------------------");

            string input;

            
            do
            {



                Console.WriteLine("Press 1 to add an item to diary\nPress 2 to print whole diary\nPress 3 to search subject via ID or title\nPress 4 to save and exit");


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



                            foreach (int i in diaryEntrysModels.Keys)
                            {

                                Models.Topic haku = null;

                                diaryEntrysModels.TryGetValue(i, out haku);

                                Console.WriteLine("---------------------------------------------");

                                Console.WriteLine("ID = {0}\n" +
                                    "Title = {1}\n"
                                    + "Description = {2}\n"
                                    + "ETA to master = {3}\n"
                                    + "Time Spent = {4}\n"
                                    + "Source = {5}\n"
                                    + "Start time = {6}\n"
                                    + "In progress = {7}\n"

                                    , haku.Id, haku.Title, haku.Description, haku.TimeToMaster, haku.TimeSpent
                                    , haku.Source, haku.StartLearningDate.ToString(), haku.InProgres);

                                if (haku.InProgres == false)
                                    Console.WriteLine("CompletionDate = " + haku.Completion.ToString());



                                //writeToFile(haku.Id, haku.Title, haku.Description, haku.TimeToMaster, haku.TimeSpent
                                  //  , haku.Source, haku.StartLearningDate, haku.InProgres, haku.Completion);



                            }

                                foreach (int s in diaryEntrys.Keys)
                            {

                                 Topic haku = null;

                                diaryEntrys.TryGetValue(s, out haku);

                                Console.WriteLine("---------------------------------------------");
                                
                                Console.WriteLine("ID = {0}\n" +
                                    "Title = {1}\n"
                                    + "Description = {2}\n"
                                    + "ETA to master = {3}\n"
                                    + "Time Spent = {4}\n"
                                    + "Source = {5}\n"
                                    + "Start time = {6}\n"
                                    + "In progress = {7}\n"

                                    , haku.Id, haku.Title, haku.Description, haku.EstimatedTimeToMaster, haku.TimeSpent
                                    , haku.Source, haku.StartLearningDate.ToShortDateString(), haku.inProgress);

                                if (haku.inProgress == false)
                                    Console.WriteLine("CompletionDate = " + haku.CompletionDate.ToShortDateString());



                                writeToFile(haku.Id, haku.Title, haku.Description, haku.EstimatedTimeToMaster, haku.TimeSpent
                                    , haku.Source, haku.StartLearningDate, haku.inProgress, haku.CompletionDate);

                                

                            }

                            Console.ReadKey();
                            break;
                        
                            
                            //Search by Id or by Title
                        case "3":

                            //tallenna koko roska databaseen ja hae sieltä
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
                                    s.Source = y.Source;
                                    s.StartLearningDate = y.StartLearningDate;
                                    s.InProgres = y.inProgress;
                                    s.Completion = y.CompletionDate;



                                    //uusi lisäys
                                    LearningDiary.Topics.Add(s);

                                }
                                //tee tallennus databaseen
                                LearningDiary.SaveChanges();

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


            
            string answer = Console.ReadLine();
            
            DateTime date1 = new DateTime(2222,2,2);

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
            }

            else
                mytopic.inProgress = false;


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

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Tässä haetun Id:n {0}, mukaiset tiedot", search);
            Console.WriteLine("ID = {0}\n" +
                "Title = {1}\n"
                + "Description = {2}\n"
                + "ETA to master = {3}\n"
                + "Time Spent = {4}\n"
                + "Source = {5}\n"
                + "Start time = {6}\n"
                + "In progress = {7}\n"

                , s.Id, s.Title, s.Description, s.TimeToMaster, s.TimeSpent
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
                Console.WriteLine("ID: 0\n" +
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
                    case 0:
                        Console.Write("Enter a new ID: ");
                        int newID = int.Parse(Console.ReadLine());
                        s.Id = newID;

                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(s);
                            LearningDiary.SaveChanges();

                        }
                        break;

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
                        Console.WriteLine("Enter a eta tom master: ");
                        int newETA = int.Parse(Console.ReadLine());
                        s.TimeToMaster = newETA;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(s);
                            LearningDiary.SaveChanges();

                        }
                        break;
                        
                    case 4:
                        Console.WriteLine("Enter a new Time spent: ");
                        int newTimeSpent = int.Parse(Console.ReadLine());
                        s.TimeSpent = newTimeSpent;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(s);
                            LearningDiary.SaveChanges();

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
                        DateTime newStart = DateTime.Parse(Console.ReadLine());
                        s.StartLearningDate = newStart;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(s);
                            LearningDiary.SaveChanges();

                        }
                        break;
                        
                    case 7:
                        Console.WriteLine("Enter a new value to in progress: ");
                        bool newInProg = bool.Parse(Console.ReadLine());
                        s.InProgres = newInProg;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(s);
                            LearningDiary.SaveChanges();

                        }
                        break;
                        
                    case 8:
                        Console.WriteLine("Enter a new completion date: ");
                        DateTime newEnd = DateTime.Parse(Console.ReadLine());
                        s.Completion = newEnd;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(s);
                            LearningDiary.SaveChanges();

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
        //search wit title
        public static void searchByTitle(string search)
        {
            //Titlestä ID:ksi käännös ei toimi databasen olioiden osalta
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
                Console.WriteLine("ID: 0\n" +
            "Title: 1\n"
            + "Description: 2\n"
            + "ETA to master: 3\n"
            + "Time Spent: 4\n"
            + "Source: 5\n"
            + "Start time: 6\n"
            + "In progress: 7\n"
            + "CompletionDate: 8");
                int subject = int.Parse(Console.ReadLine());

                //Lisää tähän päivitys serverille

                switch (subject)
                {
                    case 0:
                        Console.Write("Enter a new ID: ");
                        int newID = int.Parse(Console.ReadLine());
                        t.Id = newID;
                        
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }


                        break;

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
                        Console.WriteLine("Enter a eta tom master: ");
                        int newETA = int.Parse(Console.ReadLine());
                        t.TimeToMaster = newETA;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                        break;

                    case 4:
                        Console.WriteLine("Enter a new Time spent: ");
                        int newTimeSpent = int.Parse(Console.ReadLine());
                        t.TimeSpent = newTimeSpent; using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                        break;

                    case 5:
                        Console.WriteLine("Enter a new Source: ");
                        string newSource = Console.ReadLine();
                        t.Source = newSource; using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                        break;

                    case 6:
                        Console.WriteLine("Enter a new Start time: ");
                        DateTime newStart = DateTime.Parse(Console.ReadLine());
                        t.StartLearningDate = newStart; using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                        break;

                    case 7:
                        Console.WriteLine("Enter a new value to in progress: ");
                        bool newInProg = bool.Parse(Console.ReadLine());
                        t.InProgres = newInProg;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

                        }
                        break;

                    case 8:
                        Console.WriteLine("Enter a new completion date: ");
                        DateTime newEnd = DateTime.Parse(Console.ReadLine());
                        t.Completion = newEnd;
                        using (var LearningDiary = new LearningDiaryContext())
                        {
                            LearningDiary.Topics.Update(t);
                            LearningDiary.SaveChanges();

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

       public static void dataBaseToProgram()
        {

        }
      
        }
    }
    

