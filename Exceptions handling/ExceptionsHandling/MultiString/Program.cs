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
			while (true)
			{
				Console.WriteLine("Меню:\n" +
								  "1: Вывести первые символы введенных строк;\n" +
								  "2: Конвертировать строку в число\n" +
								  "Any key: Выход из приложения");
				var input = Console.ReadLine();
				if (input == "1")
					InputWithEmptyStrings();
				else if (input == "2")
					ConvertString();
				else
					break;

				Console.WriteLine("Нажмите любую клавишу для возврата в меню");
				Console.ReadKey();
				Console.Clear();
			}
		}

		static void InputWithEmptyStrings()
		{
			Console.WriteLine("Введите несколько строк. Для окончании ввода наберите \"ok\"");
			List<String> _strings = new List<string>();

			while (true)
			{
				var s = Console.ReadLine().Trim();
				if (s == "ok") break; else _strings.Add(s);
			}
			if(-_strings.Count > 0)
				Console.WriteLine("\nПервые символы введенных строк:");

			try
			{
				foreach (var str in _strings)
				{
					if (!String.IsNullOrEmpty(str))
						Console.WriteLine(str[0]);
					else
						Console.WriteLine("Была введена пустая строка");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			
		}

		static void ConvertString()
		{
			Console.WriteLine("Ведите число для преобразования");

			try
			{
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
		}
	}
}
