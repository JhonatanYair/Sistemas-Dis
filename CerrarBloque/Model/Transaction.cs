namespace CerrarBloque.Model
{
    public class Transaction
    {

        public int Id { get; set; }
        public Client Receiver { get; set; }
        public Client? Sender { get; set; }
        public Chain Chain { get; set; }
        public string Description { get; set; }

    }
}
