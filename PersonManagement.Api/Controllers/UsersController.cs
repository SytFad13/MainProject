using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonManagement.Domain.POCO;
using PersonManagement.Services.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IAcademyEventService _academyEventService;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(IAcademyEventService academyEventService, UserManager<IdentityUser> userManager)
        {
            _academyEventService = academyEventService;
            _userManager = userManager;
        }

        [HttpGet]
        public ActionResult<ICollection<IdentityUser>> GetUsers()
        {
            return _userManager.Users.ToList();
        }

        [HttpDelete]
        [Route("{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if(user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            return Ok();
        }

        [HttpGet]
        [Route("{username}/academyEvents")]
        public async Task<ActionResult<ICollection<AcademyEvent>>> GetUserEvents(string username)
        {
            var userEvents = await _academyEventService.GetAcademyEventsByAuthorUsername(username);

            return Ok(userEvents);
        }
    }
}
