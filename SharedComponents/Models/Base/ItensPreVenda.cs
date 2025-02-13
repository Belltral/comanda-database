using SharedComponents.Models;
using System;
using System.ComponentModel.DataAnnotations;

public partial class ItensPreVenda
{
    [Key]
    public int ID_IPRV { get; set; }

    public int NUMERO_PRVD { get; set; }

    public int? ITEM_IPRV { get; set; }

    public int? ID_PRVL { get; set; }

    public decimal? QTDE_IPRV { get; set; }

    public decimal? PRECO_IPRV { get; set; }

    public int? ID_PROM { get; set; }

    public int? QTDEPROM_IPRV { get; set; }

    public int? ITEMPROM_IPRV { get; set; }

    public decimal? DESCPROM_IPRV { get; set; }

    public decimal? DESCONTO_IPRV { get; set; }

    [StringLength(1)]
    public string UNICAPR_IPRV { get; set; }

    [StringLength(40)]
    public string OBS_IPRV { get; set; }

    public DateTime? DATAHORA_IPRV { get; set; }

    public int? ID_OPER_AUT { get; set; }

    public int? COD_VEND { get; set; }

    [StringLength(1)]
    public string SITUACAO_IPRV { get; set; }

    [StringLength(1)]
    public string IMPRESSAO_IPRV { get; set; }

    public int? COD_PROD { get; set; }

    public int? ID_VLPR01 { get; set; }

    public int? ID_VLPR02 { get; set; }

    public int? ID_VLPR03 { get; set; }

    public int? ID_ITPV_NUVEM { get; set; }

    public DateTime? DATACOM_IPRV { get; set; }

    [StringLength(80)]
    public string DESCRICAO_IPRV { get; set; }

    public decimal? ACRESCIMO_IPRV { get; set; }

    [StringLength(1)]
    public string ADICIONAL_IPRV { get; set; }

    public override string ToString()
    {
        return $"Produto: {COD_PROD}\n" +
            $"Percence a pré venda {NUMERO_PRVD}\n" +
            $"=============================================================================";
    }
}

