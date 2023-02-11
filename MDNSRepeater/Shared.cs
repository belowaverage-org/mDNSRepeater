namespace MDNSRepeater
{
    internal static class Shared
    {
        public static void Log(string Text)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:MM:ss.fff") + ": " + Text);
        }
    }
}