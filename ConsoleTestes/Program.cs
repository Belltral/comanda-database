using System;
using System.IO;
using System.Threading.Tasks;
using ComandaDB;
using SharedComponents.Models;
using Newtonsoft.Json;
using System.Linq;

namespace ConsoleTestes
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            AppDomain.CurrentDomain.SetData("DataDirectory", $@"{baseDirectory}App_Data");


            Console.Write("Server IP: ");
            string serverIp = Console.ReadLine();

            var comandaInstance = string.IsNullOrEmpty(serverIp)? new ComandaService() : new ComandaService(serverIp);

            await ServerConnectionOptions(comandaInstance);
        }

        public static async Task ServerConnectionOptions(ComandaService comandaInstance)
        {

            while (true)
            {
                await Console.Out.WriteLineAsync("1 - GetComanda");
                await Console.Out.WriteLineAsync("2 - GetPreVenda");
                await Console.Out.WriteLineAsync("3 - GetItensComanda");
                await Console.Out.WriteLineAsync("4 - DeleteComanda");
                await Console.Out.WriteLineAsync("5 - DeleteAllComandas");
                await Console.Out.WriteLineAsync("6 - SalvarComanda");
                await Console.Out.WriteLineAsync("7 - AtualizarComanda");
                //await Console.Out.WriteLineAsync("8 - ComandaExistis");
                await Console.Out.WriteLineAsync("99 - Instalar servidor");

                Console.Write("Opção: ");
                string opt = Console.ReadLine();

                switch (opt)
                {
                    case "1":
                        await Console.Out.WriteAsync("Comanda: ");
                        int getComandaNumber = int.Parse(Console.ReadLine());

                        var comanda = await comandaInstance.GetComanda(getComandaNumber);

                        //await SalvarComanda(comanda);

                        if (comanda is null)
                        {
                            await Console.Out.WriteLineAsync("Comanda não encontrada");
                        }
                        else
                        {
                            await Console.Out.WriteLineAsync(comanda.ToString());
                        }

                        Console.ReadKey();
                        break;

                    case "2":
                        await Console.Out.WriteAsync("Comanda: ");
                        int getPreVendaNumber = int.Parse(Console.ReadLine());

                        var preVenda = await comandaInstance.GetPreVenda(getPreVendaNumber);

                        if (preVenda is null)
                        {
                            await Console.Out.WriteLineAsync("Comanda não encontrada");
                        }
                        else
                        {
                            await Console.Out.WriteLineAsync(preVenda.ToString());
                        }

                        Console.ReadKey();
                        break;

                    case "3":
                        await Console.Out.WriteAsync("Comanda: ");
                        int getItensPreVendaNumber = int.Parse(Console.ReadLine());

                        var itensPreVenda = await comandaInstance.GetItensComanda(getItensPreVendaNumber);

                        if (!itensPreVenda.Any())
                        {
                            await Console.Out.WriteLineAsync("Comanda não encontrada");
                        }
                        else
                        {
                            foreach (var item in itensPreVenda)
                            {
                                Console.WriteLine(item.ToString());
                            }
                        }

                        Console.ReadKey();
                        break;

                    case "4":
                        await Console.Out.WriteAsync("Comanda: ");
                        int deleteComandaNumber = int.Parse(Console.ReadLine());

                        var deleteComanda = await comandaInstance.DeleteComanda(deleteComandaNumber);

                        await Console.Out.WriteLineAsync(deleteComanda.ToString());

                        Console.ReadKey();
                        break;

                    case "5":
                        var deleteAllComandas = await comandaInstance.DeleteAllComandas();

                        await Console.Out.WriteLineAsync(deleteAllComandas.ToString());

                        Console.ReadKey();
                        break;

                    case "6":
                        var salvarComanda = await PegarComanda();

                        var comandaSalva = await comandaInstance.CreateComanda(salvarComanda);

                        await Console.Out.WriteLineAsync(comandaSalva.ToString());

                        Console.ReadKey();

                        break;

                    case "7":
                        var comandaAtualizada = await PegarComanda();

                        var atualizarComanda = await comandaInstance.UpdateComanda(comandaAtualizada);

                        await Console.Out.WriteLineAsync(atualizarComanda.ToString());

                        Console.ReadKey();

                        break;

                    //case "8":
                    //    await Console.Out.WriteAsync("Comanda: ");
                    //    int checkComandaNumber = int.Parse(Console.ReadLine());

                    //    var check = await comandaInstance.ComandaExistis(checkComandaNumber);

                    //    await Console.Out.WriteLineAsync(check.ToString());

                    //    Console.ReadKey();
                    //    break;


                    case "99":
                        InstallServerService.Installer();
                        Console.ReadKey();
                        break;
                }
            }


            //await Console.Out.WriteLineAsync("Server");
            //await Task.Run(() => ComandasServer.InitiateServer());
            //// await ComandasServer.InitiateServer();
            //Console.ReadKey();
        }

        public static async Task SalvarComanda(Comanda comanda)
        {
            var comandaJson = JsonConvert.SerializeObject(comanda);

            using (FileStream file = new FileStream("comanda.json", FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(comandaJson);
                    await Console.Out.WriteLineAsync("Salva");
                }
            }
        }

        public static async Task<Comanda> PegarComanda()
        {
            Comanda comanda;

            using (FileStream file = new FileStream("comanda.json", FileMode.Open))
            {
                using (StreamReader sw = new StreamReader(file))
                {
                    comanda = JsonConvert.DeserializeObject<Comanda>(await sw.ReadToEndAsync());
                }
            }

            return comanda;
        }
    }
}