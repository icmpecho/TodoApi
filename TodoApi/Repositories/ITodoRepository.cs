using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Repositories.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> List();
        Task Create(TodoItem item);
        Task Update(TodoItem item);
        Task<TodoItem> Get(string id);
        Task Delete(string id);
        Task Clear();
    }
}