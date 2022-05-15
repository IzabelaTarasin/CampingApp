using System;
using System.Security.Claims;
using CampingApp.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace App_Camping.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private IUserService _userService;

        public CustomAuthenticationStateProvider(IUserService userService)
        {
            _userService = userService;
    }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var user = await _userService.GetMe();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                };

                foreach (var role in user.Roles)
                {
                    var claim = new Claim(ClaimTypes.Role, role);
                    claims.Add(claim); //dodajemy role
                }

                var claimsIdentity = new ClaimsIdentity(claims, "serverauth"); //??
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                return new AuthenticationState(claimsPrincipal); //zwraca specjallny obiekt ktory opisj
            }
            catch
            {

                //dla niezalogowaanego uzytkownikka/gdy cos sie nei uda:
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            
        }
    }
}