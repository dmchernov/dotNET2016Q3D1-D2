using System;
using System.IO;
using System.Linq;

namespace WebCrowler
{
	public static class DirectoryCreator
	{
		public static string CreateDirectoryFromUri(string root, string address)
		{
			try
			{
				if (address.StartsWith("http://")) address = address.Remove(0, 7);
				if (address.StartsWith("https://")) address = address.Remove(0, 8);

				if (address.Contains('?'))
					address = address.Substring(0, address.IndexOf('?'));

				if (UrlHelper.IsFileUri(address) && address.Contains('/'))
					address = address.Substring(0, address.LastIndexOf('/'));

				var path = Directory.CreateDirectory(Path.Combine(root, address)).FullName;

				return path;
			}
			catch
			{
				return String.Empty;
			}
		}
	}
}
