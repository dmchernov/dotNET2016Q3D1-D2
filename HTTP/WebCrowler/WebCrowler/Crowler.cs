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
	    public Crowler(int level, bool currentDomain, string path)
	    {
		    _level = level;
		    _currentDomain = currentDomain;
		    Saver.SetRoot(path);
	    }

	    public EventHandler<PageEventArgs> ContentLoaded;
	    public EventHandler<PageEventArgs> PageNotLoaded;

	    //public Crowler() : this(0, true) { }

	    public void Run(string address)
	    {
		    Load(address, _level);
	    }

	    private void Load(string address, int level)
	    {
			if (level < 0) return;

			var pattern = @"(http|https){1}\://[a-zA-Z]+[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}";
			var regEx = new Regex(pattern);

			if (String.IsNullOrEmpty(_rootAddress))
		    {
			    _rootAddress = regEx.Match(address).Value; ;
			    Pages.AddPage(_rootAddress);
		    }

		    var client = new HttpClient();
		    string html;
		    try
		    {
				html = client.GetStringAsync(address).Result;
			}
		    catch (Exception)
		    {
				if (PageNotLoaded != null)
					PageNotLoaded?.Invoke(this, new PageEventArgs {Address = address});
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
				if (String.IsNullOrEmpty(s) || s == "/" || s.Contains("#") || s.Contains("@")) continue;
				links.Add(s);
			}

			// Оставляем только уникальные ссылки
			links = links.Distinct().ToList();

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
				
				if (Pages.CanBeLoad(newLink))
				{
					Pages.AddPage(newLink);
					Load(newLink, level - 1);
				}
			}
		}
    }
}
