using DynamicFormBuilder.Service;
using DynamicFormBuilder.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DynamicFormBuilder.Controllers
{
    public class FormController : Controller
    {
        private readonly IFormService _formService;

        public FormController(IFormService formService)
        {
            _formService = formService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] FormCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid form data" });
            }

            if (model.Fields == null || model.Fields.Count == 0)
            {
                return BadRequest(new { message = "Please add at least one field" });
            }

            _formService.SaveForm(model);

            return Ok(new { message = "Saved successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetForms(int start = 0, int length = 10, string? search = null)
        {
            var result = await _formService.GetFormList(start, length, search);
            return Json(result);
        }

    }
}
