//Вам необходимо  описать структуру, которая представляет собой товар в интернет-магазине.Товар имеет наименование, цену и количество едениц на складе.
//
//В дальнейшем вы будете работать со списком товара и осуществлять различные выборки с помощью LINQ to Object.
//
//Спроектируйте и опишите структуру правильно с точки зрения реализации ValueType.
using System;

namespace Types2
{
	struct Item
	{
		public Int32 Id { get; set; }
		public String Name { get; set; }
		public Decimal Price { get; set; }
		public UInt32 Count { get; set; }

		public override Boolean Equals(object obj)
		{
			if (obj is Item)
			{
				return this.Id == ((Item)obj).Id;
			}
			return false;
		}

		public override Int32 GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}