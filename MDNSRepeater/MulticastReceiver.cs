using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MDNSRepeater
{
    internal class MulticastReceiver : IDisposable
    {
        bool Disposing = false;
        readonly UdpClient Client;
        IPEndPoint Endpoint = new(IPAddress.Any, 5353);
        readonly IPAddress[] SelfAddresses;
        internal MulticastReceiver()
        {
            Shared.Log("Setting up MCR...");
            SelfAddresses = ListAdapterIPs();
            Client = new UdpClient();
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
                if (!SelfAddresses.Contains(Endpoint.Address))
                {
                    Shared.Log("MCR <- " + Endpoint.ToString());
                    Program.UnicastSender.Send(datagram);
                }
            }
        }
        static IPAddress[] ListAdapterIPs()
        {
            Shared.Log("Enumerating assigned IPs...");
            List<IPAddress> ips = new();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface inter in interfaces)
            {
                foreach (UnicastIPAddressInformation ip in inter.GetIPProperties().UnicastAddresses)
                {
                    Shared.Log("IP found: " + ip.Address.ToString() + ".");
                    ips.Add(ip.Address);
                }
            }
            return ips.ToArray();
        }
        public void Dispose()
        {
            Disposing = true;
            Client.Close();
            Client.Dispose();
        }
    }
}