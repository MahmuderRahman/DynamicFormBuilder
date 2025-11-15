using DynamicFormBuilder.Models;
using DynamicFormBuilder.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicFormBuilder.Controllers
{
    public class DropdownController : Controller
    {
        private readonly IDropdownService _dropdownService;

        public DropdownController(IDropdownService dropdownService)
        {
            _dropdownService = dropdownService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dropdowns = await _dropdownService.GetAllDropdowns();
            var data = dropdowns.Select(d => new {
                id = d.Id,
                name = d.Name
            });
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DropdownMaster model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            var dropdown = await _dropdownService.CreateDropdown(model);
            return Json(new { success = true, dropdownId = dropdown.Id, dropdownName = dropdown.Name });
        }
    }
}
