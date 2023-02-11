using System.Net;
using System.Net.Sockets;

namespace MDNSRepeater
{
    internal class MulticastReceiver : IDisposable
    {
        bool Disposing = false;
        UdpClient Client;
        IPEndPoint Endpoint = new IPEndPoint(IPAddress.Any, 5353);
        internal MulticastReceiver()
        {
            Shared.Log("Setting up MCR...");
            Client = new UdpClient();
            Client.MulticastLoopback = false;
            Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Client.Client.Bind(Endpoint);
            Client.JoinMulticastGroup(IPAddress.Parse("224.0.0.251"));
            Task.Run(Listener);
            Shared.Log("MCR ready.");
        }
        void Listener()
        {
            while (!Disposing)
            {
                byte[] datagram = Client.Receive(ref Endpoint);
                Shared.Log("MCR <- " + Endpoint.ToString());
                Program.UnicastSender.Send(datagram);
            }
        }
        public void Dispose()
        {
            Disposing = true;
            Client.Close();
            Client.Dispose();
        }
    }
}