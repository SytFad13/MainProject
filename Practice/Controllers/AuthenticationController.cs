using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonManagement.Api.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PersonManagement.MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private const string ApiEndpoint = "auth/";

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            var url = Constants.ApiUrl + ApiEndpoint + "register";
            var json = JsonConvert.SerializeObject(new RegisterModel(username, email, password));
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await GetResponse(url, data);

            if (!response.IsSuccessStatusCode)
			{
                return View(); //error
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            var url = Constants.ApiUrl + ApiEndpoint + "login";
            var json = JsonConvert.SerializeObject(new LoginModel(login, password));
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await GetResponse(url, data);

            if (!response.IsSuccessStatusCode)
            {
                return View(); //error
            }

            CookieOptions cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(Constants.TokenExpirationTimeInMinutes),
                Secure = true
            };
            Response.Cookies.Append(Constants.Token, Token.GenerateToken(login), cookieOptions);

            return RedirectToAction("Index", "AcademyEvents");
        }
        public IActionResult Signout()
        {
            var token = Request.Cookies.FirstOrDefault(x => x.Key == Constants.Token);

            if (token.Key != null)
            {
                Response.Cookies.Delete(Constants.Token);
            }

            return RedirectToAction(nameof(Login));
        }

		private async Task<HttpResponseMessage> GetResponse(string url, StringContent data)
        {
            using var client = new HttpClient();
            var response = await client.PostAsync(url, data);

            return response;
        }
    }
}
