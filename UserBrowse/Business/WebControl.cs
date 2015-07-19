using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UserBrowse
{
	/// <summary>
	/// This class serves as the engine for making requests to our API.
	/// 
	/// We assume that we will only be using GET, POST, and multipart
	/// requests - If we choose to include PUT and DELETE, we can
	/// refactor.
	/// </summary>
	public static class WebControl
	{
		private static readonly Encoding encoding = Encoding.UTF8;

		/// <summary>
		/// Makes an asynchronous GET request.
		/// </summary>
		/// <returns>The string response from the server</returns>
		/// <param name="path">Path which we are making our request to.</param>
		public static async Task<string> MakeGetRequestAsync (string path)
		{
			var request = (HttpWebRequest)WebRequest.Create (WebConstants.Url + path);
			request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
			request.Method = "GET";

			try {
				var response = await request.GetResponseAsync ();
				var respStream = response.GetResponseStream ();
				respStream.Flush ();

				using (StreamReader sr = new StreamReader (respStream)) {
					//Need to return this response 
					string strContent = sr.ReadToEnd ();
					respStream = null;
					return strContent;
				}
			} catch (Exception ex) {    
				string message = ex.Message;
				return string.Empty;
			}
		}




		/// <summary>
		/// Makes an asynchronous POST request.
		/// </summary>
		/// <returns>The string response from the server</returns>
		/// <param name="path">Path which we are making our request to</param>
		/// <param name="data">POST data</param>
		/// <param name="isJson">Whether or not our ContentType is JSON</param>
		public static async Task<string> MakePostRequestAsync (string path, string data, bool isJson = true)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (WebConstants.Url + path);
			if (isJson)
				request.ContentType = "application/json; charset=UTF-8";
			else
				request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";
			request.Accept = "application/json";

			//request.Headers ["Cookie"] = WebConstants.Cookie;
			var stream = await request.GetRequestStreamAsync ();
			using (var writer = new StreamWriter (stream)) {
				writer.Write (data);
			}

			var response = await request.GetResponseAsync ();
			var respStream = response.GetResponseStream ();

			using (StreamReader sr = new StreamReader (respStream)) {
				return sr.ReadToEnd ();
			}
		}

		/// <summary>
		/// Makes an asynchronous multipart POST request.
		/// </summary>
		/// <returns>The string reponse from the server</returns>
		/// <param name="path">Path which we are making our request to</param>
		/// <param name="postParameters">Post parameters.</param>
		public async static Task<string> PostMultipartAsync (string path, Dictionary<string, object> postParameters)
		{
			string formDataBoundary = String.Format ("----------{0:N}", Guid.NewGuid ());
			string contentType = "multipart/form-data; boundary=" + formDataBoundary;

			byte[] formData = GetMultipartFormData (postParameters, formDataBoundary);

			return await PostForm (WebConstants.Url + path, contentType, formData);
		}

		/// <summary>
		/// Posts the multipart form.
		/// </summary>
		/// <returns>The string response from the server</returns>
		/// <param name="postUrl">Post URL</param>
		/// <param name="contentType">Content type</param>
		/// <param name="formData">Form data</param>
		private async static Task<string> PostForm (string postUrl, string contentType, byte[] formData)
		{
			HttpWebRequest request = WebRequest.Create (postUrl) as HttpWebRequest;

			if (request == null) {
				throw new NullReferenceException ("Request is not an HTTP request");
			}

			// Set up the request properties.
			request.Method = "POST";
			request.ContentType = contentType;

			//For an already built ASP MVC app, our authentication will lie in the cookie.
			//request.Headers ["Cookie"] = WebConstants.Cookie;

			// You could add authentication here as well if needed:
			// request.PreAuthenticate = true;
			// request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
			// request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("username" + ":" + "password")));

			// Send the form data to the request.
			using (Stream requestStream = await request.GetRequestStreamAsync ()) {
				requestStream.Write (formData, 0, formData.Length);

			}

			WebResponse resp = await request.GetResponseAsync ();

			using (Stream stream = resp.GetResponseStream ()) {
				StreamReader respReader = new StreamReader (stream);
				return respReader.ReadToEnd ();
			}

		}
		/// <summary>
		/// Creates the multipart form.
		/// </summary>
		/// <returns>The multipart form data</returns>
		/// <param name="postParameters">Post parameters</param>
		/// <param name="boundary">Boundary</param>
		private static byte[] GetMultipartFormData (Dictionary<string, object> postParameters, string boundary)
		{
			using (Stream formDataStream = new System.IO.MemoryStream ()) {
				bool needsCLRF = false;

				foreach (var param in postParameters) {
					// Add a CRLF to allow multiple parameters to be added.
					// Skip it on the first parameter, add it to subsequent parameters.
					if (needsCLRF)
						formDataStream.Write (encoding.GetBytes ("\r\n"), 0, encoding.GetByteCount ("\r\n"));

					needsCLRF = true;

					if (param.Value is FileParameter) {
						FileParameter fileToUpload = (FileParameter)param.Value;

						// Add just the first part of this param, since we will write the file data directly to the Stream
						string header = string.Format ("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
							boundary,
							param.Key,
							fileToUpload.FileName ?? param.Key,
							fileToUpload.ContentType ?? "application/octet-stream");

						formDataStream.Write (encoding.GetBytes (header), 0, encoding.GetByteCount (header));

						// Write the file data directly to the Stream, rather than serializing it to a string.
						formDataStream.Write (fileToUpload.File, 0, fileToUpload.File.Length);
					} else {
						string postData = string.Format ("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
							boundary,
							param.Key,
							param.Value);
						formDataStream.Write (encoding.GetBytes (postData), 0, encoding.GetByteCount (postData));
					}
				}

				// Add the end of the request.  Start with a newline
				string footer = "\r\n--" + boundary + "--\r\n";
				formDataStream.Write (encoding.GetBytes (footer), 0, encoding.GetByteCount (footer));

				// Dump the Stream into a byte[]
				formDataStream.Position = 0;
				byte[] formData = new byte[formDataStream.Length];
				formDataStream.Read (formData, 0, formData.Length);

				return formData;
			}
		}

		/// <summary>
		/// When using multipart forms, we'll wrap our files in this class.
		/// </summary>
		public class FileParameter
		{
			public byte[] File { get; set; }
			public string FileName { get; set; }
			public string ContentType { get; set; }
			public FileParameter (byte[] file) : this (file, null){}
			public FileParameter (byte[] file, string filename) : this (file, filename, null){}

			public FileParameter (byte[] file, string fileName, string contentType)
			{
				File = file;
				FileName = fileName;
				ContentType = contentType;
			}
		}
	}

}