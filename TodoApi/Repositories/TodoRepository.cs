using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Repositories.Models;

namespace TodoApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private ImmutableList<TodoItem> _todoItems = ImmutableList<TodoItem>.Empty;

        public Task<IEnumerable<TodoItem>> List() => Task.FromResult<IEnumerable<TodoItem>>(_todoItems);

        public Task Create(TodoItem item)
        {
            _todoItems = _todoItems.Add(item);
            return Task.CompletedTask;
        }

        public async Task Update(TodoItem item)
        {
            var existingItem = await Get(item.Id);
            _todoItems = _todoItems.Replace(existingItem, item);
        }

        public Task<TodoItem> Get(string id) =>
            Task.FromResult(_todoItems.FirstOrDefault(i => i.Id == id));

        public async Task Delete(string id)
        {
            var item = await Get(id);
            _todoItems = _todoItems.Remove(item);
        }

        public Task Clear()
        {
            _todoItems = ImmutableList<TodoItem>.Empty;
            return Task.CompletedTask;
        }
    }
}