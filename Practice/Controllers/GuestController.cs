using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonManagement.Domain.POCO;
using PersonManagement.MVC.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PersonManagement.MVC.Controllers
{
    [Route("[controller]/academyEvents")]
    public class GuestController : Controller
    {
        private readonly string _url = Constants.ApiUrl + "academyEvent/";

        public async Task<ActionResult> Index()
        {
            var response = await GetResponse(_url);

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

        [Route("details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var url = _url + id;

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

        private async Task<HttpResponseMessage> GetResponse(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            return response;
        }
    }
}
