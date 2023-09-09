using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Progress_Finances_API.Model
{
    public class Metas
    {
        [Key]
        public int Id { get; set; }
        public string NomeMeta { get; set; }
        public int ValorInicial { get; set; }
        public int ValorTotal { get; set; }
        public int ValorMeta { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlvo { get; set; }
        public decimal Porcentagem { get; set; }
        public Status Status { get; set; }

        public virtual ICollection<Items> Items { get; set; }
    }

    public class Items
    {
        [Key]
        public int Id { get; set; }
        public int ValorDepositado { get; set; }
        public DateTime DataDeposito { get; set; }

        // Referência para o objeto pai
        public int IdMeta { get; set; }
        public int ProgressFinanceModelId { get; set; }

        [JsonIgnore]
        public virtual Metas ProgressFinanceModel { get; set; }
    }
}
