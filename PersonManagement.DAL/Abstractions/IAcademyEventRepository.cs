using PersonManagement.Domain.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonManagement.DAL.Abstractions
{
    public interface IAcademyEventRepository : IRepository<AcademyEvent>
    {
        Task<AcademyEvent> GetByNameAsync(string name);
        Task<ICollection<AcademyEvent>> GetAcademyEventsByAuthorUsername(string authorUsername);
        Task<ICollection<AcademyEvent>> GetPendingAcademyEvents();
    }
}
