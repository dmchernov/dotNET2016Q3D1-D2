using System;

namespace Fibonachi
{
	class Program
	{
		static FibonachiLine _result;
		static void Main(string[] args)
		{
			ShowRedisCalc();
			ShowMemoryCalc();
			Console.ReadKey();
		}

		private static void ShowMemoryCalc()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("Использование Memory Cache:");
			var calc = new Calculator<FibonachiLine>(new FibonachiMemoryCache());

			_result = calc.Calculate(new FibonachiLine(10));

			Console.WriteLine("Первые 10 чисел Фибоначчи:");
			foreach (var i in _result.Line)
			{
				Console.Write($"{i} ");
			}
			Console.WriteLine("\n--------------------");

			_result = calc.Calculate(new FibonachiLine(40));

			Console.WriteLine("Первые 40 чисел Фибоначчи:");
			foreach (var i in _result.Line)
			{
				Console.Write($"{i} ");
			}
			Console.WriteLine("\n--------------------");

			_result = calc.Calculate(new FibonachiLine(20));
			Console.WriteLine("Первые 20 чисел Фибоначчи:");

			foreach (var i in _result.Line)
			{
				Console.Write($"{i} ");
			}
			Console.WriteLine("\n--------------------");
		}

		private static void ShowRedisCalc()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			var calc = new Calculator<FibonachiLine>(new FibonachiRedisCache("localhost"));

			Console.WriteLine("Использование Redis кэша:");

			for (int a = 10; a <= 50; a += 10)
			{
				Console.WriteLine($"Первые {a} чисел Фибоначчи:");
				_result = calc.Calculate(new FibonachiLine(a));

				Console.WriteLine("Конечный результат:");
				foreach (var i in _result.Line)
				{
					Console.Write($"{i} ");
				}
				Console.WriteLine("\n");
			}
		}
	}
}
