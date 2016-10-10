using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebCrowler
{
	static class Saver
	{
		private static readonly string _root = Directory.GetCurrentDirectory();
		//public static List<string> Extensions = new List<string>();

		public static bool Save(string address, string content)
		{
			//var ext = address.GetExtension();
			//if (!String.IsNullOrEmpty(ext))
			//	if (!Extensions.Contains(ext))
			//		return false;

			//var client = new HttpClient();
			//var responce = client.GetAsync(address);

			//if (responce.Result.StatusCode != HttpStatusCode.OK) return false;
			//var type = responce.Result.Content.Headers.ContentType.MediaType;

			//Console.WriteLine(type);

			//string body = String.Empty;
			//byte[] file = null;

			//try
			//{
			//	if (type == "text/html")
			//	{
			//		body = client.GetStringAsync(address).Result;
			//	}
			//	//else
			//	//if (type == "application/pdf")
			//	//	file = client.GetByteArrayAsync(address).Result;
			//}
			//catch (Exception)
			//{
			//	return false;
			//}
			

			address = CreateDirectory(address);
			if (String.IsNullOrEmpty(address)) return false;
			try
			{
				if(!String.IsNullOrEmpty(content))
				using (StreamWriter fileWriter = new StreamWriter(Path.Combine(address, "index.html")))
				{
					fileWriter.Write(content);
					return true;
				}
				//if(file != null)
				//	using (FileStream stream = new FileStream(Path.Combine(address, "file.pdf"), FileMode.Create))
				//	{
				//		stream.Write(file,0, file.Length);
				//	}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			
		}

		private static string CreateDirectory(string address)
		{
			try
			{
				if (address.Contains('?'))
				{
					address = address.Substring(0, address.IndexOf('?'));
					if (!Pages.CanBeLoad(address)) return String.Empty;
				}

				if (address.StartsWith("http://")) address = address.Remove(0, 7);
				if (address.StartsWith("https://")) address = address.Remove(0, 8);

				var path = Path.Combine(_root, address);

				Directory.CreateDirectory(path);
				return path;
			}
			catch (Exception)
			{
				return String.Empty;
			}
			
		}
	}
}
