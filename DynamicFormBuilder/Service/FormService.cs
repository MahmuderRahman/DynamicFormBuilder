using DynamicFormBuilder.Context;
using DynamicFormBuilder.Models;
using DynamicFormBuilder.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;

namespace DynamicFormBuilder.Service
{
    public interface IFormService
    {
        void SaveForm(FormCreateViewModel model);

        Task<object> GetFormList(int start, int length, string? search);

    }

    public class FormService : IFormService
    {
        private readonly DynamicFormDbContext _context;

        public FormService(DynamicFormDbContext context)
        {
            _context = context;
        }

        public void SaveForm(FormCreateViewModel model)
        {
            var form = new Form
            {
                Title = model.Title,
                CreatedAt = DateTime.Now
            };

            var formFields = model.Fields.Select(f => new FormField
            {
                Form = form,
                Label = f.Label,
                IsRequired = f.IsRequired,
                FieldType = f.FieldType,
                DropdownMasterId = f.DropdownMasterId
            }).ToList();

            _context.Forms.Add(form);
            _context.FormFields.AddRange(formFields);
            _context.SaveChanges();
        }

        public async Task<object> GetFormList(int start, int length, string? search)
        {
            var query = _context.Forms
                .Include(f => f.FormFields)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(f => f.Title.Contains(search));
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(f => f.CreatedAt)
                .Skip(start)
                .Take(length)
                .Select(f => new
                {
                    f.FormId,
                    f.Title,
                    CreatedAt = f.CreatedAt.ToString("yyyy-MM-dd"),
                    Fields = f.FormFields.Select(ff => new
                    {
                        ff.Label,
                        ff.IsRequired,
                        ff.DropdownMasterId,
                        selectedOption = ff.DropdownMaster.Name
                        
                    }).ToList()
                })
                .ToListAsync();

            return new
            {
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = data
            };
        }

    }
}
