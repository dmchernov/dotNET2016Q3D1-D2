using System;
using System.Diagnostics;

namespace Fibonachi
{
	class Program
	{
		static FibonachiLine _result;
		static void Main(string[] args)
		{
			while (true)
			{
				Console.Clear();
				Console.WriteLine("1 - тестирование Memory Cache");
				Console.WriteLine("2 - тестирование Redis Cache");
				Console.WriteLine("3 - выход");

				int input;
				if (!Int32.TryParse(Console.ReadLine(), out input)) continue;

				switch (input)
				{
					case 1:
						ShowMemoryCalc();
						break;
					case 2:
						ShowRedisCalc();
						break;
					case 3:
						return;
				}
			}
		}

		private static void ShowMemoryCalc()
		{
			Console.WriteLine("\nТестирование Memmory Cache");

			var calc = new Calculator<FibonachiLine>(new FibonachiMemoryCache());
			int input;

			while (true)
			{
				Console.WriteLine("\nВведите число для вычисления ряда Фибоначчи или 0 для выхода в основное меню");

				if (!Int32.TryParse(Console.ReadLine(), out input)) continue;

				if (input == 0) return;

				Console.WriteLine($"Первые {input} чисел Фибоначчи были вычислены за {Calc(calc, input)}");
			}

			
		}

		private static void ShowRedisCalc()
		{
			var calc = new Calculator<FibonachiLine>(new FibonachiRedisCache("localhost"));

			Console.WriteLine("\nТестирование Redis кэша");

			int input;

			while (true)
			{
				Console.WriteLine("\nВведите число для вычисления ряда Фибоначчи или 0 для выхода в основное меню");

				if (!Int32.TryParse(Console.ReadLine(), out input)) continue;

				if (input == 0) return;

				Console.WriteLine($"Первые {input} чисел Фибоначчи были вычислены за {Calc(calc, input)}");
			}
		}

		private static TimeSpan Calc(Calculator<FibonachiLine> calc, int count)
		{
			var sw = new Stopwatch();
			sw.Start();
			_result = calc.Calculate(new FibonachiLine(count));
			sw.Stop();
			return sw.Elapsed;
		}
	}
}
