﻿using System.ComponentModel.DataAnnotations;

namespace Progress_Finances_API.Model
{
    public class Ativos
    {
        [Key]
        public int IdAtivo { get; set; }
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public int Nota { get; set; }
        public decimal? SugestaoInvestimento { get; set; }
        public string LocalAlocado { get; set; }
        public decimal QtdAtivos { get; set; }
        public decimal? ValorTotalInvestido { get; set; }
        public decimal ValorAtualDoAtivo { get; set; }
        public TipoAtivo Tipo { get; set; }
        public bool? ChekedParaCalculo { get; set; }

        public List<PerguntasChecked>? PerguntasMarcadas { get; set; }
    }

    public class PerguntasChecked
    {
        [Key]
        public int IdPergunta { get; set; }
        public string Descricao { get; set; }
        public bool Checked { get; set; }
        public int AtivoId { get; set; }
        public Ativos Ativo { get; set; }
    }
}
