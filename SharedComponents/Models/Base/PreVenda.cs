using SharedComponents.Models;
using System;
using System.ComponentModel.DataAnnotations;

public partial class PreVenda
{
    [Key]
    public int NUMERO_PRVD { get; set; }

    public int COMANDA_PRVD { get; set; }

    public int? NUMERO_PDV { get; set; }

    public int? COD_CLI { get; set; }

    [StringLength(60)]
    public string NOME_PRVD { get; set; }

    [StringLength(14)]
    public string CPFCNPJ_PRVD { get; set; }

    [StringLength(15)]
    public string TELEFONE_PRVD { get; set; }

    [StringLength(30)]
    public string CONTATO_PRVD { get; set; }

    [StringLength(160)]
    public string ENDERECO_PRVD { get; set; }

    public int? COD_VEND { get; set; }

    public DateTime? DATA_PRVD { get; set; }

    public DateTime? DATAENTR_PRVD { get; set; }

    public int? QTCLI_PRVD { get; set; }

    public string OBS_PRVD { get; set; }

    public decimal? VALOR_PRVD { get; set; }

    public decimal? ACRESCIMO_PRVD { get; set; }

    public decimal? TXENTR_PRVD { get; set; }

    public decimal? DESCONTO_PRVD { get; set; }

    public decimal? DESCPRMN_PRVD { get; set; }

    public decimal? DESCPRAU_PRVD { get; set; }

    [StringLength(1)]
    public string UNICAPR_PRVD { get; set; }

    public int? ID_PROM_PRVD { get; set; }

    public int? QTDEPROM_PRVD { get; set; }

    public int? ID_OPER_AUT { get; set; }

    public int? COD_ENTR { get; set; }

    [StringLength(1)]
    public string SITUACAO_PRVD { get; set; }

    [StringLength(150)]
    public string COMANDAMESCLADA_PRVD { get; set; }

    public int? ID_DSMV { get; set; }

    [StringLength(1)]
    public string IMPRESSAO_PRVD { get; set; }

    [StringLength(1)]
    public string IMPCOMPLETA_PRVD { get; set; }

    public int? COD_CLI_LOCAL { get; set; }

    [StringLength(1)]
    public string ECOMMERCE_PRVD { get; set; }

    public DateTime? DATAVISUALIZACAO_PRVD { get; set; }

    public int? ID_PRVD_NUVEM { get; set; }

    [StringLength(60)]
    public string ENDERECO_PRVD_NFE { get; set; }

    [StringLength(10)]
    public string NUMEROEND_PRVD_NFE { get; set; }

    [StringLength(10)]
    public string COMPL_PRVD_NFE { get; set; }

    [StringLength(20)]
    public string BAIRRO_PRVD_NFE { get; set; }

    [StringLength(7)]
    public string IBGECIDADE_PRVD_NFE { get; set; }

    [StringLength(20)]
    public string CIDADE_PRVD_NFE { get; set; }

    [StringLength(2)]
    public string IBGEESTADO_PRVD_NFE { get; set; }

    [StringLength(2)]
    public string ESTADO_PRVD_NFE { get; set; }

    [StringLength(8)]
    public string CEP_PRVD_NFE { get; set; }

    public int? CANAL_PRVD { get; set; }

    [StringLength(1)]
    public string TODOSITENS_PRVD { get; set; }

    [StringLength(1)]
    public string CUPOMTOTEM_PRVD { get; set; }

    public int? COD_LISTA { get; set; }

    public int? NUMEROVENDATOTEM_PRVD { get; set; }

    [StringLength(44)]
    public string CHAVEVENDA_PRVD { get; set; }

    [StringLength(50)]
    public string IDIFOOD_PRVD { get; set; }

    [StringLength(50)]
    public string CORRELATIONID_PRVD { get; set; }

    [StringLength(1)]
    public string IFOOD_PRVD { get; set; }

    [StringLength(1)]
    public string STATUSIFOOD_PRVD { get; set; }

    [StringLength(40)]
    public string TIPOIFOOD_PRVD { get; set; }

    [StringLength(30)]
    public string BENEFICIOIFOOD_PRVD { get; set; }

    [StringLength(25)]
    public string RETIRARPEDIDO_PRVD { get; set; }

    public int? ID_PDEC { get; set; }

    public int? ID_TERC { get; set; }

    [StringLength(1)]
    public string STATUSRE_PRVD { get; set; }

    public int? CRE_PRVD { get; set; }

    public int? CCF_PRVD { get; set; }

    [StringLength(1)]
    public string DESCRATEADO { get; set; }

    public int? MESA_PRVD { get; set; }

    public string PEDIDOKDS_PRVD { get; set; }

    [StringLength(15)]
    public string PLATAFORMA_PDEC { get; set; }

    public decimal? QUILOMETRAGEM_PRVD { get; set; }

    public override string ToString()
    {
        return $"Número da pré venda: {NUMERO_PRVD}\n" +
            $"Número da comanda: {COMANDA_PRVD}\n" +
            $"Cliente: {NOME_PRVD}\n" +
            $"Valor: {VALOR_PRVD}\n" +
            $"=============================================================================";
    }
}
