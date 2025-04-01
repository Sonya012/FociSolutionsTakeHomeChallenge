﻿using FociSolutionsTakeHomeChallenge.Interfaces;

namespace FociSolutionsTakeHomeChallenge.Services
{
    public class SystemConsole : IConsole
    {
        public void WriteLine(string message) => Console.WriteLine(message);
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
