//Попытайтесь разорвать связь между классами через InfoData(класс Destination ничего не должен знать об этом типе)
//
//Оптимизируйте работу с точки зрения быстродействия.
using System;

namespace Types1
{
	interface IInfoData
	{
		String FirstName { get; set; }
		String LastName { get; set; }
	}

	struct InfoData : IInfoData
	{
		public Int32 Id { get; set; }
		public String FirstName { get; set; }
		public String LastName { get; set; }

		public override Int32 GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override Boolean Equals(object obj)
		{
			if (obj is InfoData)
			{
				return this.Id == ((InfoData)obj).Id;
			}
			return false;
		}
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
		internal void ProceedData<T>(List<T> data) where T : IInfoData
		{
			foreach (var item in data)
			{
				//do something
			}
		}
	}
}