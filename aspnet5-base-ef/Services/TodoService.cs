using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnet5_base_ef.Models;
using aspnet5_base_ef.DTOs;
using aspnet5_base_ef.Repositories;
using AutoMapper;

namespace aspnet5_base_ef.Services
{
    public class TodoItemsService : ITodoItemsService
    {
        private readonly ITodoItemsRepository _repository;
        private readonly IMapper _mapper;

        public TodoItemsService(ITodoItemsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IList<TodoItemDTOv1>> GetTodoItems()
        {
            IList<TodoItem>? models = await _repository.GetTodoItems();
            return models.Select(_mapper.Map<TodoItem, TodoItemDTOv1>).ToList();
        }

        public async Task<TodoItemDTOv1?> GetTodoItem(Guid id)
        {
            return _mapper.Map<TodoItemDTOv1>(await _repository.GetTodoItem(id));
        }

        public async Task<TodoItemDTOv1?> CreateTodoItem(TodoItemDTOv1 item)
        {
            TodoItem? itemModel = _mapper.Map<TodoItem>(item);
            itemModel.CreatedAt = DateTime.UtcNow;
            itemModel = await _repository.CreateTodoItem(itemModel); // Not sure how null flows through automapper and the next line
            return _mapper.Map<TodoItemDTOv1>(itemModel);
        }

        public async Task<bool> ChangeTodoItem(TodoItemDTOv1 item)
        {
            TodoItem? itemModel = _mapper.Map<TodoItem>(item);
            return await _repository.ChangeTodoItem(itemModel);
        }

        public async Task<bool> DeleteTodoItem(Guid id)
        {
            return await _repository.DeleteTodoItem(id);
        }
    }
}
