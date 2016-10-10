using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace WebCrowler
{
	static class Saver
	{
		private static string _root;
		public static List<string> Extensions = new List<string>();
		public static void SetRoot(string path)
		{
			_root = path;
		}

		public static bool SavePage(string address, string content)
		{
			var path = CreateDirectory(address);
			if (String.IsNullOrEmpty(path)) return false;
			try
			{
				if(!String.IsNullOrEmpty(content))
				using (StreamWriter fileWriter = new StreamWriter(Path.Combine(path, "index.html")))
				{
					fileWriter.Write(content);
					return true;
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			
		}

		public static bool SaveFile(string address)
		{
			var client = new HttpClient();
			var file = client.GetByteArrayAsync(address).Result;
			var name = address.Substring(address.LastIndexOf('/') + 1);
			if (String.IsNullOrEmpty(name.GetExtension()) || !Extensions.Contains(name.GetExtension())) return false;
			var dir = CreateDirectory(address.Substring(0, address.LastIndexOf('/')));
			try
			{
				using (FileStream stream = new FileStream(Path.Combine(dir, name), FileMode.Create))
				{
					stream.Write(file, 0, file.Length);
					return true;
				}
			}
			catch
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
					Pages.AddPage(address);
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
