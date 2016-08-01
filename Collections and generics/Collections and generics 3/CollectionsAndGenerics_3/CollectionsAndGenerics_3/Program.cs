//Реализуйте свою коллекцию с итератором. Коллекция работает только с целыми числами. При итерации проход по элементам должен заканчиваться, если встречается отрицательное число.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsAndGenerics_3
{
	class Program
	{
		static void Main(string[] args)
		{
			MyCollection mc = new MyCollection();

			foreach (int digit in mc)
			{
				if (digit > 0)
					Console.WriteLine(digit);
				else
					break;
			}

			Console.ReadKey();
		}
	}

	class MyCollection : IEnumerable
	{
		private int[] _digits = new int[100];
		private Random rnd = new Random();

		public MyCollection()
		{
			for (var i = 0; i < _digits.Length; i++)
			{
				_digits[i] = rnd.Next(Int32.MinValue, Int32.MaxValue);
			}
		}

		public IEnumerator GetEnumerator()
		{
			for (int i = 0; i < _digits.Length; i++)
			{
				yield return _digits[i];
			}
		}
	}
}
