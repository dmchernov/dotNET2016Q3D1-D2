using System;
using WebCrowler;

namespace WebCrowlerUser
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Введите адрес ресурса (по умолчанию http://www.epam.com)");
			var address = Console.ReadLine();

			Console.WriteLine("Введите количество уровней (по умолчанию 3)");
			int level;
			if (!Int32.TryParse(Console.ReadLine(), out level))
				level = 3;

			Console.WriteLine("Искать только в текущем домене? (true, false - по-умолчанию true)");
			var domen = Console.ReadLine() != "false";

			//Console.WriteLine("Допустимые расширения (через запятую без пробелов)");
			//var input = Console.ReadLine();
			//var ext = new List<string>();
			//if (String.IsNullOrEmpty(input))
			//	ext = new List<string> {"html", "htm"};
			//else
			//	ext = input.Split(',').ToList();

			var c = new Crowler(level, domen) {ContentLoaded = ContentLoaded};
			c.Run(String.IsNullOrEmpty(address) ? @"https://www.epam.com" : address);

			Console.WriteLine("\n\nPress any key to continue...");
			Console.ReadLine();
		}

		private static void ContentLoaded(object sender, PageEventArgs eventArgs)
		{
			Console.WriteLine(eventArgs.Address);
		}
	}
}
