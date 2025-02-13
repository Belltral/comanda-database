using SharedComponents.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComandaDB.Interfaces
{
    internal interface IComanda
    {
        Task<Comanda> GetComanda(int comandaNumber);
        Task<PreVenda> GetPreVenda(int comandaNumber);
        Task<ICollection<ItensPreVenda>> GetItensComanda(int comandaNumber);
        Task<bool> CreateComanda(Comanda comanda);
        Task<bool> UpdateComanda(Comanda comanda);
        Task<bool> DeleteComanda(int comandaNumber);
        Task<bool> DeleteAllComandas();
    }
}
