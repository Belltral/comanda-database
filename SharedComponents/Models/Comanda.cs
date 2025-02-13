using System.Collections.Generic;
using System.Text;

namespace SharedComponents.Models
{
    public class Comanda
    {
        public PreVenda PreVenda { get; set; }
        public List<ItensPreVenda> ItensPreVenda { get; set; }

        public Comanda(PreVenda preVenda, List<ItensPreVenda>itens)
        {
            PreVenda = preVenda;
            ItensPreVenda = itens;
        }

        public override string ToString()
        {
            StringBuilder sBuilder = new StringBuilder();

            sBuilder.AppendLine($"Pré Venda: {PreVenda.NUMERO_PRVD}");
            sBuilder.AppendLine($"Comanda: {PreVenda.COMANDA_PRVD}");
            sBuilder.AppendLine($"Cliente: {PreVenda.NOME_PRVD}");
            sBuilder.AppendLine($"Documento: {PreVenda.CPFCNPJ_PRVD}");
            sBuilder.AppendLine($"Valor: R${PreVenda.VALOR_PRVD}");
            sBuilder.AppendLine($"Produtos registrados:");
            foreach (var item in ItensPreVenda)
            {
                sBuilder.AppendLine($"\tProduto: {item.COD_PROD}");
                sBuilder.AppendLine($"\tValor un: R${item.PRECO_IPRV}");
                sBuilder.AppendLine($"\tQuantidade: {item.QTDE_IPRV}");
                sBuilder.AppendLine();

            }
            sBuilder.AppendLine("=======================================================================================");

            return sBuilder.ToString();
        }
    }
}
