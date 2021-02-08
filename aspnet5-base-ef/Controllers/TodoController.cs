using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Returns all Todo items  
        /// </summary>
        /// <remarks></remarks>
        /// <response code="200">All client records are returned</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoItemDTOv1>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TodoItemDTOv1>>> GetTodoItems()
        {
            return Ok(await _service.GetTodoItems());
        }

        /// <summary>
        /// Returns a specific Todo item
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id">guid of the requested record</param>
        /// <response code="200">The record was found and returned</response>
        /// <response code="401">The record was not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoItemDTOv1), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItemDTOv1>> GetTodoItem(Guid id)
        {
            TodoItemDTOv1? item = await _service.GetTodoItem(id);
            return item == null ? NotFound() : (ActionResult<TodoItemDTOv1>)Ok(item);
        }

        /// <summary>
        /// Change a specific Todo Item
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id">Guid of the requested Item</param>
        /// <param name="item">Item record</param>
        /// <response code="200">The record was successfully modified</response>
        /// <response code="400">Request failed validation</response>
        /// <response code="404">The record was not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItemDTOv1 item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            bool success = await _service.ChangeTodoItem(item);

            if (success)
            {
                return NoContent();
            }
            else if (await _service.GetTodoItem(id) is null)
            {
                return NotFound();
            }
            else
            {
                // Couldn't change, still there, something weird.
                return BadRequest();
            }
        }

        /// <summary>
        /// Create a specific Todo item
        /// </summary>
        /// <remarks></remarks>
        /// <param name="item">todo item to create, null id will be autogened</param>
        /// <response code="201">The record was successfully modified</response>
        /// <response code="400">Request failed validation</response>
        [HttpPost]
        [ProducesResponseType(typeof(TodoItemDTOv1), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoItemDTOv1>> PostTodoItem(TodoItemDTOv1 item)
        {
            TodoItemDTOv1? created = await _service.CreateTodoItem(item);
            if (created != null)
            {
                return CreatedAtAction(nameof(GetTodoItem), new { id = created.Id }, created);
            }
            else
            {
                return BadRequest(); // gotta be a low level validation fail
            }
        }

        /// <summary>
        /// Delete a specific Todo Item
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id">id of the target todo item</param>
        /// <response code="204">The record was successfully deleted</response>
        /// <response code="404">ID Not Found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
