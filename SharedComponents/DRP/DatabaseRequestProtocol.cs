namespace SharedComponents.Requests
{
    public class DatabaseRequestProtocol
    {
        public RequestType RequestType { get; set; }
        public int ComandaNumber { get; set; }
        public bool IsPreVenda { get; set; }
        public bool IsItensPreVendas { get; set; }
        public int BufferSize { get; set; }
        public string Path { get; set; }
        public string ErrorMessage { get; set; }
    }
}