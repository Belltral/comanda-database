using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Text;
using Newtonsoft.Json;
using SharedComponents.Requests;
using SharedComponents.Models;
using SharedComponents.Utils;
using SharedComponents;

namespace ComandasServerService.ServerComponents
{
    internal class ServerListener
    {
        //public TcpListener Listener { get; set; }
        //public bool Running { get; set; }

        private static TcpListener _listener = null;
        private static bool running;
        private static byte[] dataToSend;

        private static readonly ComandasHandler _comandas = new ComandasHandler(@"D:\Estudos\C#-.Net\ComandasDB\ComandasDB\ConsoleTestes\bin\Debug\App_Data");

        private static async Task InformBufferSize(NetworkStream stream, DatabaseRequestProtocol request)
        {
            try
            {
                if (request.IsPreVenda)
                {
                    // Localizar Pre Venda, serializar e encodar
                    var preVenda = await _comandas.GetPreVenda(request.ComandaNumber);
                    string preVendaJson = JsonConvert.SerializeObject(preVenda);
                    dataToSend = Encoding.UTF8.GetBytes(preVendaJson);

                    string preVendaBufferSize = dataToSend.Length.ToString();
                    byte[] preVendaBuffer = Encoding.UTF8.GetBytes(preVendaBufferSize);

                    await stream.WriteAsync(preVendaBuffer, 0, preVendaBuffer.Length);

                    return;
                }

                else if (request.IsItensPreVendas)
                {
                    var itensPreVendas = await _comandas.GetItensComanda(request.ComandaNumber);
                    string itensJson = JsonConvert.SerializeObject(itensPreVendas);
                    dataToSend = Encoding.UTF8.GetBytes(itensJson);

                    string itensBufferSize = dataToSend.Length.ToString();
                    byte[] itensBuffer = Encoding.UTF8.GetBytes(itensBufferSize);

                    await stream.WriteAsync(itensBuffer, 0, itensBuffer.Length);

                    return;
                }

                Comanda comanda = await _comandas.GetComanda(request.ComandaNumber);
                string comandaJson = JsonConvert.SerializeObject(comanda);
                dataToSend = Encoding.UTF8.GetBytes(comandaJson);

                string comandaBufferSize = dataToSend.Length.ToString();
                byte[] comandaBuffer = Encoding.UTF8.GetBytes(comandaBufferSize);

                await stream.WriteAsync(comandaBuffer, 0, comandaBuffer.Length);

            }
            catch (JsonException e)
            {
                Logger.Log($"{e.Message}\n{e.Source}");
            }
            catch (SocketException e)
            {
                Logger.Log($"{e.Message}\n{e.Source}");
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);

                Logger.Log($"{e.Message}\n{e.Source}");
            }

        }

        internal static async Task ServerConnection()
        {
            LoggerUtil.Log("Inciando servidor...");

            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 8130);
                _listener = new TcpListener(iPEndPoint);

                _listener.Start();
                running = true;

                while (running)
                {
                    using (TcpClient handler = await _listener.AcceptTcpClientAsync())
                    {
                        await Task.Run(async () =>
                        {

                            using (NetworkStream stream = handler.GetStream())
                            {
                                try
                                {
                                    // Ler dados recebidos
                                    var buffer = new byte[1024];
                                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                                    string requestReceived = Encoding.UTF8.GetString(buffer);
                                    DatabaseRequestProtocol request = JsonConvert.DeserializeObject<DatabaseRequestProtocol>(requestReceived);



                                    switch (request.RequestType)
                                    {
                                        case RequestType.DataRequest:
                                            await InformBufferSize(stream, request);

                                            // Aguarda pela confirmação de alocação de memória
                                            var clientSavedBuffer = new byte[512];
                                            await stream.ReadAsync(clientSavedBuffer, 0, clientSavedBuffer.Length);
                                            var clientAck = Encoding.UTF8.GetString(clientSavedBuffer);

                                            // Envia os dados
                                            await stream.WriteAsync(dataToSend, 0, dataToSend.Length);
                                            break;

                                        case RequestType.DataSave:
                                            var saveBuffer = new byte[request.BufferSize];

                                            // Envia a confirmação para o cliente
                                            byte[] saveAckSend = Encoding.UTF8.GetBytes("ACK");
                                            await stream.WriteAsync(saveAckSend, 0, saveAckSend.Length);

                                            // Recebe os dados para serem salvos
                                            await stream.ReadAsync(saveBuffer, 0, saveBuffer.Length);

                                            var decodedComandaSave = Encoding.UTF8.GetString(saveBuffer);
                                            Comanda comandaToSave = JsonConvert.DeserializeObject<Comanda>(decodedComandaSave);

                                            var isComandaSaved = await _comandas.CreateComanda(comandaToSave);
                                            var isComandaSavedEncoded = Encoding.UTF8.GetBytes(isComandaSaved.ToString());
                                            await stream.WriteAsync(isComandaSavedEncoded, 0, isComandaSavedEncoded.Length);

                                            break;

                                        case RequestType.DataUpdate:
                                            var updateBuffer = new byte[request.BufferSize];

                                            byte[] updateAckSend = Encoding.UTF8.GetBytes("ACK");
                                            await stream.WriteAsync(updateAckSend, 0, updateAckSend.Length);

                                            await stream.ReadAsync(updateBuffer, 0, updateBuffer.Length);

                                            var decodedComandaUpdate = Encoding.UTF8.GetString(updateBuffer);
                                            Comanda comandaToUpdate = JsonConvert.DeserializeObject<Comanda>(decodedComandaUpdate);

                                            bool updatedComandaCheck = await _comandas.UpdateComanda(comandaToUpdate);
                                            var encodedUpdateCheck = Encoding.UTF8.GetBytes(updatedComandaCheck.ToString());
                                            await stream.WriteAsync(encodedUpdateCheck, 0, encodedUpdateCheck.Length);

                                            break;

                                        case RequestType.DataDelete:
                                            bool deletedComandas = await _comandas.DeleteComanda(request.ComandaNumber);
                                            var encodedDRespose = Encoding.UTF8.GetBytes(deletedComandas.ToString());
                                            await stream.WriteAsync(encodedDRespose, 0, encodedDRespose.Length);

                                            break;

                                        case RequestType.DataDeleteAll:
                                            bool deletedAllComandas = await _comandas.DeleteAllComandas();
                                            var encodedDaRespose = Encoding.UTF8.GetBytes(deletedAllComandas.ToString());
                                            await stream.WriteAsync(encodedDaRespose, 0, encodedDaRespose.Length);

                                            break;

                                        //case RequestType.DataCheck:
                                        //    bool dataCheck = await _comandas.ComandaExistis(request.ComandaNumber);
                                        //    var encodedCheckRespose = Encoding.UTF8.GetBytes(dataCheck.ToString());
                                        //    await stream.WriteAsync(encodedCheckRespose, 0, encodedCheckRespose.Length);
                                        //    break;
                                    }
                                }
                                catch (SocketException e)
                                {
                                    Logger.Log($"{e.Message}\n{e.Source}");

                                }
                                catch (JsonException e)
                                {
                                    Logger.Log($"{e.Message}\n{e.Source}");
                                }
                                catch (Exception e)
                                {
                                    Logger.Log($"{e.Message}\n{e.Source}");
                                }
                            }

                        });
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log($"{e.Message}\n{e.Source}");
            }
        }

        internal static void StopServer()
        {
            if (_listener != null)
            {
                _listener.Stop();
                running = false;
            }
        }
    }
}
