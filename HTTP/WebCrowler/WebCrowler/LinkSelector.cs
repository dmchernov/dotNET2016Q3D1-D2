using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace WebCrowler
{
	public class LinkSelector
	{
		public string RootDomain { get; set; }
		public List<string> GetLinksFromHtml(string html, string htmlElement, string attribute)
		{
			var links = new List<string>();

			IHtmlDocument angle = new HtmlParser().Parse(html);

			foreach (var element in angle.QuerySelectorAll(htmlElement))
			{
				var attrValue = element.GetAttribute(attribute);
				if (String.IsNullOrEmpty(attrValue)
					|| attrValue == "/"
					|| attrValue.Contains("#")
					|| attrValue.Contains("@")
					|| attrValue.Contains(",")
					|| attrValue.Contains("*"))
						continue;
				var validLink = UrlHelper.GetValidUri(RootDomain, attrValue);
				links.Add(validLink);
			}

			return links.Distinct().ToList();
		}
	}
}
