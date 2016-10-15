using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

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

		public static bool Save(string address)
		{
			using (var client = new HttpClient())
			{
				var contentType = client.GetAsync(address).Result.Content.Headers.ContentType;
				var type = contentType.MediaType.Substring(0, contentType.MediaType.IndexOf('/'));
				try
				{
					switch (type)
					{
						case "text":
							var textContent = client.GetStringAsync(address).Result;
							var encoding = String.IsNullOrEmpty(contentType.CharSet) ? Encoding.UTF8 : Encoding.GetEncoding(contentType.CharSet);
							return SaveWebPage(address, textContent, ExtensionsManager.GetExtensionForMime(contentType.MediaType), encoding);
						case "image":
							var image = client.GetByteArrayAsync(address).Result;
							var ext = ExtensionsManager.GetExtensionForMime(contentType.MediaType);
							if (Extensions.Contains(ext))
								return SaveFile(address, image, ext);
							break;
					}
					return false;
				}
				catch
				{
					return false;
				}
				
			}
		}

		private static bool SaveWebPage(string address, string content, string ext, Encoding encoding)
		{
			var dir = DirectoryCreator.CreateDirectoryFromUri(_root, address);
			if (String.IsNullOrEmpty(dir)) return false;

			var fileName = UrlHelper.GetFileNameFromUri(address);

			if (String.IsNullOrEmpty(fileName) || fileName.GetExtension() != ext) fileName = "index." + ext;

			try
			{
				if(!String.IsNullOrEmpty(content))
				using (StreamWriter fileWriter = new StreamWriter(Path.Combine(dir, fileName), false, encoding))
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

		private static bool SaveFile(string address, byte[] file, string ext)
		{
			var dir = DirectoryCreator.CreateDirectoryFromUri(_root, address);
			if (String.IsNullOrEmpty(dir)) return false;

			var fileName = UrlHelper.GetFileNameFromUri(address);
			if (String.IsNullOrEmpty(fileName))
				fileName = "file." + ext;

			try
			{
				using (FileStream stream = new FileStream(Path.Combine(dir, fileName), FileMode.Create))
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
	}
}
