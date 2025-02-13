using System;
using System.Linq;
using System.Collections.Generic;
using SharedComponents.Models;
using SharedComponents.Context;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SharedComponents
{
    public class ComandasHandler
    {
        //private readonly ComandasDbContext _context;

        //public ComandasHandler(ComandasDbContext context)
        //{
        //    _context = context;
        //}

        private readonly string DataPath;

        public ComandasHandler(string dataPath)
        {
            DataPath = dataPath;
        }

        public ComandasHandler() { }

        public async Task<Comanda> GetComanda(int comandaNumber)
        {
            using (var db = new ComandasDbContext(DataPath))
            {
                var queryPreVenda = await db.PreVendas.FirstOrDefaultAsync(p => p.COMANDA_PRVD == comandaNumber);

                if (queryPreVenda is null)
                {
                    return null;
                }

                var numeroPreVenda = queryPreVenda.NUMERO_PRVD;
                var queryItens = await db.ItensPreVendas.Select(i => i).Where(i => i.NUMERO_PRVD == numeroPreVenda).ToListAsync();

                return new Comanda(queryPreVenda, queryItens);
            }
        }

        public async Task<bool> CreateComanda(Comanda comanda)
        {
            if (comanda is null)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasDbContext(DataPath))
            {
                int comandaNumber = comanda.PreVenda.COMANDA_PRVD;

                db.PreVendas.Add(comanda.PreVenda);
                await db.SaveChangesAsync();

                var preVendaNumber = db.PreVendas.SingleOrDefault(pv => pv.COMANDA_PRVD == comandaNumber).NUMERO_PRVD;

                foreach (var item in comanda.ItensPreVenda)
                {
                    item.NUMERO_PRVD = preVendaNumber;
                }

                db.ItensPreVendas.AddRange(comanda.ItensPreVenda);

                await db.SaveChangesAsync();

                //await Task<bool> isComandaSaved = ComandaExistis(comandaNumber);

                return true;
            }
        }

        public async Task<bool> UpdateComanda(Comanda updatedComanda)
        {
            if (updatedComanda is null)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasDbContext(DataPath))
            {
                db.Entry(updatedComanda.PreVenda).State = EntityState.Modified;

                var oldItems = db.ItensPreVendas.Select(i => i).Where(i => i.NUMERO_PRVD == updatedComanda.PreVenda.NUMERO_PRVD);
                db.ItensPreVendas.RemoveRange(oldItems);

                foreach (var item in updatedComanda.ItensPreVenda)
                {
                    item.NUMERO_PRVD = updatedComanda.PreVenda.NUMERO_PRVD;
                }

                db.ItensPreVendas.AddRange(updatedComanda.ItensPreVenda);

                await db.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> DeleteComanda(int comandaNumber)
        {
            if (comandaNumber <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasDbContext(DataPath))
            {
                var preVenda = await db.PreVendas.SingleOrDefaultAsync(p => p.COMANDA_PRVD == comandaNumber);
                var itensComanda = db.ItensPreVendas.Select(i => i).Where(p => p.NUMERO_PRVD == preVenda.NUMERO_PRVD);

                if (preVenda is null)
                {
                    return false;
                }

                db.PreVendas.Remove(preVenda);
                db.ItensPreVendas.RemoveRange(itensComanda);
                await db.SaveChangesAsync();

                return true;
            }
        }
        
        public async Task<bool> DeleteAllComandas()
        {
            using (var db = new ComandasDbContext(DataPath))
            {
                string deleteItensPreVendas = "DELETE FROM ItensPreVendas;";
                await db.Database.ExecuteSqlCommandAsync(deleteItensPreVendas);

                string deletePreVendas = "DELETE FROM PreVendas;";
                await db.Database.ExecuteSqlCommandAsync(deletePreVendas);

                await db.SaveChangesAsync();

                var checkDelete = db.PreVendas.Select(p => p);

                if (!checkDelete.Any())
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<PreVenda> GetPreVenda(int comandaNumber)
        {
            if (comandaNumber <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasDbContext(DataPath))
            {
                var selectPreVenda = await db.PreVendas.SingleOrDefaultAsync(p => p.COMANDA_PRVD == comandaNumber);

                return selectPreVenda;
            }
        }

        public async Task<ICollection<PreVenda>> GetPreVendas()
        {
            using (var db = new ComandasDbContext(DataPath))
            {
                var selectPreVenda = await db.PreVendas.Select(s => s).ToListAsync();

                return selectPreVenda;
            }
        }

        public async Task<ICollection<ItensPreVenda>> GetItensComanda(int comandaNumber)
        {
            if (comandaNumber <= 0)
            {
                throw new ArgumentException();
            }

            using (var db = new ComandasDbContext(DataPath))
            {
                var numeroPreVenda = await db.PreVendas.FirstOrDefaultAsync(pv => pv.COMANDA_PRVD == comandaNumber);

                var items = await db.ItensPreVendas.Select(s => s).Where(n => n.NUMERO_PRVD == numeroPreVenda.NUMERO_PRVD).ToListAsync();

                return items;
            }
        }
    }
}