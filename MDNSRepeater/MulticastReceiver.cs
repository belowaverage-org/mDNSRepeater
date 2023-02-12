using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MDNSRepeater
{
    internal class MulticastReceiver : IDisposable
    {
        bool Disposing = false;
        UdpClient Client;
        IPEndPoint Endpoint = new IPEndPoint(IPAddress.Any, 5353);
        IPAddress[] SelfAddresses;
        internal MulticastReceiver()
        {
            Shared.Log("Setting up MCR...");
            ListAdapterIPs();
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
        void ListAdapterIPs()
        {
            Shared.Log("Enumerating assigned IPs...");
            List<IPAddress> ips = new List<IPAddress>();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface inter in interfaces)
            {
                foreach (UnicastIPAddressInformation ip in inter.GetIPProperties().UnicastAddresses)
                {
                    Shared.Log("IP found: " + ip.Address.ToString() + ".");
                    ips.Add(ip.Address);
                }
            }
            SelfAddresses = ips.ToArray();
        }
        public void Dispose()
        {
            Disposing = true;
            Client.Close();
            Client.Dispose();
        }
    }
}