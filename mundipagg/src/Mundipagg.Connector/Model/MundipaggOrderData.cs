namespace Mundipagg.Connector.Model {

    public sealed class MundipaggOrderData {

        public MundipaggOrderData() { }

        public long AmountInCents { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }
    }

}