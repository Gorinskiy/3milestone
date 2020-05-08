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
    public class AuthorController : Controller
    {
        private UserContext db;

        public AuthorController(UserContext userContext)
        {
            db = userContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var authors = await db.Authors.ToListAsync();
            return View(authors);
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id != null)
            {
                Author author = await db.Authors.FirstOrDefaultAsync(p => p.Id == Id);
                if (author != null)
                    return View();
            }
            return NotFound();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Author model)
        {
            Author author = new Author() {FullName = model.FullName };
            db.Authors.Add(author);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Author model)
        {
            db.Authors.Update(model);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("/Author/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Author author = await db.Authors.FirstOrDefaultAsync(b => b.Id == id);
            db.Authors.Remove(author);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Author");
        }
       
    }
}