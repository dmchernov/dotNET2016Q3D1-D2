//Напишите атрибут, который можно применить только к классу.
//Атрибут содержит информацию об авторе кода(имя, e-mail).
//Напишите функционал, который позволит вывести эту информацию в консоль.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassAttribute
{
	class Program
	{
		static void Main(string[] args)
		{
			System.Type myType = typeof(MyClass);

			var attr = myType.GetCustomAttributes();

			foreach (var at in attr)
			{
				if (at is AuthorAttribute)
					Console.WriteLine($"{myType.ToString()} Author is {((AuthorAttribute)at).Name}, e-mail: {((AuthorAttribute)at).EMail}");
			}

			Console.ReadKey();
		}
	}

	[System.AttributeUsage(System.AttributeTargets.Class)]
	class AuthorAttribute : Attribute
	{
		public string Name { get; set; }
		public string EMail { get; set; }

		public AuthorAttribute(String name, String eMail)
		{
			Name = name;
			EMail = eMail;
		}
	}
	[Author("Dmitrii Chernov", "dmitrii_chernov1@epam.com")]
	class MyClass
	{
		//Do something
	}
}
