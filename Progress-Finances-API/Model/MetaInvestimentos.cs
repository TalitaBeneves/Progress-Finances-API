using System.ComponentModel.DataAnnotations;

namespace Progress_Finances_API.Model
{
    public class MetaInvestimentos
    {
        [Key]
        public int Meta_id { get; set; }
        public int Usuario_id { get; set; }
        public string Nome { get; set; }
        public decimal Acoes { get; set; }
        public decimal Fiis { get; set; }
        public decimal Fixa { get; set; }
    }
}
