using System.Collections.Generic;
using System.Net;
using Dlp.Framework.Container;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mundipagg.Connector.Model;

namespace Mundipagg.Connector.UnitTest {

    [TestClass]
    public class Mark1Test {

        public Mark1Test() { 

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        private static string _createdCreditCardKey;

        [TestInitialize]
        public void TestInit() {

            MundipaggSettingsMock settings = new MundipaggSettingsMock { Authorization = "", CustomerId = "" };

            IocFactory.Register(
                Component.For<IMundipaggSettings>()
                .Instance(settings)
                );
        }

        [TestMethod]
        public void CreateCreditCard_Test() {

            MundipaggCreditCardData creditCardData = new MundipaggCreditCardData {
                BillingAddress = null,
                Number = "5547157866507129",
                HolderName = "Unit Test FC Customer",
                ExpiryMonth = 1,
                ExpiryYear = 18,
                CardCvv = "422"
            };

            MundipaggResponse<string> createCreditCardResponse = CreditCard.Create(creditCardData);

            Assert.IsTrue(createCreditCardResponse.Success);
            Assert.IsNotNull(createCreditCardResponse.ResponseData);
            Assert.IsNull(createCreditCardResponse.Error);

            _createdCreditCardKey = createCreditCardResponse.ResponseData;
        }

        [TestMethod]
        public void GetCreditCard_Test() {

            MundipaggResponse<MundipaggCreditCardData> getCreditCardResponse = CreditCard.Get(_createdCreditCardKey);

            Assert.IsTrue(getCreditCardResponse.Success);
            Assert.IsNotNull(getCreditCardResponse.ResponseData);
            Assert.IsNull(getCreditCardResponse.Error);
        }

        [TestMethod]
        public void GetAllCreditCards_Test() {

            MundipaggResponse<IEnumerable<MundipaggCreditCardData>> getCreditCardsResponse = CreditCard.GetAll();

            Assert.IsTrue(getCreditCardsResponse.Success);
            Assert.IsNotNull(getCreditCardsResponse.ResponseData);
            Assert.IsNull(getCreditCardsResponse.Error);
        }

        [TestMethod]
        public void DeleteCreditCard_Test() {

            MundipaggResponse<bool> deleteCreditCardResponse = CreditCard.Delete(_createdCreditCardKey);

            Assert.IsTrue(deleteCreditCardResponse.Success);
            Assert.IsNotNull(deleteCreditCardResponse.ResponseData);
            Assert.IsNull(deleteCreditCardResponse.Error);
        }
    }
}
