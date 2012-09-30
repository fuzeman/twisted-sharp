using System;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using twistedsharp.Internet.Protocol;

namespace twistedsharp.Internet
{
	public class SSLConnector : IConnector
	{
		public TcpClient Client { get; set; }
		public Factory Factory { get; set; }

		public Transport Transport { get; set; }
		public Stream Stream { get; set; }

		public SSLConnector(TcpClient client, Factory factory)
		{
			Client = client;
			Factory = factory;

			Stream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateCert));
			Transport = new Transport(Stream);
			((SslStream)Stream).AuthenticateAsClient("twisted-sharp", null, SslProtocols.Ssl3, false);

			factory.Protocol.MakeConnection(Transport);
		}

		public void Close()
		{
			Stream.Close();
			Client.Close();
		}

		private bool ValidateCert(object sender, X509Certificate certificate,
		                          X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			Console.WriteLine("Certificate Validated");
			return true;
		}
	}
}

