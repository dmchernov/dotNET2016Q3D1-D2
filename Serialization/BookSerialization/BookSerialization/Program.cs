using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BookSerialization
{
	class Program
	{
		private static Catalog _catalog = new Catalog() {Date = DateTime.Today};
		static void Main (string[] args)
		{
			// Чтение и десериализация исходного файла
			Read();

			// Вывод в консоль данных по книгам из исходного xml файла
			ShowCatalog(_catalog);

			//var c = new Catalog() {Date = DateTime.Today};
			//c.Books = new List<Book>();
			//c.Books.Add(new Book() { Author = "author1", Title = "title1", Description = "desc1", Id = "bk001", Isbn = "isbn1", Publisher = "publ1", PublishDate = DateTime.Today });
			//c.Books.Add(new Book() { Author = "author2", Title = "title2", Description = "desc2", Id = "bk002", Isbn = "isbn2", Publisher = "publ2", PublishDate = DateTime.Today });
			//c.Books.Add(new Book() { Author = "author3", Title = "title3", Description = "desc3", Id = "bk003", Isbn = "isbn3", Publisher = "publ3", PublishDate = DateTime.Today });

			// Сериализация прочитанных данных в новый файл
			Write(_catalog);

			Console.ReadKey();
		}

		private static void Read()
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Catalog));
				FileStream fs = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "Files/books.xml"), FileMode.Open);

				_catalog = (Catalog)serializer.Deserialize(fs);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		private static void Write(Catalog catalog)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Catalog));
			FileStream fs = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), $"Files/new_books.xml"), FileMode.Create);

			serializer.Serialize(fs, catalog);
		}

		private static void ShowCatalog (Catalog catalog)
		{
			foreach (var book in catalog.Books)
			{
				Console.WriteLine($"Title: {book.Title}");
				Console.WriteLine($"Genre: {book.Genre}");
				Console.WriteLine($"Author: {book.Author}");
				Console.WriteLine($"Description: {book.Description}");
				Console.WriteLine($"Publish Date: {book.PublishDate.ToString("d")}");
				Console.WriteLine("\n\n");
			}
		}
	}
}
