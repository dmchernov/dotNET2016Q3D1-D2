using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebCrowler
{
	public static class UrlHelper
	{
		private const string Uri = @"(http|https){1}\://[a-zA-Z]+[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}";
		private const string Domain = @"[a-zA-Z]+[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}";

		private static readonly Regex UriRegEx;
		private static readonly Regex DomainRegEx;

		static UrlHelper()
		{
			UriRegEx = new Regex(Uri);
			DomainRegEx = new Regex(Domain);
		}

		public static bool ValidateUrl(string address)
		{
			if (UriRegEx.IsMatch(address))
				return true;
			return false;
		}

		public static string GetValidUri(string rootUri, string address)
		{
			if (UriRegEx.IsMatch(address))
				return address;

			while (address[0] == '/')
			{
				address = address.Substring(1);
			}

			if (DomainRegEx.IsMatch(address.Substring(0, address.Contains('/') ? address.IndexOf('/') : address.Length)))
			{
				return "http://" + address;
			}

			return rootUri + '/' + address;
		}

		public static bool IsCurrentDomainLink(string domain, string address)
		{
			var link = GetValidUri(domain, address);

			if (UriRegEx.IsMatch(link))
			{
				return true;
			}

			return false;
		}

		public static string GetRootUri(string address)
		{
			return UriRegEx.IsMatch(address) ? UriRegEx.Match(address).Value : String.Empty;
		}

		public static bool IsFileUri(string address)
		{
			if (!address.Contains('/')) return false;
			var name = address.Substring(address.LastIndexOf('/'));
			return name.Contains('.');
		}

		public static string GetFileNameFromUri(string address)
		{
			if (address.Contains('?')) address = address.Substring(0, address.IndexOf('?'));
			return (IsFileUri(address)) ? address.Substring(address.LastIndexOf('/') + 1) : String.Empty;
		}
	}
}
