using PersonManagement.Domain.POCO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonManagement.Data.Abstractions
{
    public interface IAcademyEventRepository : IRepository<AcademyEvent>
    {
        Task<AcademyEvent> GetByNameAsync(string name);
        Task<ICollection<AcademyEvent>> GetAcademyEventsByAuthorId(string authorId);
    }
}
