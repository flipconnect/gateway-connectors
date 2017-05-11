using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using Dlp.Framework;
using Mundipagg.Connector.Model;

namespace Mundipagg.Connector {

    public sealed class CreditCard {

        public CreditCard(string customerId, string authorization) : base() {
            this.CustomerId = customerId;
            this.Authorization = authorization;
        }

        private string Authorization { get; }

        private string CustomerId { get; }

        private const string MUNDIPAGG_CORE_HOST_ADDRESS = "https://api.mundipagg.com/core/v1";

        public MundipaggResponse<MundipaggCreditCardData> GetCreditCard(string creditCardKey) {

            string creditCardEndpoint = $"{MUNDIPAGG_CORE_HOST_ADDRESS}/customers/{this.CustomerId}/cards/{creditCardKey}";

            NameValueCollection header = new NameValueCollection { { "Authorization", this.Authorization } };

            WebResponse<string> result = RestClient.SendHttpWebRequest<string>(null, HttpVerb.Get, HttpContentType.Json, creditCardEndpoint, header, true);

            MundipaggResponse<MundipaggCreditCardData> response = new MundipaggResponse<MundipaggCreditCardData> { Success = result.IsSuccessStatusCode };

            if (result.IsSuccessStatusCode) {
                response.ResponseData = Serializer.NewtonsoftDeserialize<MundipaggCreditCardData>(Convert.ToString(result.ResponseData));
            } else {

                if (result.StatusCode == HttpStatusCode.NotFound)
                    return new MundipaggResponse<MundipaggCreditCardData> { Success = true };

                dynamic deserializedResult = Serializer.DynamicDeserialize(result.ResponseData);

                response.Error = deserializedResult.message;
            }

            return response;
        }

        public MundipaggResponse<IEnumerable<MundipaggCreditCardData>> GetCreditCards() {

            string creditCardEndpoint = $"{MUNDIPAGG_CORE_HOST_ADDRESS}/customers/{this.CustomerId}/cards";

            NameValueCollection header = new NameValueCollection { { "Authorization", this.Authorization } };

            WebResponse<string> result = RestClient.SendHttpWebRequest<string>(null, HttpVerb.Get, HttpContentType.Json, creditCardEndpoint, header, true);

            MundipaggResponse<IEnumerable<MundipaggCreditCardData>> response = new MundipaggResponse<IEnumerable<MundipaggCreditCardData>> { Success = result.IsSuccessStatusCode };

            dynamic deserializedResult = Serializer.DynamicDeserialize(result.ResponseData);

            if (result.IsSuccessStatusCode)
                response.ResponseData = Serializer.NewtonsoftDeserialize<IEnumerable<MundipaggCreditCardData>>(Convert.ToString(deserializedResult.data));
            else
                response.Error = deserializedResult.message;

            return response;
        }

        public MundipaggResponse<bool> DeleteCreditCard(string creditCardKey) {

            string creditCardEndpoint = $"{MUNDIPAGG_CORE_HOST_ADDRESS}/customers/{this.CustomerId}/cards/{creditCardKey}";

            NameValueCollection header = new NameValueCollection { { "Authorization", this.Authorization } };

            WebResponse<string> result = RestClient.SendHttpWebRequest<string>(null, HttpVerb.Delete, HttpContentType.Json, creditCardEndpoint, header, true);

            MundipaggResponse<bool> response = new MundipaggResponse<bool> { Success = result.IsSuccessStatusCode };

            if (result.IsSuccessStatusCode) {

                MundipaggCreditCardData creditCardData = Serializer.NewtonsoftDeserialize<MundipaggCreditCardData>(result.ResponseData);
                response.ResponseData = string.IsNullOrWhiteSpace(creditCardData.Status) == false && "delete".Equals(creditCardData.Status, StringComparison.OrdinalIgnoreCase) == true;

            } else {

                if (result.StatusCode == HttpStatusCode.NotFound)
                    return new MundipaggResponse<bool> { Success = true, ResponseData = true };

                dynamic deserializedResult = Serializer.DynamicDeserialize(result.RawData);
                response.Error = deserializedResult.message;
            }

            return response;
        }

        public MundipaggResponse<string> CreateCreditCard(MundipaggCreditCardData creditCardInfo) {

            string creditCardEndpoint = $"{MUNDIPAGG_CORE_HOST_ADDRESS}/customers/{this.CustomerId}/cards";

            NameValueCollection header = new NameValueCollection { { "Authorization", this.Authorization } };

            WebResponse<string> result = RestClient.SendHttpWebRequest<string>(creditCardInfo, HttpVerb.Post, HttpContentType.Json, creditCardEndpoint, header, true);

            MundipaggResponse<string> response = new MundipaggResponse<string> { Success = result.IsSuccessStatusCode };

            if (result.IsSuccessStatusCode) {

                MundipaggCreditCardData creditCardData = Serializer.NewtonsoftDeserialize<MundipaggCreditCardData>(result.ResponseData);
                response.ResponseData = creditCardData.Token;
            } else {

                dynamic deserializedResult = Serializer.DynamicDeserialize(result.ResponseData);
                response.Error = deserializedResult.message;
            }

            return response;
        }
    }
}