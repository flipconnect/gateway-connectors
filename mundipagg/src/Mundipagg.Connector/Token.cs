using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Dlp.Framework;
using Mundipagg.Connector.Model;

namespace Mundipagg.Connector {

    public sealed class Token {

        public Token(string customerId, string authorization) {
            this.CustomerId = customerId;
            this.Authorization = authorization;
        }

        private string CustomerId { get; }

        private string Authorization { get; }

        private const string MUNDIPAGG_CORE_HOST_ADDRESS = "https://api.mundipagg.com/core/v1";

        private const string MUNDIPAGG_COMMERCE_HOST_ADDRESS = "https://api.mundipagg.com/checkout/v1";

        public MundipaggResponse<string> CreateCheckoutToken(MundipaggPaymentData orderData, MundipaggAddressData addressData = null) {

            #region Dynamic Token Parameters

            object addressParameter = null;

            if (addressData != null) {

                var address = new {
                    editable = false,
                    street = addressData.Street,
                    number = addressData.Number,
                    complement = addressData.Complement,
                    zip_code = addressData.ZipCode.GetDigits(),
                    neighborhood = addressData.District,
                    city = addressData.City,
                    state = addressData.State,
                    country = addressData.CountryIsoCode
                };

                addressParameter = address;
            }
            
            var orderParameter = new {
                items = orderData.OrderDataCollection.Select(itemData => new { amount = itemData.AmountInCents, description = itemData.Description, quantity = itemData.Quantity }).Cast<object>().ToList()
            };

            List<object> installmentCollection = new List<object> {
                new {
                        installments = orderData.Installments,
                        total_amount = orderData.TotalAmountInCents
                    }
            };

            var cardParameters = new {
                capture = "true",
                statement_descriptor = orderData.StatementDescriptor,
                installments = installmentCollection
            };

            var paymentSettingsParameters = new {
                default_payment_method = "credit_card",
                accepted_payment_methods = new[] { "credit_card" },
                card = cardParameters
            };

            var data = new {
                expires_in = 30,
                type = "order",
                order = orderParameter,
                customer_id = this.CustomerId,
                customer_editable = false,
                shippable = false,
                currency = "BRL",
                success_url = orderData.SuccessUrl,
                billing_address = addressParameter,
                payment_settings = paymentSettingsParameters
            };

            #endregion

            string createTokenEndpoint = $"{MUNDIPAGG_COMMERCE_HOST_ADDRESS}/tokens";

            NameValueCollection createTokenHeader = new NameValueCollection { { "Authorization", this.Authorization } };

            WebResponse<string> createTokenResult = RestClient.SendHttpWebRequest<string>(data, HttpVerb.Post, HttpContentType.Json, createTokenEndpoint, createTokenHeader, true);

            bool createTokenSuccess = createTokenResult.IsSuccessStatusCode;

            MundipaggResponse<string> response = new MundipaggResponse<string> { Success = createTokenSuccess };

            dynamic deserializedResult = Serializer.DynamicDeserialize(createTokenResult.RawData);

            if (createTokenSuccess)
                response.ResponseData = (string)deserializedResult.id;
            else
                response.Error = deserializedResult.Message;

            return response;
        }

        public MundipaggResponse<string> CreateAccessToken() {

            string accessTokenEndpoint = $"{MUNDIPAGG_CORE_HOST_ADDRESS}/customers/{this.CustomerId}/access-tokens";

            NameValueCollection createAccessTokenHeader = new NameValueCollection { { "Authorization", this.Authorization } };

            WebResponse<string> createAccessTokenResult = RestClient.SendHttpWebRequest<string>(null, HttpVerb.Post, HttpContentType.Json, accessTokenEndpoint, createAccessTokenHeader, true);

            bool createAccessTokenSuccess = createAccessTokenResult.IsSuccessStatusCode;

            MundipaggResponse<string> response = new MundipaggResponse<string> { Success = createAccessTokenSuccess };

            dynamic deserializedResult = Serializer.DynamicDeserialize(createAccessTokenResult.RawData);

            if (createAccessTokenSuccess)
                response.ResponseData = (string)deserializedResult.code;
            else
                response.Error = deserializedResult.message;

            return response;
        }
    }
}
