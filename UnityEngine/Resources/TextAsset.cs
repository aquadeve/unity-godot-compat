namespace UnityEngine
{
	/// <summary>
	/// Represents a text file asset. Used by Resources.Load.
	/// </summary>
	public class TextAsset : Object
	{
		private readonly string _text;
		private readonly byte[] _bytes;

		public TextAsset() { _text = ""; _bytes = System.Array.Empty<byte>(); }
		public TextAsset(string text) { _text = text ?? ""; _bytes = System.Text.Encoding.UTF8.GetBytes(_text); }

		public string text => _text;
		public byte[] bytes => _bytes;

		public override string ToString() => _text;
	}
}
