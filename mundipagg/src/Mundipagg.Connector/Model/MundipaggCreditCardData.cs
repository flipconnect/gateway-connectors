using System;
using Newtonsoft.Json;

namespace Mundipagg.Connector.Model {

    public sealed class MundipaggCreditCardData {

        public MundipaggCreditCardData() { }

        [JsonProperty(PropertyName="id")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "last_four_digits")]
        public string LastFourDigits { get; set; }

        [JsonProperty(PropertyName = "cvv")]
        public string CardCvv { get; set; }

        [JsonProperty(PropertyName = "brand")]
        public string Brand { get; set; }

        [JsonProperty(PropertyName = "holder_name")]
        public string HolderName { get; set; }

        [JsonProperty(PropertyName = "exp_month")]
        public short ExpiryMonth { get; set; }

        [JsonProperty(PropertyName = "exp_year")]
        public short ExpiryYear { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public Nullable<DateTime> CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public Nullable<DateTime> UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "deleted_at")]
        public Nullable<DateTime> DeletedAt { get; set; }

        [JsonProperty(PropertyName = "billing_address")]
        public object BillingAddress { get; set; }
        
    }
}