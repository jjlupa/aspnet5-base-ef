using System;

namespace aspnet5_base_ef.DTOs
{
    public class TodoItemDTOv1
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!; // will never be null, but could be empty.
        public bool IsComplete { get; set; }
    }
}
