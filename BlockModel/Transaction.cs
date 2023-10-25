namespace BlockModel.Model
{
    public class TransactionChain
    {

        public int? Id { get; set; }
        public Client? Receiver { get; set; }
        public Client? Sender { get; set; }
        public float? Money { get; set; }
        public string? Description { get; set; }

    }

    public class TransactionDTO
    {

        public Client? Receiver { get; set; }
        public Client? Sender { get; set; }
        public float? Money { get; set; }
        public string? Description { get; set; }

    }
}
