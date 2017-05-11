namespace Mundipagg.Connector.Model {

    public sealed class MundipaggAddressData {

        public MundipaggAddressData() { }
        
        public string Street { get; set; }

        public string Number { get; set; }

        public string Complement { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string AddressReference { get; set; }

        public string ZipCode { get; set; }

        public string CountryIsoCode { get; set; }
    }
}
