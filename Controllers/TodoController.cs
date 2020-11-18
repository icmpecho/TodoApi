using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("/api/todoItems")]
    public class TodoController : ControllerBase
    {

        private readonly ILogger<TodoController> _logger;
        private readonly ITodoService _todoService;

        public TodoController(ILogger<TodoController> logger, ITodoService todoService)
        {
            _logger = logger;
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemVm>>> List()
        {
            var result = await _todoService.ListTodos();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemVm>> Create(CreateTodoItemRequest request)
        {
            var result = await _todoService.AddTodo(request.Name);
            return Ok(result);
        }

        [HttpPost]
        [Route("complete")]
        public async Task<ActionResult<TodoItemVm>> Complete(CompleteTodoItemRequest request)
        {
            var result = await _todoService.MarkAsDone(request.Id);
            return Ok(result);
        }
    }
}
