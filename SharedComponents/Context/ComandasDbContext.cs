using System;
using System.Data.Entity;
using System.IO;

namespace SharedComponents.Context
{
    public partial class ComandasDbContext : DbContext
    {
        public ComandasDbContext(string path = null) : base()
        {
            string dataDirectory = path ?? AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

            if (!File.Exists($@"{dataDirectory}\ComandasPDV.MDF"))
            {
                try
                {
                    Database.SetInitializer(new CreateDatabaseIfNotExists<ComandasDbContext>());
                    Database.Initialize(false);

                    //if (File.Exists($@"{dataDirectory}\ComandasPDV.MDF"))
                    //{
                    //    // RetrieveFromPDV.RetriveFromPDVToComandas();
                    //}
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }

            this.Database.Connection.ConnectionString = $@"data source=(localdb)\MSSQLLocalDB;attachdbfilename={dataDirectory}\ComandasPDV.MDF;integrated security=True;connect timeout=30;MultipleActiveResultSets=True";
        }

        public virtual DbSet<ItensPreVenda> ItensPreVendas { get; set; }
        public virtual DbSet<PreVenda> PreVendas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItensPreVenda>()
                .Property(e => e.QTDE_IPRV)
                .HasPrecision(10, 3);

            modelBuilder.Entity<ItensPreVenda>()
                .Property(e => e.PRECO_IPRV)
                .HasPrecision(12, 3);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.VALOR_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.ACRESCIMO_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.TXENTR_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.DESCONTO_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.DESCPRMN_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.DESCPRAU_PRVD)
                .HasPrecision(12, 2);

            modelBuilder.Entity<PreVenda>()
                .Property(e => e.PEDIDOKDS_PRVD)
                .IsUnicode(false);
        }
    }
}
