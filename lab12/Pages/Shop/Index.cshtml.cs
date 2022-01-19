using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using lab12.Data;
using lab12.Models;

namespace lab12.Pages_Shop
{
    public class IndexModel : PageModel
    {
        private readonly lab12.Data.MyDbContext _context;

        public IndexModel(lab12.Data.MyDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        public IList<Article> Article { get;set; }

        public async Task OnGetAsync()
        {
            var myDbContext = _context.Article.AsQueryable();
            if(CategoryId is not null){
                myDbContext = myDbContext.Where(a => a.CategoryId == CategoryId);
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryID", "Name", CategoryId);

            Article = await myDbContext.Include(a => a.Category).ToListAsync();
        }
    }
}
