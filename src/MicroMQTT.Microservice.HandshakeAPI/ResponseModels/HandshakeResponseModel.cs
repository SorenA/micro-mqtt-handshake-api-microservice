namespace MicroMQTT.Microservice.HandshakeAPI.ResponseModels
{
    public class HandshakeResponseModel
    {
        public HandshakeResponseModel(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }

        public HandshakeResponseModel() { }

        public bool IsSuccessful { get; set; }

        public string Identity { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
