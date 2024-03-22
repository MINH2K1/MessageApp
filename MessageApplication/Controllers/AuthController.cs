using MessageApplication.Service.Interface;
using MessageApplication.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MessageApplication.Controllers
{
    public class AuthController:Controller
    {
        private readonly IAuthService _authService;
        public AuthController( IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Login")]
        public IActionResult Login( LoginViewModel Login)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
           
            var result = _authService.Login(Login);
            var tokenString = _authService.GenerateTokenString(Login);
            return View(result);
        }
        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
        [HttpPost("Register")]
        public IActionResult Register(RegisterViewModel Register)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
          var result =  _authService.RegisterUser(Register);
          
            return View(result);


        }
    }
}
