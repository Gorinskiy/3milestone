using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Project.Models.Contexts;
using Project.Models.Entities;
using Project.ViewModels;

namespace Project.Controllers
{
    public class BookController : Controller
    {
        private UserContext db;

        public BookController(UserContext userContext)
        {
            db = userContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await db.Books.Where(b => b.isFree == true).ToListAsync();
            foreach(var book in books)
            {
                book.Author = await db.Authors.FirstOrDefaultAsync(a => a.Id == book.AuthorId);
            }
            return View("Index",books);
        }

        [HttpGet]
        [Route("/Book/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            Book book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
            var authors = await db.Authors.ToListAsync();
            var categories = await db.Categories.ToListAsync();
            var bookCategories = await db.BookCategories.Where(b => b.BookId == id).ToListAsync();
            BookAuthorCategoryModel model = new BookAuthorCategoryModel { book = book, authors = authors};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookAuthorCategoryModel model)
        {
            Book book = await db.Books.FirstOrDefaultAsync(u => u.Id == model.book.Id);
            book.AuthorId = model.book.AuthorId;
            book.Title = model.book.Title;
            db.Books.Update(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Book");
        }

        [HttpGet]
        [Route("/Book/EditCategory/{id}")]
        public async Task<IActionResult> EditCategory(int id)
        {
            Book book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
            var categories = await db.Categories.ToListAsync();
            var bookCategories = await db.BookCategories.Where(b => b.BookId == id).ToListAsync();
            BookAuthorCategoryModel model = new BookAuthorCategoryModel { book = book, categories = categories, bookCategories = bookCategories };

            return View(model);
        }


        [HttpGet]
        [Route("/Book/AddCategory/{bookid}/{id}")]
        public async Task<IActionResult> AddCategory(int bookid, int id)
        {
            Book book = await db.Books.FirstOrDefaultAsync(b => b.Id == bookid);
            var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == bookid);
            BookCategory bookCategory = await db.BookCategories.FirstOrDefaultAsync(b => b.BookId == bookid && b.CategoryId == id);
            if(bookCategory == null)
            {
                db.BookCategories.Add(new BookCategory {BookId = bookid, CategoryId = id });
                await db.SaveChangesAsync();
            }
            return RedirectToAction("EditCategory", new RouteValueDictionary(
    new { controller = "Book", action = "EditCategory", Id = bookid }));
        }

        [HttpGet]
        [Route("/Book/RemoveCategory/{bookid}/{id}")]
        public async Task<IActionResult> RemoveCategory(int bookid, int id)
        {
            Book book = await db.Books.FirstOrDefaultAsync(b => b.Id == bookid);
            var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == bookid);
            BookCategory bookCategory = await db.BookCategories.FirstOrDefaultAsync(b => b.BookId == bookid && b.CategoryId == id);
            if (bookCategory != null)
            {
                db.BookCategories.Remove(bookCategory);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("EditCategory", new RouteValueDictionary(
    new { controller = "Book", action = "EditCategory", Id = bookid }));
        }

        [HttpGet]
        [Route("/Book/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Book book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
            db.Books.Remove(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Book");
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var authors = await db.Authors.ToListAsync();
            var categories = await db.Categories.ToListAsync();
            BookAuthorCategoryModel model = new BookAuthorCategoryModel {authors = authors };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookAuthorCategoryModel model)
        {
            Book book = new Book() { AuthorId = model.book.AuthorId, Title = model.book.Title, isFree = true };

            db.Books.Add(book);
            await db.SaveChangesAsync();
            return RedirectToAction("EditCategory", new RouteValueDictionary(
    new { controller = "Book", action = "EditCategory", Id = book.Id}));
        }


        }
    }
