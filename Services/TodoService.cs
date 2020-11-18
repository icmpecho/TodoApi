using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Repositories.Models;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        private static TodoItemVm MapTodoItem(TodoItem item) =>
            new TodoItemVm
            {
                Id = item.Id,
                Name = item.Name,
                IsComplete = item.IsCompleted,
            };

        public async Task<TodoItemVm> AddTodo(string name)
        {
            var now = DateTime.Now;
            var id = now.ToString("O");
            await _repository.Create(new TodoItem
            {
                Id = id,
                Created = DateTime.Now,
                Name = name,
                IsCompleted = false,
            });
            var item = await _repository.Get(id);
            return MapTodoItem(item);
        }

        public async Task<IEnumerable<TodoItemVm>> ListTodos()
        {
            var todoItems = await _repository.List();
            return todoItems.Select(MapTodoItem);
        }

        public async Task<TodoItemVm> MarkAsDone(string id)
        {
            var item = await _repository.Get(id);
            var newItem = new TodoItem
            {
                Id = item.Id,
                Name = item.Name,
                Created = item.Created,
                IsCompleted = true,
            };
            await _repository.Update(newItem);
            return MapTodoItem(newItem);
        }
    }
}