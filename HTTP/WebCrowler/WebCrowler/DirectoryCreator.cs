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
				address = UrlHelper.RemoveProtocol(address);

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
