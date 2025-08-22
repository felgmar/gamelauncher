using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp
{
    internal class ProcessManager
    {
        internal static string ProcessOutput = string.Empty;
        internal static int ExitCode = -1;

        internal static void CreateProcess(string FileName, string? Arguments)
        {
            try
            {
                ArgumentException.ThrowIfNullOrEmpty(FileName);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }

            Process process = new();

            try
            {
                process.StartInfo.FileName = FileName;
                process.StartInfo.Arguments = !string.IsNullOrEmpty(Arguments) ? Arguments : "";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                ProcessOutput = process.StandardOutput.ReadToEnd();
            }
            catch (ObjectDisposedException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Win32Exception)
            {
                throw;
            }
            catch (PlatformNotSupportedException)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                process.WaitForExit();
                process.Close();
            }
        }
    }
}
