using System.Collections.Generic;

namespace WebCrowler
{
	public static class LinkRepository
	{
		private static readonly List<string> LoadedLinks = new List<string>();

		public static bool CanBeLoad(string link)
		{
			return !LoadedLinks.Contains(UrlHelper.RemoveProtocol(link));
		}

		public static void AddLink(string address)
		{
			address = UrlHelper.RemoveProtocol(address);

			if (CanBeLoad(address))
			{
				LoadedLinks.Add(address);
			}
		}
	}
}
