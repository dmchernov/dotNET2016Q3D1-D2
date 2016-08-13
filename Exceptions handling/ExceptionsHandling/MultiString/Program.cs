//Напишите консольное приложение, которое выводит на экран первый из введенных символов каждой строки ввода.
//Опишите корректное поведение приложения, если пользователь ввел пустую строку.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StringToDigit;

namespace MultiString
{
	class Program
	{
		static void Main(string[] args)
		{
			InputWithEmptyStringsAndExceptionHandling();
			InputWithoutEmptyStrings();
			InputWithEmptyStrings();

			//StringToDigit Call
			try
			{
				Console.WriteLine("Ведите число для преобразования");
				var digit = StringConvert.ToDigit(Console.ReadLine());
				Console.WriteLine($"Строка успешно преобразована в число {digit}");
			}
			catch (InvalidOperationException ex)
			{
				Console.WriteLine(ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine($"Ошибка возникла в сборке {ex.Source}, метод {ex.TargetSite}");
			}

			Console.ReadKey(true);
		}

		//Этот метод позволяет добавлять в коллекцию пустые строки (пробелы отбрасываются) и использует механизм обработки исключений
		static void InputWithEmptyStringsAndExceptionHandling()
		{
			Console.WriteLine("Этот метод позволяет добавлять в коллекцию пустые строки (пробелы отбрасываются) и использует механизм обработки исключений");
			List<String> strings = new List<string>();

			while (true)
			{
				var s = Console.ReadLine().Trim();
				if (s == "exit") break; else strings.Add(s);
			}

			Console.WriteLine("\nПервые символы введенных строк:");
			try
			{
				foreach (var str in strings)
				{
					Console.WriteLine(str[0]);
				}
			}
			catch (IndexOutOfRangeException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		//Этот метод пропускает пустые строки (пробелы отбрасываются)
		static void InputWithoutEmptyStrings()
		{
			Console.WriteLine("Этот метод пропускает пустые строки (пробелы отбрасываются)");

			List<String> strings = new List<string>();

			while (true)
			{
				var s = Console.ReadLine().Trim();
				if (s == "exit") break;
				else if(s == String.Empty) continue;
				else strings.Add(s);
			}

			Console.WriteLine("\nПервые символы введенных строк:");
			foreach (var str in strings)
				{
					Console.WriteLine(str[0]);
				}
		}

		//Этот метод позволяет добавлять в коллекцию пустые строки (пробелы отбрасываются) и проверяет строки на корректность при итерации
		static void InputWithEmptyStrings()
		{
			Console.WriteLine("Этот метод позволяет добавлять в коллекцию пустые строки (пробелы отбрасываются) и проверяет строки на корректность при итерации");
			List<String> _strings = new List<string>();

			while (true)
			{
				var s = Console.ReadLine().Trim();
				if (s == "exit") break; else _strings.Add(s);
			}

			Console.WriteLine("\nПервые символы введенных строк:");
			foreach (var str in _strings)
				{
					if(!String.IsNullOrEmpty(str))
						Console.WriteLine(str[0]);
				}
		}
	}
}
