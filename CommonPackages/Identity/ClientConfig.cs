namespace Identity
{
    public class ClientConfig
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string RedirectUris { get; set; }
        public string AllowedGrantTypes { get; set; }
        public string FrontChannelLogoutUri { get; set; }
        public string PostLogoutRedirectUris { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public string ClientSecrets { get; set; }
        public string AllowedScopes { get; set; }
    }
}