using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynamicFormBuilder.Models
{
    [Table("FormFields")]
    public class FormField
    {
        [Key]
        public int FieldId { get; set; }        
        public int FormId { get; set; }
        [ForeignKey("FormId")]
        public Form Form { get; set; }
        public string Label { get; set; } = null!;
        public bool IsRequired { get; set; }
        [Required, StringLength(50)]
        public string FieldType { get; set; }

        public int? DropdownMasterId { get; set; }

        [ForeignKey("DropdownMasterId")]
        public DropdownMaster? DropdownMaster { get; set; }

    } 

}
