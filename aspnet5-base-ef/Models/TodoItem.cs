using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnet5_base_ef.Models
{
    public class TodoItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!; // Null forgiving operator, will never be null as accessed
        [Required]
        public bool IsComplete { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
