namespace ConsultaAcoes.Models
{
    public class Acao
    {
        public string Stock { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double? Close { get; set; }
        public double? Change { get; set;}
        public double? Volume { get; set; }
        public double? MarketCap { get; set; }
        public string Logo { get; set; }
        public string Sector { get; set; }
        public string Type { get; set; }
    }
}
