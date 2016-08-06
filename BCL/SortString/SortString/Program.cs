//Напишите метод для сортировки массива строк в независимости от региональных стандартов пользователя. Использование Linq запрещено.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortString
{
	static class Program
	{
		static void Main(string[] args)
		{
			var strings = new String[] {"Serg", "Ivan", "Petr", "Alex", "John"};
			strings.CustomSort();

			foreach (var s in strings)
			{
				Console.WriteLine(s);
			}

			Console.ReadKey();
		}

		public static void CustomSort(this String[] strings)
		{


			for (int i = 0; i < strings.Length; i++)
			{
				for (int j = 0; j < strings.Length - i - 1; j++)
				{
					if (String.Compare(strings[j], strings[j + 1], StringComparison.OrdinalIgnoreCase) > 0)
					{
						var s = strings[j];
						strings[j] = strings[j + 1];
						strings[j + 1] = s;
					}
				}
			}
		}
	}
}
