using System.Collections.Specialized;
using Dlp.Framework;

namespace Mundipagg.Connector {

    public sealed class Customer {

        public Customer(string authorization) {
            this.Authorization = authorization;
        }

        private string Authorization { get; }

        private const string MUNDIPAGG_CORE_HOST_ADDRESS = "https://api.mundipagg.com/core/v1";

        public MundipaggResponse<string> CreateCustomer(string name, string email) {

            string customersEndpoint = $"{MUNDIPAGG_CORE_HOST_ADDRESS}/customers";

            NameValueCollection createCustomerHeader = new NameValueCollection { { "Authorization", this.Authorization } };

            WebResponse<string> createCustomerResult = RestClient.SendHttpWebRequest<string>(new { name, email }, HttpVerb.Post, HttpContentType.Json, customersEndpoint, createCustomerHeader, true);

            bool createdCustomerSuccess = createCustomerResult.IsSuccessStatusCode;

            MundipaggResponse<string> response = new MundipaggResponse<string> { Success = createdCustomerSuccess };

            dynamic deserializedResult = Serializer.DynamicDeserialize(createCustomerResult.RawData);

            if (createdCustomerSuccess) response.ResponseData = (string)deserializedResult.id;
            else response.Error = deserializedResult.message;

            return response;
        }
    }
}