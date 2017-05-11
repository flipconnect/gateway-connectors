using System.Collections.Generic;

namespace Mundipagg.Connector.Model {

    public sealed class MundipaggPaymentData {

        public MundipaggPaymentData() { }

        public int Installments { get; set; }

        public long TotalAmountInCents { get; set; }

        public string StatementDescriptor { get; set; }

        public string SuccessUrl { get; set; }

        public List<MundipaggOrderData> OrderDataCollection { get; set; }
    }
}
