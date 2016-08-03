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
			Random rnd = new Random();

			for (int i = 0; i < 100; i++)
			{
				mc[i] = rnd.Next(-100000000, Int32.MaxValue);
			}

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

	class MyCollection : IEnumerable<int>
	{
		private int[] _digits = new int[100];

		public int this[int i]
		{
			get { return _digits[i]; }
			set { _digits[i] = value; }
		}
		public IEnumerator<int> GetEnumerator()
		{
			for (int i = 0; i < _digits.Length; i++)
			{
				yield return _digits[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _digits.GetEnumerator();
		}
	}
}
