using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicFormBuilder.Models
{
    [Table("DropdownMaster")]
    public class DropdownMaster
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

    }
}
