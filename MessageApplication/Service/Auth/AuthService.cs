using MessageApplication.Models;
using MessageApplication.Service.Interface;
using MessageApplication.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MessageApplication.Service.Auth
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _cofig;
        public AuthService(UserManager<AppUser> userManager, IConfiguration cofig) { 

            _userManager = userManager;
            _cofig = cofig;
        }
        public async Task<bool> Login(LoginViewModel login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return false;
            }
            return await _userManager.CheckPasswordAsync(user, login.Password);
           
           

        }
        public string GenerateTokenString(LoginViewModel login)
        {
            IEnumerable<Claim> claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name,login.Email),
                new Claim(ClaimTypes.Role,"Admin")
            };

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cofig.GetSection("Jwt:Key").Value));
            SigningCredentials signingCredentials = new SigningCredentials(
                securityKey,SecurityAlgorithms.HmacSha256Signature);
          var  SecurityToken = new JwtSecurityToken(
              claims:claim,
              expires:DateTime.Now.AddMinutes(120),
             issuer: _cofig.GetSection("Jwt:ValidIssuer").Value,
             audience:_cofig.GetSection("Jwt:ValidAudiences").Value,
             signingCredentials: signingCredentials
              );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return tokenString;
        }
        public async Task<bool> RegisterUser(RegisterViewModel register)
        {
            var user = await _userManager.FindByEmailAsync(register.Email);
            if (user != null)
            {
                return false;
            }
            if (register.Email == null)
            {
                return false;
            }
            if (register.Password == null || register.ConfirmPassword == null)
            {
                return false;
            }
            if (register.Password != register.ConfirmPassword)
            {
                return false;
            }
            var identityUser = new AppUser
            {
                Email = register.Email,
                UserName = register.Email
            };
            var result = await  _userManager.CreateAsync(identityUser, register.Password);

            return result.Succeeded;
        }
    }
}
