using System;

namespace WebCrowler
{
	public static class Extensions
	{
		public static string GetExtension(this string address)
		{
			if (address.StartsWith("http://")) address = address.Remove(0, 7);
			if (address.StartsWith("https://")) address = address.Remove(0, 8);
			var parts = address.Split('/');
			if (parts.Length < 2) return String.Empty;

			var index = parts[parts.Length - 1].LastIndexOf('.');
			if (index != -1)
				return address.Substring(index + 1);
			return String.Empty;
		}
	}
}
