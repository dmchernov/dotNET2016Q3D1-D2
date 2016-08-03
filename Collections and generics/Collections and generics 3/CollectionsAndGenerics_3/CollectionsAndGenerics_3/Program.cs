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
			MyCollection mc = new MyCollection(new int[] {1,2,3,4,5,6,7,8,9,-4,8,4,7,-5});

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
		private int[] _digits;

		public MyCollection(int[] digits)
		{
			_digits = digits;
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
