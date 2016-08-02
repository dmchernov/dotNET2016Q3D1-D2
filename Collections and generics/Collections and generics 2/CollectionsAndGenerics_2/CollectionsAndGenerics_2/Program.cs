//Реализуйте обобщенную коллекцию с функциональностью сравнения ее элементов на равенство с переданными объектом. Объект может являться любым типом данных. Обоснуйте свой выбор.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsAndGenerics_2
{
	class Program
	{
		static void Main(string[] args)
		{
			int[] arr = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
			MyCollection<int> myCol = new MyCollection<int>(arr);
			Console.WriteLine(myCol.Equals(8));

			string[] strArr = new[] {"Ivan", "ivan", "Petr", "Sergey"};
			MyCollection<string> myColStr = new MyCollection<string>(strArr);
			Console.WriteLine(myColStr.Equals("Petr"));

			Point[] pointArr = new[] {new Point(1, 2), new Point(4, 5),};
			MyCollection<Point> myColPoint = new MyCollection<Point>(pointArr);
			Console.WriteLine(myColPoint.Equals(new Point(4,5)));

			Console.ReadKey();
		}
	}

	class MyCollection<T>
	{
		private IList<T> _collection;

		public MyCollection(IList<T> collection)
		{
			_collection = collection;
		}

		public new int Equals(object o)
		{
			foreach (var item in _collection)
			{
				if (item.Equals(o)) return _collection.IndexOf(item);
			}
			return -1;
		}
	}
}
