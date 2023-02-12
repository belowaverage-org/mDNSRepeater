using System.Net;
using System.Net.Sockets;

namespace MDNSRepeater
{
    internal class UnicastSender
    {
        readonly IPEndPoint[] Recipients;
        readonly int DestPort = 5354;
        readonly UdpClient Client;
        readonly IPEndPoint LocalEndpoint;
        internal UnicastSender()
        {
            Shared.Log("Setting up UCS...");
            string[] sRecipients = Environment.GetEnvironmentVariable("mDNSRepeater_UCSDestAddresses")?.Split(',') ?? Array.Empty<string>();
            Shared.Log("UCS destination addresses: " + string.Join(", ", sRecipients) + ".");
            DestPort = int.Parse(Environment.GetEnvironmentVariable("mDNSRepeater_UCSDestPort") ?? DestPort.ToString());
            Shared.Log("UCS destination port: " + DestPort + ".");
            Recipients = new IPEndPoint[sRecipients.Length];
            for (int i = 0; i < sRecipients.Length; i++) Recipients[i] = new IPEndPoint(IPAddress.Parse(sRecipients[i]), DestPort);
            LocalEndpoint = new(IPAddress.Any, 0);
            Client = new UdpClient();
            Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            Client.Client.Bind(LocalEndpoint);
            Shared.Log("UCS ready.");
        }
        internal void Send(byte[] data)
        {
            foreach (IPEndPoint ep in Recipients)
            {
                Client.Send(data, ep);
                Shared.Log("UCS -> " + ep.ToString());
            }
        }
    }
}