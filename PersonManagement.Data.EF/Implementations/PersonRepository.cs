using Microsoft.EntityFrameworkCore;
using PersonManagement.Data.Abstractions;
using PersonManagement.Domain.POCO;
using PersonManagement.PersistenceDB.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonManagement.Data.EF.Implementations
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _dbContext;

        public PersonRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Person> CreateAsync(Person person)
        {
            await _dbContext.Persons.AddAsync(person);
            await _dbContext.SaveChangesAsync();
            return person;
        }

        public async Task DeleteAsync(Person person)
        {
            _dbContext.Persons.Remove(person);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Person>> GetAllAsync()
        {
            return await _dbContext.Persons.ToListAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await _dbContext.Persons.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Person> UpdateAsync(Person person)
        {
            _dbContext.Persons.Update(person);
            await _dbContext.SaveChangesAsync();

            return person;
        }

    }
}
