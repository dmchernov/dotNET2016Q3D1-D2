//Напишите метод для конкатенации каждого второго элемента массива строк в результирующую строку. Обоснуйте выбор реализации. Использование Linq запрещено.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringConcat
{
	static class Program
	{
		static void Main(string[] args)
		{
			var strings = new String[] {"one", "two", "three", "for", "five", "six", "seven"};

			Console.WriteLine(strings.CustomConcat());

			Console.ReadKey();
		}

		public static String CustomConcat(this String[] strings)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 1; i < strings.Length; i = i + 2)
			{
				sb.Append(strings[i]); //Для конкатенации выбран класс StringBuilder с целью минимизировать издержки памяти
				sb.Append(' ');		   //и увеличить быстродействие кода.
			}
			return sb.ToString();
		}
	}
}
