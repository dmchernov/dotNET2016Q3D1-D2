using System.Collections.Generic;

namespace WebCrowler
{
	public static class Pages
	{
		private static List<string> LoadedPages = new List<string>();

		public static bool CanBeLoad(string link)
		{
			if (LoadedPages.Contains(link)) return false;

			LoadedPages.Add(link);
			return true;
		}

		public static void AddRootPage(string root)
		{
			if (!LoadedPages.Contains(root))
				LoadedPages.Add(root);
		}
	}
}
