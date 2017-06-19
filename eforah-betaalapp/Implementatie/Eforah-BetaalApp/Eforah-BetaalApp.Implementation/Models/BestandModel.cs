namespace Eforah_BetaalApp.Implementation.Models
{
    public class BestandModel
    {
        public int bestandId { get; set; }
        public int verenigingId { get; set; }
        public string url { get; set; }
        public string docType { get; set; }
        public string docNaam { get; set; }
        public int docGrootte { get; set; }
    }
}
