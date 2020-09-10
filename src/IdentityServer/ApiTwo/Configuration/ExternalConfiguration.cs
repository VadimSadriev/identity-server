namespace ApiTwo.Configuration
{
    public abstract class ExternalConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
        public string Endpoint { get; set; }
    }
}
