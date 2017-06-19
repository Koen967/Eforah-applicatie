namespace Eforah_BetaalApp.Implementation.Models
{
    public class TransactieModel
    {
        public int transactieId { get; set; }
        public int lidId { get; set; }
        public string transactieDatum { get; set; }
        public decimal bedrag { get; set; }
    }
}
