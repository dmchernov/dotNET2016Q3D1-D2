using System;

namespace WebCrowler
{
	public static class Extensions
	{
		public static string GetExtension(this string address)
		{
			var index = address.LastIndexOf('.');
			if (index != -1)
				return address.Substring(index + 1);
			return String.Empty;
		}
	}
}
