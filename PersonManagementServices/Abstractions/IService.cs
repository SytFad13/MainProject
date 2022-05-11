using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonManagement.Services.Abstractions
{
	public interface IService<T>
	{
		Task<ICollection<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		Task<T> CreateAsync(T model);
		Task<T> UpdateAsync(T model);
		Task<bool> DeleteAsync(int id);
	}
}
