using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PersonManagement.Data.Abstractions;
using PersonManagement.Domain.POCO;
using PersonManagement.PersistenceDB.Context;

namespace PersonManagement.Data.EF.Implementations
{
	public class AcademyEventRepository : IAcademyEventRepository
	{
		private readonly AppDbContext _dbContext;

		public AcademyEventRepository(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

        public async Task<AcademyEvent> CreateAsync(AcademyEvent academyEvent)
		{
			await _dbContext.AcademyEvents.AddAsync(academyEvent);
			await _dbContext.SaveChangesAsync();
			return academyEvent;
		}

		public async Task DeleteAsync(AcademyEvent academyEvent)
		{
			_dbContext.AcademyEvents.Remove(academyEvent);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<ICollection<AcademyEvent>> GetAllAsync()
		{
			return await _dbContext.AcademyEvents.ToListAsync();
		}

        public async Task<AcademyEvent> GetByIdAsync(int id)
		{
			return await _dbContext.AcademyEvents.FirstOrDefaultAsync(c => c.Id == id);
		}
		public async Task<AcademyEvent> GetByNameAsync(string name)
		{
			var academyEvent = await _dbContext.AcademyEvents.FirstOrDefaultAsync(a => a.Name == name);

			return academyEvent;
		}

		public async Task<AcademyEvent> UpdateAsync(AcademyEvent academyEvent)
		{
			_dbContext.AcademyEvents.Update(academyEvent);
			await _dbContext.SaveChangesAsync();

			return academyEvent;
		}

		public async Task<ICollection<AcademyEvent>> GetAcademyEventsByAuthorUsername(string authorUsername)
		{
			return await _dbContext.AcademyEvents.Where(a => a.AuthorUsername == authorUsername).ToListAsync();
		}
	}
}
