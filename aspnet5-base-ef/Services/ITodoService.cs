using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aspnet5_base_ef.DTOs;

namespace aspnet5_base_ef.Services
{
    public interface ITodoItemsService
    {
        Task<IList<TodoItemDTOv1>> GetTodoItems();
        Task<TodoItemDTOv1?> GetTodoItem(Guid id);
        Task<TodoItemDTOv1?> CreateTodoItem(TodoItemDTOv1 item);
        Task<bool> ChangeTodoItem(TodoItemDTOv1 item);
        Task<bool> DeleteTodoItem(Guid id);
    }
}