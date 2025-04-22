using System;
namespace Model.Manager
{
    public class JwtSettings
	{
        public string IssuerSigningKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int ExpirationSeconds { get; set; }
    }
}

