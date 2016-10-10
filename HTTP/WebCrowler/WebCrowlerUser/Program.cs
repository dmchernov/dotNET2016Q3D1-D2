using System;
using System.IO;
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

			Console.WriteLine("Ведите путь для сохранения (по умолчанию папка приложения)");
			var path = Console.ReadLine();
			try
			{
				Directory.CreateDirectory(path);
			}
			catch (Exception)
			{
				Console.WriteLine("Неправильный путь. Будет использован путь по умолчанию");
				path = Directory.GetCurrentDirectory();
			}
			

			var c = new Crowler(level, domen, path) {ContentLoaded = ContentLoaded, PageNotLoaded = PageNotLoaded};
			c.Run(String.IsNullOrEmpty(address) ? @"https://www.epam.com" : address);

			Console.WriteLine("\n\nPress any key to continue...");
			Console.ReadLine();
		}

		private static void PageNotLoaded(object sender, PageEventArgs pageEventArgs)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Page \"{pageEventArgs.Address}\" not loaded");
		}

		private static void ContentLoaded(object sender, PageEventArgs eventArgs)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Page \"{eventArgs.Address}\" was loaded");
		}
	}
}
