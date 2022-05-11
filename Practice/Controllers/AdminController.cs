using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonManagement.Api.Models;
using PersonManagement.Domain.POCO;
using PersonManagement.MVC.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PersonManagement.MVC.Controllers
{
    [Route("[controller]")]
    public class AdminController : Controller
    {
        [HttpGet]
        [Route("academyEvents")]
        public async Task<ActionResult> Index()
        {
            var url = Constants.ApiUrl + "academyEvent";
            var response = await GetResponse(url);

            if (!response.IsSuccessStatusCode)
            {
                return View(); //not found
            }

            var json = await response.Content.ReadAsStringAsync();
            var academyEvents = JsonConvert
                .DeserializeObject<List<AcademyEvent>>(json)
                .Where(x => x.IsApproved)
                .Select(a => a.Adapt<AcademyEventViewModel>());

            return View(academyEvents); //success
        }

        [HttpGet]
        [Route("academyevents/update/{id}")]
        public async Task<ActionResult> Update(int id)
        {
            var url = Constants.ApiUrl + "academyEvent/" + id;

            var response = await GetResponse(url);

            if (!response.IsSuccessStatusCode)
            {
                return View(); //not found
            }

            var json = await response.Content.ReadAsStringAsync();
            var academyEvent = JsonConvert
                .DeserializeObject<AcademyEvent>(json)
                .Adapt<AcademyEventViewModel>();

            return View(academyEvent);
        }

        [HttpPost]
        [Route("academyevents/update/{id}")]
        public async Task<ActionResult> Update(int id, AcademyEventViewModel viewModel)
        {
            var academyEvent = viewModel.Adapt<AcademyEvent>();
            academyEvent.Id = id;
            academyEvent.IsApproved = true;
            var url = Constants.ApiUrl + "academyEvent/" + id;
            var json = JsonConvert.SerializeObject(academyEvent);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await PutRequest(url, data);

            if (!response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(responseJson);
                var errors = obj["errors"].ToList();

                return View(errors); //errors view
            }

            return RedirectToAction("Index"); //success
        }

        [HttpGet]
        [Route("academyevents/delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var url = Constants.ApiUrl + "academyEvent/" + id;
            var response = await DeleteResponse(url);

            if (!response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(responseJson);
                var errors = obj["errors"].ToList();

                return View(errors); //errors view
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("academyevents/pending")]
        public async Task<ActionResult> Pending()
        {
            var url = Constants.ApiUrl + "academyEvent/pending";
            var response = await GetResponse(url);

            if (!response.IsSuccessStatusCode)
            {
                return View(); //not found
            }

            var json = await response.Content.ReadAsStringAsync();
            var pendingAcademyEvents = JsonConvert
                .DeserializeObject<List<AcademyEvent>>(json)
                .Select(a => a.Adapt<AcademyEventViewModel>());

            return View(pendingAcademyEvents); //success
        }

        [HttpGet]
        [Route("academyevents/approve/{id}")]
        public async Task<ActionResult> Approve(int id, AcademyEventViewModel viewModel)
        {
            var url = Constants.ApiUrl + "academyEvent/" + id;

            var response = await GetResponse(url);

            var json = await response.Content.ReadAsStringAsync();
            var academyEvent = JsonConvert
                .DeserializeObject<AcademyEvent>(json);

            academyEvent.Id = id;
            academyEvent.IsApproved = true;
            url = Constants.ApiUrl + "academyEvent/" + id;
            json = JsonConvert.SerializeObject(academyEvent);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            response = await PutRequest(url, data);

            if (!response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(responseJson);
                var errors = obj["errors"].ToList();

                return View(errors); //errors view
            }

            return RedirectToAction("Index"); //success
        }

        [HttpGet]
        [Route("users")]
        public async Task<ActionResult> GetUsers()
        {
            var url = Constants.ApiUrl + "users";
            var response = await GetResponse(url);

            if (!response.IsSuccessStatusCode)
            {
                return View(); //not found
            }

            var json = await response.Content.ReadAsStringAsync();
            var users = JsonConvert
                .DeserializeObject<List<IdentityUser>>(json);

            var variable = users.Adapt<List<UserViewModel>>();

                //.Select(a => a.Adapt<UserViewModel>());

            return View(variable); //success
        }

        [HttpGet]
        [Route("users/create")]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Route("users/create")]
        public async Task<ActionResult> CreateUser(string username, string email, string password)
        {
            var url = Constants.ApiUrl + "auth/" + "register";
            var json = JsonConvert.SerializeObject(new RegisterModel(username, email, password));
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await PostResponse(url, data);

            if (!response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(responseJson);
                var errors = obj["errors"].ToList();

                return View(errors); //errors view
            }

            return View();
        }

        [HttpGet]
        [Route("users/delete/{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var url = Constants.ApiUrl + "users/" + username;
            var response = await DeleteResponse(url);

            if (!response.IsSuccessStatusCode)
            {
                return View(); //not found
            }

            return RedirectToAction("GetUsers"); //success
        }

        private async Task<HttpResponseMessage> PutRequest(string url, HttpContent data)
        {
            using var client = new HttpClient();
            var response = await client.PutAsync(url, data);

            return response;
        }

        private async Task<HttpResponseMessage> PostResponse(string url, HttpContent data)
        {
            using var client = new HttpClient();
            var response = await client.PostAsync(url, data);

            return response;
        }

        private async Task<HttpResponseMessage> GetResponse(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            return response;
        }

        private async Task<HttpResponseMessage> DeleteResponse(string url)
        {
            using var client = new HttpClient();
            var response = await client.DeleteAsync(url);

            return response;
        }

        private string GetUserLogin()
        {
            return Token.GetLoginFromToken(Request.Cookies.FirstOrDefault(x => x.Key == Constants.Token).Value);
        }
    }
}
