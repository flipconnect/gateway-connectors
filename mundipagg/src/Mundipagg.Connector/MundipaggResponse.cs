namespace Mundipagg.Connector {

    public sealed class MundipaggResponse<T> {

        public MundipaggResponse() { }

        public bool Success { get; set; }

        public T ResponseData { get; set; }

        public string Error { get; set; }
        
    }
}