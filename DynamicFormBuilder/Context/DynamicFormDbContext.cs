using DynamicFormBuilder.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormBuilder.Context
{
    public class DynamicFormDbContext: DbContext
    {
        public DynamicFormDbContext(DbContextOptions<DynamicFormDbContext> options) : base(options)
        {
        }
        public DbSet<Form> Forms { get; set; }
        public DbSet<FormField> FormFields { get; set; }
        public DbSet<DropdownMaster> DropdownMasters { get; set; }

    }

}
