using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        Task<TodoItemVm> AddTodo(string name);
        Task<IEnumerable<TodoItemVm>> ListTodos();
        Task<TodoItemVm> MarkAsDone(string id);
    }
}