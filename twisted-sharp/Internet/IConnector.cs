using System;
using System.IO;
using System.Net.Sockets;
using twistedsharp.Internet.Protocol;

namespace twistedsharp.Internet
{
	public interface IConnector
	{
		TcpClient Client { get; set; }
		Factory Factory { get; set; }

		Stream Stream { get; set;}
		Transport Transport { get; set; }
	}
}

