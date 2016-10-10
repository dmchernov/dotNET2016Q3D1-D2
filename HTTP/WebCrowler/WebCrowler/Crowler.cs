using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;

namespace WebCrowler
{
    public class Crowler
    {
	    private readonly int _level;
	    private readonly bool _currentDomain;
	    private string _rootAddress;
	    
	    public Crowler(int level, bool currentDomain, string path, List<String> ext)
	    {
		    _level = level;
		    _currentDomain = currentDomain;
		    Saver.SetRoot(path);
		    Saver.Extensions = ext;
	    }

	    public EventHandler<PageEventArgs> ContentLoaded;
	    public EventHandler<PageEventArgs> ContentNotLoaded;
	    public EventHandler<PageEventArgs> ContentSkipped;

	    public void Run(string address)
	    {
		    Load(address, _level);
	    }

	    private void Load(string address, int level)
	    {
			if (level < 0) return;

			var uri = @"(http|https){1}\://[a-zA-Z]+[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}";
		    var domain = @"[a-zA-Z]+[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}";
			var regExUri = new Regex(uri);
		    var regExDomain = new Regex(domain);

			if (String.IsNullOrEmpty(_rootAddress))
		    {
			    _rootAddress = regExUri.Match(address).Value;
			    Pages.AddPage(_rootAddress);
		    }

		    var client = new HttpClient();
		    string html = String.Empty;
		    try
		    {
			    var responce = client.GetAsync(address);
				if (responce.Result.Content.Headers.ContentType.MediaType == "text/html")
					html = client.GetStringAsync(address).Result;
			}
		    catch (Exception)
		    {
			    ContentNotLoaded?.Invoke(this, new PageEventArgs {Address = address});
			    return;
		    }
		    
			if (!String.IsNullOrEmpty(html))
			{
				if (Saver.SavePage(address, html))
					ContentLoaded?.Invoke(this, new PageEventArgs() { Address = address });
			}

			var links = new List<string>();

			IHtmlDocument angle = new HtmlParser().Parse(html);
			
			// Ссылки на другие ресурсы
			foreach (var element in angle.QuerySelectorAll("a"))
			{
				var s = element.GetAttribute("href");
				if (String.IsNullOrEmpty(s) || s == "/" || s.Contains("#") || s.Contains("@") || s.Contains(",") || s.Contains("*")) continue;
				links.Add(s);
			}

			// Ссылки на картинки
			foreach (var element in angle.QuerySelectorAll("img"))
			{
				var s = element.GetAttribute("src");
				if (String.IsNullOrEmpty(s) || s == "/" || s.Contains("#") || s.Contains("?") || s.Contains(",") || s.Contains("*")) continue;

				while (s[0] == '/')
				{
					s = s.Substring(1);
				}

				if (!regExUri.IsMatch(s))
				{
					if (!regExDomain.IsMatch(s) || regExDomain.IsMatch(s) && (s.Contains("/") && !s.Substring(0, s.IndexOf("/", StringComparison.Ordinal)).Contains(".")))
						s = regExUri.Match(address).Value + "/" + s;
					else
						s = "http://" + s;
				}

				try
				{
					if (Pages.CanBeLoad(s) && client.GetAsync(s).Result.StatusCode == HttpStatusCode.OK)
					{
						Pages.AddPage(s);
						if (Saver.SaveFile(s))
							ContentLoaded?.Invoke(this, new PageEventArgs {Address = s});
						else
							ContentNotLoaded?.Invoke(this, new PageEventArgs { Address = s });
					}
					else
					{
						ContentSkipped(this, new PageEventArgs {Address = s});
					}
				}
				catch (Exception)
				{
					ContentNotLoaded?.Invoke(this, new PageEventArgs {Address = s});
				}
			}

			// Оставляем только уникальные ссылки
			links = links.Distinct().ToList();

			foreach (var link in links)
			{
				string newLink = link;
				while (newLink[0] == '/')
				{
					newLink = newLink.Substring(1);
				}
				
				if (!regExUri.IsMatch(newLink))
				{
					if (regExDomain.IsMatch(newLink) && ((newLink.Contains("/") && newLink.Substring(0, newLink.IndexOf('/')) == regExDomain.Match(newLink).Value) || newLink == regExDomain.Match(newLink).Value))
						newLink = "http://" + newLink;
					else if (!String.IsNullOrEmpty(regExUri.Match(address).Value))
						newLink = regExUri.Match(address).Value + (link[0] == '/' ? link : "/" + link);
					else
					{
						ContentSkipped?.Invoke(this, new PageEventArgs {Address = newLink});
						continue;
					}
				}
				if (_currentDomain)
				{
					var s = regExUri.Match(newLink).Value;
					if (s != regExUri.Match(_rootAddress).Value)
					{
						ContentSkipped?.Invoke(this, new PageEventArgs { Address = newLink });
						continue;
					}
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
