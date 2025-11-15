using DynamicFormBuilder.Context;
using DynamicFormBuilder.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormBuilder.Service
{
    public interface IDropdownService
    {
        Task<List<DropdownMaster>> GetAllDropdowns();
        Task<DropdownMaster> CreateDropdown(DropdownMaster model);
    }

    public class DropdownService:IDropdownService
    {
        private readonly DynamicFormDbContext _context;

        public DropdownService(DynamicFormDbContext context)
        {
            _context = context;
        }

        public async Task<List<DropdownMaster>> GetAllDropdowns()
        {
            return await _context.DropdownMasters
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        public async Task<DropdownMaster> CreateDropdown(DropdownMaster model)
        {
            _context.DropdownMasters.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

    }
}
