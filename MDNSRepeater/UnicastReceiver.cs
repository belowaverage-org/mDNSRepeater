using System.Net;
using System.Net.Sockets;

namespace MDNSRepeater
{
    internal class UnicastReceiver : IDisposable
    {
        bool Disposing = false;
        UdpClient Client;
        IPEndPoint Endpoint;
        int ListenPort = 5354;
        internal UnicastReceiver()
        {
            Shared.Log("Setting up UCR...");
            Client = new UdpClient();
            ListenPort = int.Parse(Environment.GetEnvironmentVariable("mDNSRepeater_UCRListenPort") ?? ListenPort.ToString());
            Shared.Log("UCR listening port: " + ListenPort + ".");
            Endpoint = new IPEndPoint(IPAddress.Any, ListenPort);
            Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Client.Client.Bind(Endpoint);
            Task.Run(Listener);
            Shared.Log("UCR ready.");
        }
        void Listener()
        {
            while (!Disposing)
            {
                byte[] datagram = Client.Receive(ref Endpoint);
                Shared.Log("UCR <- " + Endpoint.ToString());
                Program.MulticastSender.Send(datagram);
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