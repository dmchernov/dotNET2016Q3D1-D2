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
		static string _address;
		static int _level = 3;
		static bool domain = true;
		static List<string> _extensions = new List<string> { "png", "jpg", "jpeg", "gif", "svg", "ico" };
		static string _path;
		private static bool _useVerboseMode = true;

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

			if (args.Length > 0 && args[0] == "-?")
				ShowHelp();
			else if (args.Length > 0 && args.Length%2 == 0)
			{
				for (int i = 0; i < args.Length; i += 2)
				{
					switch (args[i])
					{
						case "-u":
							_address = args[i + 1];
							break;
						case "-d":
							domain = args[i + 1] != "false";
							break;
						case "-l":
							int param;
							_level = Int32.TryParse(args[i + 1], out param) ? param : _level;
							break;
						case "-e":
							_extensions = args[i + 1].Split(',').ToList();
							break;
						case "-p":
							_path = args[i + 1];
							break;
						case "-v":
							_useVerboseMode = args[i + 1] != "false";
							break;
					}
				}
			}
			
			if(!ValidateParameters())
			{
				Console.WriteLine("\nPress any key to exit...");
				Console.ReadKey();
				return;
			}
			
			Console.WriteLine("Приложение будет запущено со следующими параметрами:");
			
			Console.WriteLine($"Адрес: {_address}");
			Console.WriteLine($"Только страницы текущего домена: {domain}");
			Console.WriteLine($"Уровень вложенности: {_level}");

			StringBuilder extListBuilder = new StringBuilder();
			foreach (var extension in _extensions)
			{
				extListBuilder.Append(extension);
				extListBuilder.Append(" ");
			}

			Console.WriteLine($"Расширения: {extListBuilder}");
			Console.WriteLine($"Путь для сохранения: {_path}\n\n\n");

			Thread.Sleep(2000);

			var c = new Crowler(_level, domain, _path, _extensions);
			if (_useVerboseMode)
			{
				c.ContentSkipped = ContentSkipped;
				c.ContentLoaded = ContentLoaded;
				c.ContentNotLoaded = ContentNotLoaded;
			}
			c.Run(_address);

			Console.ResetColor();

			Console.WriteLine("\n\nPress any key to exit...");
			Console.ReadLine();
		}

		private static void ContentSkipped(object sender, PageEventArgs pageEventArgs)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"Content with address \"{pageEventArgs.Address}\" was skipped");
		}

		private static void ContentNotLoaded(object sender, PageEventArgs pageEventArgs)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Page \"{pageEventArgs.Address}\" not loaded");
		}

		private static void ContentLoaded(object sender, PageEventArgs eventArgs)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Content with address \"{eventArgs.Address}\" was loaded");
		}

		private static void ShowHelp()
		{
			Console.WriteLine("{0:10},{1}", "-u", "URL address (http://www.example.com)");
			Console.WriteLine("{0:10},{1}", "-d", "Use current domen only (true/false)");
			Console.WriteLine("{0:10},{1}", "-l", "Level count for dowmload web content");
			Console.WriteLine("{0:10},{1}", "-e", "File extensions for images");
			Console.WriteLine("{0:10},{1}", "-p", "Path for save web content");
		}

		private static bool ValidateParameters()
		{
			var result = true;
			if (!UrlHelper.ValidateUrl(_address))
			{
				Console.WriteLine("Указан неправильный адрес ресурса.");
				result = false;
			}
			try
			{
				Directory.CreateDirectory(_path);
			}
			catch (Exception)
			{
				_path = Directory.GetCurrentDirectory();
				Console.WriteLine("Не удалось создать указанную директорию.");
				result = false;
			}
			
			return result;
		}
	}
}
