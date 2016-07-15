//Попытайтесь разорвать связь между классами через InfoData(класс Destination ничего не должен знать об этом типе)
//
//Оптимизируйте работу с точки зрения быстродействия.
using System;

namespace Types1
{
	struct InfoData
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	class Source
	{
		internal void CheckAndProceed(List<InfoData> data)
		{
			var dest = new Destination();
			//do something

			dest.ProceedData(data);
		}
	}

	class Destination
	{
		internal void ProceedData<T>(List<T> data) where T : struct
		{
			foreach (var item in data)
			{
				//do something
			}
		}
	}
}