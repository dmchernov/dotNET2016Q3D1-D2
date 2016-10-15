using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace WebCrowler
{
	public class Crowler
	{
		private readonly int _level;
		private readonly bool _currentDomain;
		private string _rootAddress;
		private readonly LinkSelector _selector;
		
		public Crowler(int level, bool currentDomain, string path, List<String> ext)
		{
			_level = level;
			_currentDomain = currentDomain;
			Saver.SetRoot(path);
			Saver.Extensions = ext;
			_selector = new LinkSelector();
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
			// Окончание загрузки, если достигнута максимальная глубина рекурсии
			if (level < 0) return;

			// Домен, с которым было запущено приложение
			if (String.IsNullOrEmpty(_rootAddress))
			{
				_rootAddress = UrlHelper.GetRootUri(address);
				LinkRepository.AddLink(_rootAddress);
			}

			LinkRepository.AddLink(address);

			// Установка корневого домена для корректного формирования ссылок
			_selector.RootDomain = UrlHelper.GetRootUri(address);
			if (String.IsNullOrEmpty(_selector.RootDomain)) return;

			

			// HTML код страницы
			string html = String.Empty;

			using (var client = new HttpClient())
			{
				try
				{
					var responce = client.GetAsync(address);
					if (responce.Result.Content.Headers.ContentType.MediaType == "text/html" && responce.Result.StatusCode == HttpStatusCode.OK)
						html = client.GetStringAsync(address).Result;
				}
				catch (Exception)
				{
					ContentNotLoaded?.Invoke(this, new PageEventArgs {Address = address});
					return;
				}
			}

			if (!String.IsNullOrEmpty(html))
			{
				if (Saver.Save(address))
					ContentLoaded?.Invoke(this, new PageEventArgs() { Address = address });
			}
			else return;

			// Ссылки на web страницы
			var links = _selector.GetLinksFromHtml(html, "a", "href");

			// Загрузка картинок
			LoadContentFromAttribute(html, "img", "src");

			// Загрузка web страниц, на которые указывают ссылки с данной страницы
			foreach (var link in links)
			{
				if (_currentDomain && !UrlHelper.IsCurrentDomainLink(_rootAddress, link))
					continue;

				if (LinkRepository.CanBeLoad(link))
				{
					LinkRepository.AddLink(link);
					Load(link, level - 1);
				}
			}
		}

		private void LoadContentFromAttribute(string html, string element, string attribute)
		{
			var contentLinks = _selector.GetLinksFromHtml(html, element, attribute);
			foreach (var content in contentLinks)
			{
				try
				{
					if (LinkRepository.CanBeLoad(content))
					{
						LinkRepository.AddLink(content);
						if (Saver.Save(content))
							ContentLoaded?.Invoke(this, new PageEventArgs { Address = content });
						else
							ContentNotLoaded?.Invoke(this, new PageEventArgs { Address = content });
					}
					else
					{
						ContentSkipped(this, new PageEventArgs { Address = content });
					}
				}
				catch (Exception)
				{
					ContentNotLoaded?.Invoke(this, new PageEventArgs { Address = content });
				}
			}
		}
	}
}
