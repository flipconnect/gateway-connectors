using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using Dlp.Framework;
using Dlp.Framework.Container;
using Mundipagg.Connector.Model;

namespace Mundipagg.Connector {

    public static class CreditCard {

        static CreditCard() {

            IMundipaggSettings settings = IocFactory.Resolve<IMundipaggSettings>();
            Authorization = settings.Authorization;
            CustomerId = settings.CustomerId;

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        private static string Authorization { get; }

        private static string CustomerId { get; }

        private const string MUNDIPAGG_CORE_HOST_ADDRESS = "https://api.mundipagg.com/core/v1";

        public static MundipaggResponse<MundipaggCreditCardData> Get(string creditCardKey) {

            string creditCardEndpoint = $"{MUNDIPAGG_CORE_HOST_ADDRESS}/customers/{CustomerId}/cards/{creditCardKey}";

            NameValueCollection header = new NameValueCollection { { "Authorization", Authorization } };

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

        public static MundipaggResponse<IEnumerable<MundipaggCreditCardData>> GetAll() {

            string creditCardEndpoint = $"{MUNDIPAGG_CORE_HOST_ADDRESS}/customers/{CustomerId}/cards";

            NameValueCollection header = new NameValueCollection { { "Authorization", Authorization } };

            WebResponse<string> result = RestClient.SendHttpWebRequest<string>(null, HttpVerb.Get, HttpContentType.Json, creditCardEndpoint, header, true);

            MundipaggResponse<IEnumerable<MundipaggCreditCardData>> response = new MundipaggResponse<IEnumerable<MundipaggCreditCardData>> { Success = result.IsSuccessStatusCode };

            dynamic deserializedResult = Serializer.DynamicDeserialize(result.ResponseData);

            if (result.IsSuccessStatusCode)
                response.ResponseData = Serializer.NewtonsoftDeserialize<IEnumerable<MundipaggCreditCardData>>(Convert.ToString(deserializedResult.data));
            else
                response.Error = deserializedResult.message;

            return response;
        }

        public static MundipaggResponse<bool> Delete(string creditCardKey) {

            string creditCardEndpoint = $"{MUNDIPAGG_CORE_HOST_ADDRESS}/customers/{CustomerId}/cards/{creditCardKey}";

            NameValueCollection header = new NameValueCollection { { "Authorization", Authorization } };

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

        public static MundipaggResponse<string> Create(MundipaggCreditCardData creditCardInfo) {

            string creditCardEndpoint = $"{MUNDIPAGG_CORE_HOST_ADDRESS}/customers/{CustomerId}/cards";

            NameValueCollection header = new NameValueCollection { { "Authorization", Authorization } };

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