using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Rules.Engine.Tests.Utilities
{
    internal sealed class SmtpServerSession : IDisposable
    {
        private NetworkStream _stream;
        private SmtpServerState _state;
        private TcpListener _listener;
        private readonly ManualResetEvent _closed;
        private void AcceptConnection(IAsyncResult result)
        {
            var listener = (TcpListener)result.AsyncState;
            using (_stream = new NetworkStream(listener.EndAcceptSocket(result)))
            {
                _stream.ReadTimeout = 500;
                _state = new SmtpServerState(_stream);
                _state.Accept();
            }
            _closed.Set();
        }

        public SmtpServerSession(IPEndPoint endPoint) : this(endPoint.Address, endPoint.Port)
        {
        }
        public SmtpServerSession(IPAddress address, int port)
        {
            _closed = new ManualResetEvent(false);

            _listener = new TcpListener(address, port);
            _listener.Start(1);
            _listener.BeginAcceptSocket(AcceptConnection, _listener);
        }
        public void Dispose()
        {
            _closed.WaitOne(20000);

            _listener.Stop();
            if (_stream != null)
            {
                _stream.Close();
                _stream.Dispose();
            }

            _listener = null;
            _stream = null;
        }
        public SmtpServerState State
        {
            get
            {
                if (!_closed.WaitOne(3000))
                {
                    throw new Exception("Waited 3000ms but session did not end.");
                }
                return _state;
            }
        }
    }
}

