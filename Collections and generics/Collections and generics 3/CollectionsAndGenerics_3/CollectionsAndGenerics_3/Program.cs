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

			mc[0] = 1;
			mc[1] = 2;
			mc[2] = 8;
			mc[3] = -1;

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
		private List<int> _digits = new List<int>();

		public MyCollection()
		{
			_digits.Add(0);
		}

		public int this[int i]
		{
			get { return _digits[i]; }
			set
			{
				var index = i;
				while (index >= _digits.Count)
				{
					_digits.Add(0);
				}
				_digits[i] = value;
			}
		}
		public IEnumerator<int> GetEnumerator()
		{
			return _digits.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _digits.GetEnumerator();
		}
	}
}
