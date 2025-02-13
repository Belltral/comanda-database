using System;
using System.Diagnostics;

namespace ComandaDB
{
    public static class InstallServerService
    {
        public static void Installer()
        {
            var process = new Process()
            {
                StartInfo =
                {
                    FileName = "InstallServerService.exe"
                }
            };
            try
            {
                process.Start();

                process.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
