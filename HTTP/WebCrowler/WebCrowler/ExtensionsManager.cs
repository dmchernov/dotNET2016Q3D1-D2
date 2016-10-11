using System;

namespace WebCrowler
{
	public static class ExtensionsManager
	{
		public static string GetExtension(this string address)
		{
			var index = address.LastIndexOf('.');
			if (index != -1)
				return address.Substring(index + 1);
			return String.Empty;
		}

		public static string GetExtensionForMime(string mimeType)
		{
			switch (mimeType)
			{
				case "image/gif":
					return "gif";
				case "image/jpeg":
				case "image/pjpeg":
					return "jpeg";
				case "image/png":
					return "png";
				case "image/svg+xml":
					return "svg";
				case "image / tiff":
					return "tiff";
				case "image/vnd.microsoft.icon":
					return "ico";
				case "image/vnd.wap.wbmp":
					return "bmp";
				case "image/webp":
					return "webp";
				default:
					return String.Empty;
			}
		}
	}
}
