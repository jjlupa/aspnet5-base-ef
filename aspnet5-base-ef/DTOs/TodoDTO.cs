using System;

namespace aspnet5_base_ef.DTOs
{
    /// <summary>
    /// A Todo Item represents a note about a future task to complete. 
    /// </summary>
    public class TodoItemDTOv1
    {
        /// <summary>
        /// Unique ID of each record
        /// </summary>
        /// <example>
        /// 27da5195-d870-42dc-a339-f5489449a1d1
        /// </example>
        public Guid Id { get; set; }
        /// <summary>
        /// String text of the TODO item.
        /// </summary>
        /// <example>
        /// Write unit tests.
        /// </example>
        public string Name { get; set; } = null!; // will never be null, but could be empty.
        /// <summary>
        /// A boolean value indicating if this TODO item has been completed
        /// </summary>
        /// <example>
        /// false
        /// </example>
        public bool IsComplete { get; set; }
    }
}
