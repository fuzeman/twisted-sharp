using System;

namespace twistedsharp.Internet.Protocol
{
	public abstract class BaseProtocol
	{
		public Transport Transport { get; internal set; }

		public BaseProtocol()
		{
		}

		public virtual void MakeConnection(Transport transport)
		{
			this.Transport = transport;
			this.Transport.Protocol = this;
			this.ConnectionMade();
			this.Transport.Read();
		}

		public virtual void ConnectionMade()
		{

		}

		public virtual void ConnectionLost()
		{

		}

		public virtual void DataReceived(string data)
		{

		}
	}
}

