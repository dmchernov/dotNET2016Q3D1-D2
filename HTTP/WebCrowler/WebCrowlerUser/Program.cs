using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using WebCrowler;

namespace WebCrowlerUser
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			// Для запуска необходимо испоьзовать аргументы командной строки, иначе произойдет запуск с параметрами по умолчанию
			// Формат:
			// http://www.example.com - адрес ресурса
			// true\false - загружать страницы только в пределах текущего домена
			// 3 - уровень рекурсии при обходе страниц
			// jpg,bmp,png - расширения допустимых графических файлов через запятую без пробела
			// d:\temp - локальный абсолютный путь сохранения контента
			// http://www.playground.ru false 3 jpg,png,bmp,ico,gif d:\temp\httpTemp
			string address;
			int level;
			bool domain;
			List<string> extensions;
			string path;
			try
			{
				address = args[0];
				domain = args[1] != "false";
				level = Int32.Parse(args[2]);
				extensions = args[3].Split(',').ToList();
				path = args[4];
				Directory.CreateDirectory(path);
			}
			catch
			{
				Console.WriteLine("Неправильный набор входных аргументов, приложение будет запущено с параметрами по умолчанию.");
				address = @"http://www.epam.com";
				level = 3;
				domain = true;
				extensions = new List<string> { "png", "jpg", "jpeg", "gif", "svg", "ico" };
				path = Directory.GetCurrentDirectory();
			}

			Console.WriteLine("Параметры:");
			Console.WriteLine($"Адрес: {address}");
			Console.WriteLine($"Только страницы текущего домена: {domain}");
			Console.WriteLine($"Уровень вложенности: {level}");

			StringBuilder extListBuilder = new StringBuilder();
			foreach (var extension in extensions)
			{
				extListBuilder.Append(extension);
				extListBuilder.Append(" ");
			}

			Console.WriteLine($"Расширения: {extListBuilder}");
			Console.WriteLine($"Путь для сохранения: {path}\n\n\n");

			Thread.Sleep(2000);

			var c = new Crowler(level, domain, path, extensions) {ContentLoaded = ContentLoaded, ContentNotLoaded = PageNotLoaded, ContentSkipped = ContentSkipped};
			c.Run(address);

			Console.ResetColor();

			Console.WriteLine("\n\nPress any key to continue...");
			Console.ReadLine();
		}

		private static void ContentSkipped(object sender, PageEventArgs pageEventArgs)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"Content with address \"{pageEventArgs.Address}\" was skipped");
		}

		private static void PageNotLoaded(object sender, PageEventArgs pageEventArgs)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Page \"{pageEventArgs.Address}\" not loaded");
		}

		private static void ContentLoaded(object sender, PageEventArgs eventArgs)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Content with address \"{eventArgs.Address}\" was loaded");
		}
	}
}
