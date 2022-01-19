using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using lab12.Data;
using lab12.Models;

namespace lab12.Pages_Categories
{
    public class DeleteModel : PageModel
    {
        private readonly lab12.Data.MyDbContext _context;

        public DeleteModel(lab12.Data.MyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; }

        public Boolean HasArticles { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Category.FirstOrDefaultAsync(m => m.CategoryID == id);

            if (Category == null)
            {
                return NotFound();
            }
            
            HasArticles = _context.Article.Any(m=>m.CategoryId == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Category.FindAsync(id);

            if(_context.Article.Any(m=>m.CategoryId == id)){
                return BadRequest();
            }
            
            if (Category != null)
            {
                _context.Category.Remove(Category);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
