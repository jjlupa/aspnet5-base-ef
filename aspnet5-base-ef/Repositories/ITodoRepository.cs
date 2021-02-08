using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aspnet5_base_ef.Models;

namespace aspnet5_base_ef.Repositories
{
    public interface ITodoItemsRepository
    {
        Task<IList<TodoItem>> GetTodoItems();
        Task<TodoItem?> GetTodoItem(Guid id);
        Task<TodoItem?> CreateTodoItem(TodoItem item);
        Task<bool> ChangeTodoItem(TodoItem item);
        Task<bool> DeleteTodoItem(Guid id);
    }
}
