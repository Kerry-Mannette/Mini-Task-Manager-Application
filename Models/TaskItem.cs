using System.ComponentModel.DataAnnotations;

namespace Mini_Task_Manager_Application.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
