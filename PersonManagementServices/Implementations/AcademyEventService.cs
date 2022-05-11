using PersonManagement.DAL.Abstractions;
using PersonManagement.Domain.POCO;
using PersonManagement.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonManagement.Services.Implementations
{
    public class AcademyEventService : IAcademyEventService
    {
        private readonly IAcademyEventRepository _academyEventRepo;

        public AcademyEventService(IAcademyEventRepository academyEventRepository)
        {
            _academyEventRepo = academyEventRepository;
        }

        public async Task<AcademyEvent> CreateAsync(AcademyEvent academyEvent)
        {
            var academyEventInDb = await _academyEventRepo.GetByNameAsync(academyEvent.Name);

            if (academyEventInDb != null)
            {
                return null;
            }

            academyEvent.EditDurationInDays = 30;

            await _academyEventRepo.CreateAsync(academyEvent);

            return academyEvent;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var academyEvent = await _academyEventRepo.GetByIdAsync(id);

            if (academyEvent == null)
            {
                return false;
            }

            await _academyEventRepo.DeleteAsync(academyEvent);

            return true;
        }

        public async Task<ICollection<AcademyEvent>> GetAllAsync()
        {
            var academyEvent = await _academyEventRepo.GetAllAsync();

            return academyEvent;
        }

        public async Task<AcademyEvent> GetByIdAsync(int id)
        {
            var academyEvent = await _academyEventRepo.GetByIdAsync(id);

            return academyEvent;
        }

        public async Task<AcademyEvent> UpdateAsync(AcademyEvent academyEvent)
        {
            var academyEventInDb = await _academyEventRepo.GetByIdAsync(academyEvent.Id);

            if (academyEventInDb == null)
            {
                return null;
            }

            if(academyEventInDb.CreatedAt.AddDays(academyEventInDb.EditDurationInDays) < DateTime.Now)
            {
                return null;
            }

            await _academyEventRepo.UpdateAsync(academyEvent);

            return academyEvent;
        }

        public async Task<AcademyEvent> GetByName(string name)
        {
            var academyEvent = await _academyEventRepo.GetByNameAsync(name);

            return academyEvent;
        }

        public async Task<ICollection<AcademyEvent>> GetAcademyEventsByAuthorUsername(string authorUsername)
        {
            var academyEvents = await _academyEventRepo.GetAcademyEventsByAuthorUsername(authorUsername);

            return academyEvents;
        }

        public async Task<ICollection<AcademyEvent>> GetPendingAcademyEvents()
        {
            var pendingAcademyEvents = await _academyEventRepo.GetPendingAcademyEvents();

            return pendingAcademyEvents;
        }
    }
}
