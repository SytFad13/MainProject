using PersonManagement.Domain.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonManagement.Services.Abstractions
{
    public interface IAcademyEventService : IService<AcademyEvent>
    {
        Task<AcademyEvent> GetByName(string name);
        Task<ICollection<AcademyEvent>> GetAcademyEventsByAuthorUsername(string authorUsername);
        Task<ICollection<AcademyEvent>> GetPendingAcademyEvents();
    }
}
