using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspnet5_base_ef.DTOs;
using aspnet5_base_ef.Services;

namespace aspnet5_base_ef.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemsService _service;
        public TodoItemsController(ITodoItemsService service)
        {
            _service = service;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTOv1>>> GetTodoItems()
        {
            return Ok(await _service.GetTodoItems());
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTOv1>> GetTodoItem(Guid id)
        {
            var item = await _service.GetTodoItem(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItemDTOv1 todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            bool success = await _service.ChangeTodoItem(todoItem);

            if (success)
            {
                return NoContent();
            }
            else if (_service.GetTodoItem(id) is null)
            {
                return NotFound();
            }
            else
            {
                // Couldn't change, still there, something weird.
                return BadRequest();
            }
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItemDTOv1>> PostTodoItem(TodoItemDTOv1 todoItem)
        {
            TodoItemDTOv1? created = await _service.CreateTodoItem(todoItem);
            if (created != null)
            {
                return CreatedAtAction(nameof(GetTodoItem), new { id = created.Id }, created);
            }
            else
                return BadRequest(); // gotta be a low level validation fail
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            if (await _service.GetTodoItem(id) is null)
            {
                return NotFound();
            }

            bool success = await _service.DeleteTodoItem(id);
            System.Diagnostics.Debug.Assert(success); // If there is a way for this to be false, what do I return?
            return NoContent();
        }
    }
}
