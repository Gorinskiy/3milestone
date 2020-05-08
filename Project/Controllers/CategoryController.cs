using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models.Contexts;
using Project.Models.Entities;

namespace Project.Controllers
{
    public class CategoryController : Controller
    {
        private UserContext db;

        public CategoryController(UserContext userContext)
        {
            db = userContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await db.Categories.ToListAsync();
            return View(categories);
        }
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id != null)
            {
                Category category = await db.Categories.FirstOrDefaultAsync(p => p.Id == Id);
                if (category != null)
                    return View(category);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            db.Categories.Update(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Category model)
        {
            Category category = new Category() { CategoryName = model.CategoryName };
            db.Categories.Add(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/Category/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await db.Categories.FirstOrDefaultAsync(b => b.Id == id);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Category");
        }
    }
}