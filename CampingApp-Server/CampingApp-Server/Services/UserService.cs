using System;
using Microsoft.AspNetCore.Identity;
using CampingApp_Server.Database;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CampingApp_Server.Services
{
    public interface IUserService
    {
        public Task<bool> CreateUser(string name, string phoneNumber, string email, string password);
        public Task<User> GetUserById(string id);
        public Task<string> SignIn(string email, string password);
        public Task<List<string>> GetRolesForUserId(string id);
    }

    public class UserService : IUserService
    {
        public IConfiguration _configuration;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration) //dependency - wstrzykiwany do konstruktora.
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<bool> CreateUser(string name, string phoneNumber, string email, string password)
        {
            User user = new User
            {
                UserName = name,
                PhoneNumber = phoneNumber,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password); //nastepuje tworzenie uzytkownika

            if (!result.Succeeded)
            {
                return false;
            }

            //przypanie roli
            var resultRole = await _userManager.AddToRoleAsync(user, "standard");

            if (!resultRole.Succeeded)
            {
                return false;
            }

            return true;

        }

        public async Task<User> GetUserById(string id) //bo name to email ktory jest unkalny wiec mozna szukac po name
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<string> SignIn(string email, string password)
        {
            //ma zwracac imie uzytkownika
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("brak uzytkownika");
            }

            //spr czy hasla sa poprawne
            var check = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!check.Succeeded)
            {
                throw new Exception("niepoprawne haslo"); //pamietaj ze jak jest throow exce tto przy wywolaniu tej metody trzeba robic try catch
            }

            //powyzsze kroki mowia ze mmay dobrego uzytkownika z haslem wiec teraz tworzymy tokken i go zwracamy
            var token = await CreateToken(user);
            if (token == null)
            {
                throw new Exception("Brak tokena");
            }
            return token;
        }

        public async Task<List<string>> GetRolesForUserId(string id)
        {
            var user = await GetUserById(id);
            if (user == null)
            {
                throw new Exception("Brak uzytkownika o podanym id");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null)
            {
                throw new Exception("nie utworzono listy roli");
            }

            return roles.ToList();
        }

        private async Task<string> CreateToken(User user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)

            };

            //dodanie roli
            var userRoles = await _userManager.GetRolesAsync(user); //roles bo moze miec kilka rol

            foreach (var role in userRoles)
            {
                var claim = new Claim(ClaimTypes.Role, role);
                claims.Add(claim); //nie mozna dodac tak jak wyzej bo tzreba role przekształccić na posszczególne wpisy (był blad)  wiec sobie wyciagamy poszczegolne role i dodajemy            }

            }

            //Jwt:Key -> tym kluczem są podpisywane tokeny jwt potrzebne do logowania, one sa haszowane i zapisywane pod smienna signInCredentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])); //aby z tego korzystac trzebabylo wstrzyknac do konstruktora IConfigurstion bo ten obiektt zawiera daane z pliiku appsettings.json
            var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //obiekt tokenDescrriptor bedzie zawierac dane jakie m amiec token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), //zawiera claimsy
                Expires = DateTime.UtcNow.AddDays(1), //wygsa za 1 dzien
                SigningCredentials = signInCredentials //musza zgadzac sie credentiale zahashowane
            };

            //obiekt tokenHandler do faaktycznego poddpisywania tokena
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

//teraz dependencynjectons:
//pamietac aby dodac do program.cs -> builder.Services.AddScoped<IUserService, UserService>();
//i w signUpconttr. -> private IUserService _userService;

//public SignUpController(IUserService userService)
//{
//	_userService = userService;
//}
