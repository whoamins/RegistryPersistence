using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    internal class WindowsRegistry
    {
        private static RegistryKey registryKey = Registry.CurrentUser;

        public static RegistryKey OpenCurrentUserKey(string pathToSubKey)
        {
            return registryKey.OpenSubKey(pathToSubKey, true); // null?
        }

        public static RegistryKey OpenLocalMachineKey(string pathToSubKey)
        {
            registryKey = Registry.LocalMachine;
            return registryKey.OpenSubKey(pathToSubKey, true); // null?
        }
    }
}
