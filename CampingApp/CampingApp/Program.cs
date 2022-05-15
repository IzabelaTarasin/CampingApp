using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CampingApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using App_Camping.Data;
using CampingApp.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>(); //inaczej blad o tresci: Cannot provide a value for property 'AuthorizationPolicyProvider' on type 'Microsoft.AspNetCore.Components.Authorization.AuthorizeView'. There is no registered service of type 'Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider'.

builder.Services.AddScoped(
    sp => new HttpClient {
        BaseAddress = new Uri("https://localhost:7026")
    });

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();

