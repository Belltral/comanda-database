using ComandaDB.Interfaces;
using Newtonsoft.Json;
using SharedComponents;
using SharedComponents.Models;
using SharedComponents.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ComandaDB
{
    public class ComandaService : IComanda
    {
        private readonly string _serverIp = null;
        private readonly Client _client;
        private readonly ComandasHandler _comandas;
        private readonly string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

        public ComandaService(string serverIp)
        {
            _serverIp = serverIp;
            _client = new Client();
            _comandas = new ComandasHandler();
        }

        public ComandaService()
        {
            _client = new Client();
            _comandas = new ComandasHandler();
        }

        
        /// <summary>
        /// Localiza uma comanda no banco do servidor de acordo com o número de comanda fornecido
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a ser retornada</param>
        /// <returns>Retorna uma comanda do banco pelo servidor de comandas</returns>
        public async Task<Comanda> GetComanda(int comandaNumber)
        {
            DatabaseRequestProtocol request = new DatabaseRequestProtocol()
            {
                RequestType = RequestType.DataRequest,
                IsPreVenda = false,
                IsItensPreVendas = false,
                ComandaNumber = comandaNumber,
                Path = dataDirectory
            };

            var comanda = 
                string.IsNullOrEmpty(_serverIp) ? await _comandas.GetComanda(comandaNumber) : await _client.ClientConnection(_serverIp, request);

            return (Comanda)comanda;
        }

        /// <summary>
        /// Retorna uma Pré Venda do banco do servidor de acordo com o número de comanda fornecido
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a qual pertence a Pré Venda</param>
        /// <returns></returns>
        public async Task<PreVenda> GetPreVenda(int comandaNumber)
        {
            DatabaseRequestProtocol request = new DatabaseRequestProtocol
            {
                RequestType = RequestType.DataRequest,
                IsPreVenda = true,
                IsItensPreVendas = false,
                ComandaNumber = comandaNumber,
                Path = dataDirectory
            };

            var preVenda =
                string.IsNullOrEmpty(_serverIp) ? await _comandas.GetPreVenda(comandaNumber) : await _client.ClientConnection(_serverIp, request);

            return (PreVenda)preVenda;
        }

        public async Task<ICollection<PreVenda>> GetPreVendas()
        {
            DatabaseRequestProtocol request = new DatabaseRequestProtocol
            {
                RequestType = RequestType.DataRequest,
                IsPreVenda = true,
                IsItensPreVendas = false,
                Path = dataDirectory
            };

            var preVendas =
                string.IsNullOrEmpty(_serverIp) ? await _comandas.GetPreVendas() : await _client.ClientConnection(_serverIp, request);

            return (ICollection<PreVenda>)preVendas;
        }

        /// <summary>
        /// Retorna os produtos de uma comanda específica do servidor de acordo com o número de comanda
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a qual os produtos fazem parte</param>
        /// <returns></returns>
        public async Task<ICollection<ItensPreVenda>> GetItensComanda(int comandaNumber)
        {
            DatabaseRequestProtocol request = new DatabaseRequestProtocol()
            {
                RequestType = RequestType.DataRequest,
                IsPreVenda = false,
                IsItensPreVendas = true,
                ComandaNumber = comandaNumber,
                Path = dataDirectory
            };

            var itensPreVenda =
                string.IsNullOrEmpty(_serverIp) ? await _comandas.GetItensComanda(comandaNumber) : await _client.ClientConnection(_serverIp, request);

            return (List<ItensPreVenda>)itensPreVenda;
        }

        /// <summary>
        /// Salva uma comanda na base do servidor
        /// </summary>
        /// <param name="comanda">Comanda que será salva</param>
        /// <returns>Retorna verdadeiro caso a comanda seja salva ou falso caso não seja</returns>
        public async Task<bool> CreateComanda(Comanda comanda)
        {
            string comandaJson = JsonConvert.SerializeObject(comanda);

            DatabaseRequestProtocol request = new DatabaseRequestProtocol()
            {
                RequestType = RequestType.DataSave,
                IsPreVenda = false,
                IsItensPreVendas = false,
                Path = dataDirectory
            };

            var isComandaSaved =
                string.IsNullOrEmpty(_serverIp) ? await _comandas.CreateComanda(comanda) : await _client.ClientConnection(_serverIp, request, comandaJson);

            return (bool)isComandaSaved;
        }

        /// <summary>
        /// Faz a atualização de uma comanda existente na base do servidor
        /// </summary>
        /// <param name="comanda">Comanda atualizada a ser salva</param>
        /// <returns>Retorna verdadeiro caso a comanda seja atualizada</returns>
        public async Task<bool> UpdateComanda(Comanda comanda)
        {
            string comandaJson = JsonConvert.SerializeObject(comanda);

            DatabaseRequestProtocol request = new DatabaseRequestProtocol()
            {
                RequestType = RequestType.DataUpdate,
                IsPreVenda = false,
                IsItensPreVendas = false,
                Path = dataDirectory
            };

            var update = 
                string.IsNullOrEmpty(_serverIp) ? await _comandas.UpdateComanda(comanda) :  await _client.ClientConnection(_serverIp, request, comandaJson);

            return (bool)update;
        }

        /// <summary>
        /// Remove uma comanda da base do servidor de acordo com o número
        /// </summary>
        /// <param name="comandaNumber">Número da comanda a ser removida</param>
        /// <returns>Retorna verdadeiro se o procedimento foi efetuado ou falso caso não exista a comanda informada</returns>
        public async Task<bool> DeleteComanda(int comandaNumber)
        {
            DatabaseRequestProtocol request = new DatabaseRequestProtocol()
            {
                RequestType = RequestType.DataDelete,
                IsPreVenda = false,
                IsItensPreVendas = false,
                ComandaNumber = comandaNumber,
                Path = dataDirectory
            };

            var delete =
                string.IsNullOrEmpty(_serverIp) ? await _comandas.DeleteComanda(comandaNumber) : await _client.ClientConnection(_serverIp, request);

            return (bool)delete;
        }

        /// <summary>
        /// Apaga todas as comandas da base do servidor
        /// </summary>
        /// <returns>Retorna verdadeiro caso as comandas forem apagadas</returns>
        public async Task<bool> DeleteAllComandas()
        {
            DatabaseRequestProtocol request = new DatabaseRequestProtocol()
            {
                RequestType = RequestType.DataDeleteAll,
                IsPreVenda = false,
                IsItensPreVendas = false,
                Path = dataDirectory
            };

            var deleteAll =
                string.IsNullOrEmpty(_serverIp) ? await _comandas.DeleteAllComandas() : await _client.ClientConnection(_serverIp, request);

            return (bool)deleteAll;
        }




        /// <summary>
        /// Verifica se uma comanda já existe na base do servidor
        /// </summary>
        /// <param name="comandaNumber">Número da comanda para verificar sua existência</param>
        /// <returns>Retorna verdadeiro caso exista uma comanda com o mesmo número ou falso caso não sejam encontrados dados</returns>
        //public async Task<bool> ComandaExistis(int comandaNumber)
        //{
        //    Request request = new Request()
        //    {
        //        RequestType = RequestType.DataCheck,
        //        IsPreVenda = false,
        //        IsItensPreVendas = false,
        //        ComandaNumber = comandaNumber,
        //        Path = dataDirectory
        //    };

        //    var check = await _client.ClientConnection(_serverIp, request);

        //    return (bool)check;
        //}
    }
}
