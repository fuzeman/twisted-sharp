using System;
using System.Net.Sockets;
using System.Threading;
using twistedsharp.Internet.Protocol;

namespace twistedsharp.Internet
{
    public struct AsyncConnectState
    {
        public Reactor.ConnectedCallback responseCallback;
        public Reactor.TimeoutCallback timeoutCallback;
        public TcpClient client;
        public Factory factory;
        public object state;
    }
	public class Reactor
	{
        public delegate void ConnectedCallback(IConnector connector, object state);
        public delegate void TimeoutCallback(object state);

		public static IConnector ConnectSSL(string host, int port, Factory factory, object contextFactory)
		{
			return ConnectSSL(host, port, factory, contextFactory, 2000);
		}
		public static IConnector ConnectSSL(string host, int port, Factory factory, object contextFactory, int timeout)
		{
			return ConnectSSL(host, port, factory, contextFactory, timeout, null);
		}
		public static IConnector ConnectSSL(string host, int port, Factory factory, object contextFactory, int timeout, string bindAddress)
		{
			TcpClient client = new TcpClient();
            IAsyncResult ar = client.BeginConnect(host, port, null, null);
            WaitHandle wh = ar.AsyncWaitHandle;
            try
            {
                if (!wh.WaitOne(TimeSpan.FromMilliseconds(timeout), false))
                {
                    client.Close();
                    throw new TimeoutException();
                }
                client.EndConnect(ar);
            }
            finally
            {
                wh.Close();
            }
			return new SSLConnector(client, factory);
		}

        public static void ConnectSSLAsync(string host, int port, Factory factory, object contextFactory, ConnectedCallback callback, TimeoutCallback timeoutCallback, object state)
        {
            ConnectSSLAsync(host, port, factory, contextFactory, 2000, callback, timeoutCallback, state);
        }
        public static void ConnectSSLAsync(string host, int port, Factory factory, object contextFactory, int timeout, ConnectedCallback callback, TimeoutCallback timeoutCallback, object state)
        {
            ConnectSSLAsync(host, port, factory, contextFactory, timeout, null, callback, timeoutCallback, state);
        }
        public static void ConnectSSLAsync(string host, int port, Factory factory, object contextFactory, int timeout, string bindAddress, ConnectedCallback callback, TimeoutCallback timeoutCallback, object state)
        {
            TcpClient client = new TcpClient();
            IAsyncResult ar = client.BeginConnect(host, port, null, new AsyncConnectState {
                responseCallback = callback,
                timeoutCallback = timeoutCallback,
                client = client,
                factory = factory,
                state = state
            });

            ThreadPool.RegisterWaitForSingleObject(ar.AsyncWaitHandle, _ConnectSSL_Callback, ar, timeout, true);
        }
        private static void _ConnectSSL_Callback(object async_state, bool timedOut)
        {
            IAsyncResult ar = async_state as IAsyncResult;
            AsyncConnectState state = (AsyncConnectState)ar.AsyncState;

            if (!timedOut && state.client.Connected)
                state.responseCallback(new SSLConnector(state.client, state.factory), state.state);
            else
                state.timeoutCallback(state.state);
        }
	}
}

