namespace QuorumCodingChallenge.Domain.DTO
{
    public class BillDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public int supporter_count { get; set; }
        public int opposer_count { get; set; }
        public string primary_sponsor { get; set; }
    }
}
