namespace Mundipagg.Connector {

    public sealed class MundipaggSettings : IMundipaggSettings {

        public MundipaggSettings() { }

        public string Authorization { get; set; }

        public string CustomerId { get; set; }
    }
}