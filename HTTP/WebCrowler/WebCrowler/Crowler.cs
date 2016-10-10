using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace WebCrowler
{
    public class Crowler
    {
	    private readonly int _level;
	    private bool _currentDomain;
	    private string _rootAddress;
	    public Crowler(int level, bool currentDomain/*, List<string> extensionsList*/)
	    {
		    _level = level;
		    _currentDomain = currentDomain;
		    //Saver.Extensions = extensionsList;
	    }

	    public EventHandler<PageEventArgs> ContentLoaded;

	    public Crowler() : this(0, true) { }

	    public void Run(string address)
	    {
		    Load(address, _level);
	    }

	    private void Load(string address, int level)
	    {
			if (level < 0) return;

			if (String.IsNullOrEmpty(_rootAddress))
		    {
			    _rootAddress = address;
			    Pages.AddRootPage(address);
		    }

		    var client = new HttpClient();
		    string html;
		    try
		    {
				html = client.GetStringAsync(address).Result;
			}
		    catch (Exception)
		    {
			    return;
		    }
		    
			if (!String.IsNullOrEmpty(html))
			{
				if (Saver.Save(address, html))
					ContentLoaded?.Invoke(this, new PageEventArgs() { Address = address });
			}

			var links = new List<string>();

			IHtmlDocument angle = new HtmlParser().Parse(html);
			foreach (var element in angle.QuerySelectorAll("a"))
			{
				var s = element.GetAttribute("href");
				if (String.IsNullOrEmpty(s) || s == "/" || s.Contains("#")) continue;
				links.Add(s);
			}

			// Оставляем только уникальные ссылки
			links = links.Distinct().ToList();

		    var pattern = @"(http|https){1}\://[a-zA-Z]+[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}";

			var regEx = new Regex(pattern);

			foreach (var link in links)
			{
				string newLink = link;
				if (!regEx.IsMatch(link))
				{
					newLink = _rootAddress + (link[0] == '/' ? link : "/" + link);
				}
				if (_currentDomain)
				{
					var s = regEx.Match(newLink).Value;
					if (s != regEx.Match(_rootAddress).Value)
						continue;
				}
				
				//Console.WriteLine(newLink);
				if (Pages.CanBeLoad(newLink))
					Load(newLink, level - 1);
			}
		}
    }
}
