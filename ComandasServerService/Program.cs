using System.ServiceProcess;

namespace ComandasServerService
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServerService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
