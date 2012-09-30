using System;
using System.IO;

namespace twistedsharp
{
	public class ReadInfo
	{
		public byte[] Buffer { get; set; }
		public Stream Stream { get; set; }

		public ReadInfo(byte[] buffer, Stream stream)
		{
			Buffer = buffer;
			Stream = stream;
		}
	}
}

