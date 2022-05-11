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
    public class AcademyEventController : ControllerBase
    {
        private readonly IAcademyEventService _academyEventService;

        public AcademyEventController(IAcademyEventService academyEventService)
        {
            _academyEventService = academyEventService;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<AcademyEvent>>> GetAll()
        {
            var academyEvents = await _academyEventService.GetAllAsync();

            return Ok(academyEvents);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AcademyEvent>> GetById(int id)
        {
            var academyEvent = await _academyEventService.GetByIdAsync(id);

            if (academyEvent == null)
            {
                return NotFound();
            }

            return Ok(academyEvent);
        }

        [HttpPost]
        public async Task<ActionResult<AcademyEvent>> Create(AcademyEvent academyEvent)
        {
            await _academyEventService.CreateAsync(academyEvent);

            if (academyEvent == null)
            {
                return new BadRequestObjectResult("Academy event with given name already exist");
            }

            return Ok(academyEvent);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<AcademyEvent>> Update(AcademyEvent academyEvent)
        {
            var result = await _academyEventService.UpdateAsync(academyEvent);

            if (result == null)
            {
                return new BadRequestObjectResult("Academy event not exists");
            }

            return Ok(academyEvent);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _academyEventService.DeleteAsync(id);

            if (!response)
            {
                return StatusCode(404);
            }

            return Ok();
        }

        [HttpGet]
        [Route("pending")]
        public async Task<ActionResult> GetPendingAcademyEvents()
        {
            var pendingAcademyEvents = await _academyEventService.GetPendingAcademyEvents();

            return Ok(pendingAcademyEvents);
        }
    }
}
