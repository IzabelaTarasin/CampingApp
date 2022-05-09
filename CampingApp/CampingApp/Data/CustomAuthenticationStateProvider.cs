using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace App_Camping.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //var claim = new Claim(ClaimTypes.Name, "aro@gmail.com");
            //var claimsIdentity = new ClaimsIdentity(new[] { claim }, "serverauth");
            //var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            //return new AuthenticationState(claimsPrincipal);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
    }
}

