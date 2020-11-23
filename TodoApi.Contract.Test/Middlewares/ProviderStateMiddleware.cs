using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TodoApi.Repositories;
using TodoApi.Repositories.Models;

namespace TodoApi.Test.Middlewares
{
    public class ProviderStateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITodoRepository _todoRepository;
        
        private async Task SetProviderState(ProviderState state)
        {
            var task = state.State switch
            {
                "Todo list is empty" => TodoListIsEmpty(),
                "Todo list has an item" => TodoListHasAnItem(),
                _ => TodoListIsEmpty(),
            };
            await task;
        }

        public ProviderStateMiddleware(RequestDelegate next, ITodoRepository todoRepository)
        {
            _next = next;
            _todoRepository = todoRepository;
        }
        
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == Constants.ProviderStateEndpoint)
            {
                await HandleProviderStatesRequest(context);
                await context.Response.WriteAsync(string.Empty);
            }
            else
            {
                await this._next(context);
            }
        }

        private async Task HandleProviderStatesRequest(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var providerState = await GetProviderStateFromBody(context.Request.Body);
            await SetProviderState(providerState);
        }

        private static async Task<ProviderState> GetProviderStateFromBody(Stream body)
        {
            using var reader = new StreamReader(body, Encoding.UTF8);
            var jsonRequestBody = await reader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);
        }

        private async Task TodoListIsEmpty()
        {
            await _todoRepository.Clear();
        }

        private async Task TodoListHasAnItem()
        {
            await _todoRepository.Clear();
            var now = DateTime.Now;
            await _todoRepository.Create(
                new TodoItem
                {
                    Id = now.ToString("O"),
                    Name = "Todo Item 1",
                    Created = now,
                    IsCompleted = false,
                });
        }
    }
}