using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;
using SharedComponents.Requests;
using Newtonsoft.Json;
using SharedComponents.Models;

namespace ComandaDB
{
    internal class Client
    {
        internal async Task<object> ClientConnection(string ipAddress, DatabaseRequestProtocol request, string comandaJson = null)
        {
            if (!(comandaJson is null))
            {
                request.BufferSize = Encoding.UTF8.GetBytes(comandaJson).Length;
            }

            int portNumber = 8130;

            using (TcpClient client = new TcpClient())
            {
                try
                {
                    await client.ConnectAsync(IPAddress.Parse(ipAddress), portNumber);

                    using (NetworkStream stream = client.GetStream())
                    {
                        string sendRequest = JsonConvert.SerializeObject(request);
                        byte[] requestToSend = Encoding.UTF8.GetBytes(sendRequest);

                        switch (request.RequestType)
                        {
                            case RequestType.DataRequest:
                                await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                                // Resposta da solicitação de buffer para o servidor
                                var requestSizeBuffer = new byte[1024];
                                int bytesSizeRead = await stream.ReadAsync(requestSizeBuffer, 0, requestSizeBuffer.Length);

                                int requestSize = int.Parse(Encoding.UTF8.GetString(requestSizeBuffer));

                                var bufferToReceiveRequest = new byte[requestSize];

                                // Envia a confirmação para o servidor
                                byte[] ackSend = Encoding.UTF8.GetBytes("ACK");
                                await stream.WriteAsync(ackSend, 0, ackSend.Length);

                                // Recebe a requisição e faz a desserialização
                                int data = await stream.ReadAsync(bufferToReceiveRequest, 0, bufferToReceiveRequest.Length);
                                string requestReceived = Encoding.UTF8.GetString(bufferToReceiveRequest);

                                if (request.IsPreVenda)
                                {
                                    PreVenda preVenda = JsonConvert.DeserializeObject<PreVenda>(requestReceived);

                                    return preVenda;
                                }

                                else if (request.IsItensPreVendas)
                                {
                                    List<ItensPreVenda> itensPreVenda = JsonConvert.DeserializeObject<List<ItensPreVenda>>(requestReceived);

                                    return itensPreVenda;
                                }

                                Comanda comandaDeserialized = JsonConvert.DeserializeObject<Comanda>(requestReceived);

                                return comandaDeserialized;

                            case RequestType.DataSave:
                                await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                                // Aguarda o servidor responder que armazenou o buffer
                                var serverSavedBuffer = new byte[1024];
                                await stream.ReadAsync(serverSavedBuffer, 0, serverSavedBuffer.Length);
                                var serverAck = Encoding.UTF8.GetString(serverSavedBuffer);

                                // Envia os dados a serem salvos
                                var comandaBuffer = Encoding.UTF8.GetBytes(comandaJson);
                                await stream.WriteAsync(comandaBuffer, 0, comandaBuffer.Length);

                                // Aguarda a resposta da requisição
                                var savedBuffer = new byte[1024];
                                await stream.ReadAsync(savedBuffer, 0, savedBuffer.Length);
                                bool isComandaSaved = bool.Parse(Encoding.UTF8.GetString(savedBuffer));

                                return isComandaSaved;

                            case RequestType.DataUpdate:
                                await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                                var serverUpdateBufferAck = new byte[1024];
                                await stream.ReadAsync(serverUpdateBufferAck, 0, serverUpdateBufferAck.Length);
                                var serverUpdateAck = Encoding.UTF8.GetString(serverUpdateBufferAck);

                                var updateComandaBuffer = Encoding.UTF8.GetBytes(comandaJson);
                                await stream.WriteAsync(updateComandaBuffer, 0, updateComandaBuffer.Length);

                                var updateBuffer = new byte[1024];
                                await stream.ReadAsync(updateBuffer, 0, updateBuffer.Length);
                                bool updateComandaResponse = bool.Parse(Encoding.UTF8.GetString(updateBuffer));

                                return updateComandaResponse;

                            case RequestType.DataDelete:
                                await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                                var deleteBuffer = new byte[1024];
                                await stream.ReadAsync(deleteBuffer, 0, deleteBuffer.Length);
                                var deleteResponse = bool.Parse(Encoding.UTF8.GetString(deleteBuffer));

                                return deleteResponse;

                            case RequestType.DataDeleteAll:
                                await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                                var deleteAllBuffer = new byte[1024];
                                await stream.ReadAsync(deleteAllBuffer, 0, deleteAllBuffer.Length);
                                var deleteAllResponse = bool.Parse(Encoding.UTF8.GetString(deleteAllBuffer));

                                return deleteAllResponse;

                            //case RequestType.DataCheck:
                            //    await stream.WriteAsync(requestToSend, 0, requestToSend.Length);

                            //    var checkBuffer = new byte[1024];
                            //    await stream.ReadAsync(checkBuffer, 0, checkBuffer.Length);
                            //    var checkResponse = bool.Parse(Encoding.UTF8.GetString(checkBuffer));

                            //    return checkResponse;
                        }
                    }
                }
                catch (SocketException e)
                {
                    await Console.Out.WriteLineAsync(e.Message);
                    await Console.Out.WriteLineAsync(e.StackTrace);
                }
                catch (JsonException e)
                {
                    await Console.Out.WriteLineAsync(e.Message);
                    await Console.Out.WriteLineAsync(e.StackTrace);
                }
                catch (Exception e)
                {
                    await Console.Out.WriteLineAsync(e.Message);
                    await Console.Out.WriteLineAsync(e.StackTrace);
                    await Console.Out.WriteLineAsync(e.Source);
                }
                return null;
            }

        }
    }
}
