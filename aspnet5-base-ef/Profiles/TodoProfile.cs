
using AutoMapper;
using aspnet5_base_ef.DTOs;
using aspnet5_base_ef.Models;

namespace aspnet5_base_ef.Profiles
{
    class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<TodoItem, TodoItemDTOv1>()
                .ForMember(dest =>
                    dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest =>
                    dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest =>
                    dest.IsComplete,
                    opt => opt.MapFrom(src => src.IsComplete))
                .ReverseMap();
        }
    }
}