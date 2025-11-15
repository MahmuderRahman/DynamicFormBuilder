namespace DynamicFormBuilder.ViewModels
{
    public class FormCreateViewModel
    {
        public string Title { get; set; } = null!;
        public List<FormFieldViewModel> Fields { get; set; } = new List<FormFieldViewModel>();
    }

    public class FormFieldViewModel
    {
        public string Label { get; set; } = null!;
        public bool IsRequired { get; set; }
        public string FieldType { get; set; }
        public int? DropdownMasterId { get; set; }
    }
}
