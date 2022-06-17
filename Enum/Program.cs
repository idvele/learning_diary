using System;

namespace Enum
{
    class Program
    {
		
			enum WeekDays
			{
				Monday,
				Tuesday,
				Wednesday,
				Thursday,
				Friday,
				Saturday,
				Sunday
			}

			public static void Main()
			{
				Console.WriteLine(WeekDays.Friday);
				int day = (int)WeekDays.Friday;
				Console.WriteLine(day);

				var wd = (WeekDays)5;
				Console.WriteLine(wd);

			}
		
	}
}
