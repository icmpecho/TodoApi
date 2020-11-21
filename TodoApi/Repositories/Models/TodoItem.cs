using System;

namespace TodoApi.Repositories.Models
{
    public class TodoItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public bool IsCompleted { get; set; }
    }
}