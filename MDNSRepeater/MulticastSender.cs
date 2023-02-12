using System.Net;
using System.Net.Sockets;

namespace MDNSRepeater
{
    internal class MulticastSender
    {
        readonly UdpClient Client;
        readonly IPEndPoint LocalEndpoint = new(IPAddress.Any, 5353);
        readonly IPEndPoint RemoteEndpoint = new(IPAddress.Parse("224.0.0.251"), 5353);
        internal MulticastSender()
        {
            Shared.Log("Setting up MCS...");
            Client = new UdpClient();
            Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Client.Client.Bind(LocalEndpoint);
            Shared.Log("MCS ready.");
        }
        internal void Send(byte[] data)
        {
            Client.Send(data, RemoteEndpoint);
            Shared.Log("MCS -> " + RemoteEndpoint.ToString());
        }
    }
}