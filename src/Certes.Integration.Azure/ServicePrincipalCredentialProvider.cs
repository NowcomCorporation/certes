﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;

namespace Certes.Integration.Azure
{
    public class ServicePrincipalCredentialProvider : IClientCredentialProvider
    {
        private readonly ServicePrincipalOptions options;
        private AuthenticationResult token;

        public ServicePrincipalCredentialProvider(IOptions<ServicePrincipalOptions> options)
        {
            this.options = options.Value;
        }

        public async Task<string> GetOrCreateAccessToken()
        {
            if (token == null || token.ExpiresOn < DateTimeOffset.Now)
            {
                var authContext = new AuthenticationContext($"https://login.windows.net/{options.TenantId}");
                var credential = new ClientCredential(options.ClientId, options.ClientSecret);
                this.token = await authContext.AcquireTokenAsync("https://management.azure.com/", credential);
            }

            return token.AccessToken;
        }
    }
}