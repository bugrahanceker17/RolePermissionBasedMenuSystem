﻿namespace RoleBasedMenuSystem.Core.Security.JWT
{
    public class TokenOptionsModel
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }
        public string SecurityKey { get; set; } 
    }
}

