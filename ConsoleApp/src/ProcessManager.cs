using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp
{
    internal sealed class ProcessManager
    {
        internal static string ProcessOutput = string.Empty;
        internal static int ExitCode = -1;

        internal static int CreateProcess(string FileName, string? Arguments)
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
                process.StartInfo.Arguments = string.IsNullOrEmpty(Arguments) ? string.Empty : Arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                ProcessOutput = process.StandardOutput.ReadToEnd();
                ExitCode = process.ExitCode;
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
                process.Close();
            }

            return ExitCode;
        }
    }
}
