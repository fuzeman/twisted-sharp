using System;
using System.IO;
using twistedsharp.Utils;

namespace twistedsharp.Internet.Protocol
{
	public class Transport
	{
		public const int READ_BUFFER_SIZE = 1024;

		public Stream Stream { get; set; }
		public BaseProtocol Protocol { get; internal set; }

		public Transport(Stream stream)
		{
			Stream = stream;
		}

		public void Read()
		{
			byte[] buffer = new byte[READ_BUFFER_SIZE];
			Read(new ReadInfo(buffer, this.Stream));
		}

		public void Read(ReadInfo info)
		{
			info.Stream.BeginRead(info.Buffer, 0, info.Buffer.Length,
			                 new AsyncCallback(ReadCallback), info);
		}

		private void ReadCallback(IAsyncResult ar)
		{
			ReadInfo info = ar.AsyncState as ReadInfo;

			int read = 0;
			try
			{
				read = info.Stream.EndRead(ar);
			} catch(IOException ex)
			{
				throw new Exception("Read Error", ex);
			}

			string data = EncodingHelper.GetString(info.Buffer, 0, read);

			if(Protocol == null)
				throw new Exception("No Protocol Set, (Use BaseProtocol.MakeConnection)");

			Protocol.DataReceived(data);

			Read(info);
		}

		public void Write(string data)
		{
			Write(EncodingHelper.GetBytes(data));
		}

		public void Write(byte[] data)
		{
			Write(data, 0, data.Length);
		}

		public void Write(byte[] data, int offset, int count)
		{
			this.Stream.Write(data, offset, count);
		}
	}
}

