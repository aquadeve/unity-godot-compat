using System.Collections.Generic;
using Godot;

namespace UnityEngine
{
	public partial class WWW : CustomYieldInstruction, System.IDisposable
	{
		HttpRequest? requestNode;

		public string url { get { return _url; } }
		string _url = "";

		byte[] _bytes = new byte[0];
		public byte[] bytes { get { return _bytes; } }
		public string text { get { return System.Text.Encoding.Default.GetString(_bytes); } }
		public Texture2D texture { get { return new Texture2D(bytes); } }
		public string? error { get; private set; }

		bool _isDone = false;
		public bool isDone { get { return _isDone; } }

		UnityWebRequestMethod _method = UnityWebRequestMethod.Get;


		public override bool keepWaiting
		{
			get { return !_isDone; }
		}


		public Dictionary<string, string> responseHeaders
		{
			get
			{
				Debug.Log("WWW.responseHeaders not implemented");
				return new Dictionary<string, string>();
			}
		}


		public WWW(string url)
		{
			_url = url;
			SendRequest();
		}


		public WWW(string url, WWWForm form)
		{
			_url = url;
			_method = UnityWebRequestMethod.Post;
			SendRequest(System.Text.Encoding.Default.GetString(form.data));
		}


		void SendRequest(string requestData = "")
		{
			if ( _url.StartsWith("file://") )
			{
				string path = _url.Substring(7);
				Debug.Log(_url);

				if ( System.IO.File.Exists(path) )
				{
					_bytes = System.IO.File.ReadAllBytes(path);
				}
				else
				{
					error = "Couldn't find file at path: " + path;
				}

				_isDone = true;
			}
			else
			{
				requestNode = new HttpRequest()
				{
					UseThreads = true
				};
				UnityEngineAutoLoad.Instance?.AddChild(requestNode);

				requestNode.RequestCompleted += OnRequestCompleted;

				string[] headers = new string[]
				{
					"Content-Type: application/x-www-form-urlencoded",
					"Content-Length: " + requestData.Length
				};

				requestNode.Request(_url, headers, HttpClient.Method.Get, requestData);
			}
		}


		void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
		{
			_bytes = body;
			_isDone = true;

			requestNode?.QueueFree();
		}


		public void Dispose()
		{
			requestNode?.QueueFree();
			requestNode = null;
		}


		internal enum UnityWebRequestMethod
		{
			Get,
			Head,
			Post,
			Put,
			Delete,
			Options,
			Trace,
			Connect,
			Max
		}


		public static string EscapeURL(string s)
		{
            return s.URIEncode();
		}


		public static string UnEscapeURL(string s)
		{
            return s.URIDecode();
		}


		public AudioClip? GetAudioClip()
		{
			Debug.Log("Implement getting audio clips");
			return null;
		}
	}
}