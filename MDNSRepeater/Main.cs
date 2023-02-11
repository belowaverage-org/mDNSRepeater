namespace MDNSRepeater
{
    internal static class Program
    {
        internal static UnicastSender UnicastSender;
        internal static UnicastReceiver UnicastReceiver;
        internal static MulticastSender MulticastSender;
        internal static MulticastReceiver MulticastReceiver;
        internal static void Main(string[] args)
        {
            Shared.Log("Starting mDNS repeater...");
            UnicastSender = new UnicastSender();
            UnicastReceiver = new UnicastReceiver();
            MulticastSender = new MulticastSender();
            MulticastReceiver = new MulticastReceiver();
            Shared.Log("mDNS repeater started.");
            Thread.Sleep(-1);
        }
    }
}