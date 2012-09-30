using System;
using System.Net.Sockets;
using twistedsharp.Internet.Protocol;

namespace twistedsharp.Internet
{
	public class Reactor
	{
		public static IConnector connectSSL(string host, int port, Factory factory, object contextFactory)
		{
			return connectSSL(host, port, factory, contextFactory, 30, null);
		}
		public static IConnector connectSSL(string host, int port, Factory factory, object contextFactory, int timeout)
		{
			return connectSSL(host, port, factory, contextFactory, timeout);
		}
		public static IConnector connectSSL(string host, int port, Factory factory, object contextFactory, int timeout, string bindAddress)
		{
			TcpClient client = new TcpClient(host, port);
			return new SSLConnector(client, factory);
		}
	}
}

