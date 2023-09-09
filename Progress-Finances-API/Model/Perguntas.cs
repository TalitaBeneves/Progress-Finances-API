using System.ComponentModel.DataAnnotations;

namespace Progress_Finances_API.Model
{
    public class Perguntas
    {
        [Key]
        public int Pergunta_Id { get; set; }
        public int Usuario_Id { get; set; }
        public string Pergunta { get; set; }
        public TipoAtivoParaPergunta Tipo { get; set; }
        public bool? Ativo { get; set; }
    }
}
