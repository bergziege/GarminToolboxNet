﻿using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace GarminConnectExporter.Infrastructure
{
	static class HttpUtils
	{
		/// <summary>
		/// Creates a NameValueCollection that can be easily converted to a query string by calling ToString()
		/// </summary>
		public static NameValueCollection CreateQueryString()
		{
			return HttpUtility.ParseQueryString(String.Empty);
		}

		public static void AddIfNotNull(this NameValueCollection collection, string key, object value)
		{
			if (value != null) collection.Add(key, value.ToString());
		}

		public static void AddIfNotNull(this NameValueCollection collection, string key, bool? value)
		{
			if (value != null) collection.Add(key, value.ToString().ToLower());
		}

		public static void AddIfNotNull(this NameValueCollection collection, string key, int? value)
		{
			if (value != null) collection.Add(key, value.ToString());
		}

		public static HttpWebRequest CreateRequest(string url, CookieContainer cookies)
		{
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.CookieContainer = cookies;
			request.KeepAlive = true;
			request.Method = "GET";
		    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:40.0) Gecko/20100101 Firefox/40.1";
		    request.Timeout = 50000;
			return request;
		}

		public static void WriteFormData(this HttpWebRequest request, NameValueCollection data)
		{
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			WriteBinary(request, Encoding.UTF8.GetBytes(data.ToString()));
		}

		public static void WriteBinary(this HttpWebRequest request, byte[] data)
		{
			if (request == null) throw new ArgumentNullException("request");
			if (data == null) throw new ArgumentNullException("data");

			request.ContentLength = data.Length;

			using (Stream stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
				stream.Flush();
				stream.Close();
			}
		}

		public static void SaveResponseToFile(this HttpWebResponse response, string targetFilePath)
		{
			if (response == null) throw new ArgumentNullException("response");
			if (String.IsNullOrEmpty(targetFilePath)) throw new ArgumentException("No target file path specified.", "targetFilePath");

			using (Stream responseStream = response.GetResponseStream())
			using (FileStream fileStream = File.Create(targetFilePath))
			{
				if (responseStream != null && response.ContentLength > 0)
					responseStream.CopyTo(fileStream);
			}
		}

		public static string GetResponseAsString(this HttpWebResponse response)
		{
			if (response == null) throw new ArgumentNullException("response");

			Stream responseStream = response.GetResponseStream();
			if (responseStream == null) return null;

			using (var stream = new StreamReader(responseStream, Encoding.UTF8))
			{
				return stream.ReadToEnd();
			}
		}
	}

}
