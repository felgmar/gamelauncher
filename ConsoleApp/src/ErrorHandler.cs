using System;

namespace ConsoleApp
{
    internal class ErrorHandler
    {
        internal static void WaitAndExit(Exception exception)
        {
            if (string.IsNullOrWhiteSpace(exception.Message))
            {
                Console.WriteLine("\n[ERROR] An unknown error occurred.");
            }
            else
            {
                Console.WriteLine("\n[ERROR] " + exception.Message);
            }

            Console.WriteLine("Press the ENTER key to close this program.");
            Console.ReadLine();
            Environment.Exit(1);
        }
    }
}
