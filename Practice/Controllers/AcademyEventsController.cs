using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonManagement.Domain.POCO;
using PersonManagement.MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PersonManagement.MVC.Controllers
{
    public class AcademyEventsController : Controller
    {
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

        public async Task<ActionResult> My()
        {
            var url = Constants.ApiUrl + "users/" + GetUserLogin() + "/academyEvents";
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

            return View(academyEvents);
        }

        public async Task<ActionResult> Details(int id)
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(AcademyEventViewModel viewModel)
        {
            var academyEvent = viewModel.Adapt<AcademyEvent>();
            academyEvent.AuthorUsername = GetUserLogin();
            academyEvent.CreatedAt = DateTime.Now;
            var url = Constants.ApiUrl + "academyEvent";
            var json = JsonConvert.SerializeObject(academyEvent);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await PostResponse(url, data);

            if (!response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(responseJson);
                var errors = obj["errors"].ToList();

                return View(errors); //errors view
            }

            return RedirectToAction("Index"); //success
        }

        public IActionResult Update()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Update(AcademyEventViewModel viewModel)
        {
            var academyEvent = viewModel.Adapt<AcademyEvent>();
            
            var url = Constants.ApiUrl + "academyEvent";
            var json = JsonConvert.SerializeObject(academyEvent);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await PutResponse(url, data);

            if (!response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(responseJson);
                var errors = obj["errors"].ToList();

                return View(errors); //errors view
            }

            return View(); //success
        }

        private async Task<HttpResponseMessage> PostResponse(string url, HttpContent data)
        {
            using var client = new HttpClient();
            var response = await client.PostAsync(url, data);

            return response;
        }

        private async Task<HttpResponseMessage> PutResponse(string url, HttpContent data)
        {
            using var client = new HttpClient();
            var response = await client.PutAsync(url, data);

            return response;
        }

        private async Task<HttpResponseMessage> GetResponse(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            return response;
        }

        private string GetUserLogin()
        {
            return Token.GetLoginFromToken(Request.Cookies.FirstOrDefault(x => x.Key == Constants.Token).Value);
        }
    }
}
