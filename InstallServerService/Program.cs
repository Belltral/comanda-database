using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text.RegularExpressions;
using System.Threading;

namespace InstallServerService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Instalando serviço...");
            bool installServer = Installer();

            if (installServer)
                Console.WriteLine("O serviço do servidor de comandas foi instalado.");

            Console.Write("Pressione qualquer tecla para fechar esta janela...");
            Console.ReadKey();
        }

        public static bool Installer()
        {
            string windir = Environment.GetEnvironmentVariable("WINDIR") ?? "";

            string installUtilPath = GetInstallUtilPath() + "\\InstallUtil.exe";

            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "cmd",
                        WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                        Arguments = $@"/C {installUtilPath} ComandasServerService.exe", // /C is mandatory to carry the commando to cmd prompt
                        //RedirectStandardError = true,
                        //RedirectStandardOutput = true,
                        UseShellExecute = false
                    }
                };

                process.Start();

                //var error = process.StandardError.ReadToEnd();
                //var standardOutput = process.StandardOutput.ReadToEnd();

                //Console.WriteLine(error);
                //Console.WriteLine();
                //Console.WriteLine(standardOutput);

                process.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var windowsServices = ServiceController.GetServices();

            foreach (var service in windowsServices)
            {
                if (service.ServiceName == "ComandasServerService")
                {
                    service.Start();

                    return true;
                }
            }

            return false;
        }

        private static string GetInstallUtilPath()
        {
            string windir = Environment.GetEnvironmentVariable("WINDIR") ?? "";

            string[] directories;

            if (Directory.Exists($@"{windir}\Microsoft.NET\Framework64\"))
                directories = Directory.GetDirectories($@"{windir}\Microsoft.NET\Framework64\");
            else
                directories = Directory.GetDirectories($@"{windir}\Microsoft.NET\Framework\");

            string frameworkPath = string.Empty;

            foreach (var directory in directories)
            {
                int lastSlashIndex = directory.LastIndexOf('\\');

                if (Regex.IsMatch(directory.Substring(lastSlashIndex + 1), @"^v4\..+|^v4.+"))
                    frameworkPath = directory;
            }

            return frameworkPath;
        }
    }
}
