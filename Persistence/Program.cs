using Microsoft.Win32;
using System;

namespace Persistence 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Persistence persistence = new();
            persistence.ExecProgAndCommand("Acrobat.exe", "calc.exe");
        }
    }
}