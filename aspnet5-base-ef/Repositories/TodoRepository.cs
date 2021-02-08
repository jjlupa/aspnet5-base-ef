using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnet5_base_ef.Models;
using Microsoft.EntityFrameworkCore;

namespace aspnet5_base_ef.Repositories
{
    public class TodoItemsRepository : ITodoItemsRepository
    {
        private readonly TodoContext _context;
        public TodoItemsRepository(TodoContext context)
        {
            _context = context;
        }
        public async Task<IList<TodoItem>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }
        public async Task<TodoItem?> GetTodoItem(Guid id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            return todoItem;
        }
        public async Task<TodoItem?> CreateTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task<bool> ChangeTodoItem(TodoItem item)
        {
            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // This code is from MS, but I don't like it. We tried to change a thing, it won't let us due to conflict.
                // If the item is gone, we can just punt false, but why matriculate the concurrency exception at all? We
                // either changed it or we didn't, if not, False, for any reason seems reasonable. Could turn into differentiated
                // response values, but other than that, false is false.
                if (!TodoItemExists(item.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task<bool> DeleteTodoItem(Guid id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
                return false;

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool TodoItemExists(Guid id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
