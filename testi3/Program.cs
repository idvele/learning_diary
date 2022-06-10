using System;

namespace testi3
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt1 = new DateTime(2008, 5, 1);

            Console.WriteLine(dt1.ToShortDateString());
        }
    }
}
