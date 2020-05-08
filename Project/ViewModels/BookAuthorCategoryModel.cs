using Project.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class BookAuthorCategoryModel
    {
        public Book book { get; set; }  
        public IEnumerable<Author> authors { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<BookCategory> bookCategories { get; set; }

    }
}
