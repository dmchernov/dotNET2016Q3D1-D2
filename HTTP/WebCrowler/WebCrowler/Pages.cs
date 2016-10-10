using System.Collections.Generic;

namespace WebCrowler
{
	public static class Pages
	{
		private static List<string> LoadedPages = new List<string>();

		public static bool CanBeLoad(string link)
		{
			if (LoadedPages.Contains(link)) return false;

			return true;
		}

		public static void AddPage(string address)
		{
			if (!LoadedPages.Contains(address))
				LoadedPages.Add(address);
		}
	}
}
