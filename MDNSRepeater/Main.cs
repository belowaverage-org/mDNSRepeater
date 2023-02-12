namespace MDNSRepeater
{
    internal static class Program
    {
        #pragma warning disable CS8618
        internal static UnicastSender UnicastSender;
        internal static UnicastReceiver UnicastReceiver;
        internal static MulticastSender MulticastSender;
        internal static MulticastReceiver MulticastReceiver;
        #pragma warning restore CS8618
        internal static void Main()
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