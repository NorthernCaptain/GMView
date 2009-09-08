using System;
using System.Collections.Generic;
using System.Text;

namespace ncUtils
{
    public class ShellExec
    {
        /// <summary>
        /// Executes a shell command synchronously vi CMD (command line shell).
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>string, as output of the command or Exception object if error occured</returns>
        public static object ExecuteCommandSync(string command, string parameters)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command + " " + parameters);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                // Display the command output.
                return result;
            }
            catch (Exception objException)
            {
                return objException;
            }
        }
        /// <summary>
        /// Executes a shell command synchronously.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>string, as output of the command or Exception object if error occured</returns>
        public static object ExecuteCommandSyncNoCMD(string command, string parameters)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo;

                procStartInfo = new System.Diagnostics.ProcessStartInfo(command, parameters);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                // Display the command output.
                return result;
            }
            catch (Exception objException)
            {
                return objException;
            }
        }
    }
}
