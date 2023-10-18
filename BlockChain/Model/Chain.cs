namespace BlockChain.Model
{
    public class Chain
    {
        public int Nonce { get; set; }
        public float Data { get; set; }
        public Guid Hash { get; set; }
        public Guid Prev { get; set; }
    }
}
