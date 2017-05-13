using System;
using IoCContainerFunApp.Dependencies.Interfaces;

namespace IoCContainerFunApp.Dependencies.Implementations
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"Logging message: {message}");
        }
    }
}
