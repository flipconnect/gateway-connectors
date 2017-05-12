namespace Mundipagg.Connector.UnitTest {

    public sealed class MundipaggSettingsMock : IMundipaggSettings {

        public MundipaggSettingsMock() { }

        public string Authorization { get; set; }

        public string CustomerId { get; set; }
    }
}