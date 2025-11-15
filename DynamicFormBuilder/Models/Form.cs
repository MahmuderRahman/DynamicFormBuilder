using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicFormBuilder.Models
{
    [Table("Forms")]
    public class Form
    {
        [Key]
        public int FormId { get; set; }
        [Required]
        [StringLength(255)]
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<FormField> FormFields { get; set; }
    }
}
