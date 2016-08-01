//Реализуйте обобщенную коллекцию с функциональностью сравнения ее элементов на равенство с переданными объектом. Объект может являться любым типом данных. Обоснуйте свой выбор.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsAndGenerics_2
{
	class Program
	{
		static void Main(string[] args)
		{
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
