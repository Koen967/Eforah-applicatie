namespace Eforah_BetaalApp.Implementation.Models
{
    public class MededelingModel
    {
        public int mededelingId { get; set; }
        public int verenigingId { get; set; }
        public string plaatsingDatum { get; set; }
        public string titel { get; set; }
        public string mededeling { get; set; }
    }
}
