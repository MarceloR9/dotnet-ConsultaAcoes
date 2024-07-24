namespace ConsultaAcoes.Models
{
    public class AcaoViewModel
    {
        public IEnumerable<Acao> Acoes { get; set; }
        public IEnumerable<Acao> TopValorizadas { get; set; }
        public IEnumerable<Acao> TopDesvalorizadas { get; set; }
        public string Search { get; set; }
    }
}
