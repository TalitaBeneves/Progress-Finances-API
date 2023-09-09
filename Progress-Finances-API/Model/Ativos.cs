using System.ComponentModel.DataAnnotations;

namespace Progress_Finances_API.Model
{
    public class Ativos
    {
        [Key]
        public int ativo_id { get; set; }
        public int usuario_id { get; set; }
        public string Nome { get; set; }
        public int Nota { get; set; }
        public decimal? SugestaoInvestimento { get; set; }
        public string LocalAlocado { get; set; }
        public int QtdAtivos { get; set; }
        public decimal? ValorTotalInvestido { get; set; }
        public decimal ValorAtualDoAtivo { get; set; }
        public TipoAtivo Tipo { get; set; }
        public bool? ChekedParaCalculo { get; set; }
    }
}
