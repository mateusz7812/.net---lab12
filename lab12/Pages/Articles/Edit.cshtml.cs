using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lab12.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using lab12.Models;
using System.IO;

namespace lab12.Pages_Articles
{
    public class EditModel : PageModel
    {
        private readonly lab12.Data.MyDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(lab12.Data.MyDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Article Article { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }
        
        public async Task<String> SaveFile(IFormFile file){
            string uploads = Path.Combine(_environment.WebRootPath, "upload");
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploads, fileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }

        public void RemoveFile(String name){
            string uploads = Path.Combine(_environment.WebRootPath, "upload");
            string filePath = Path.Combine(uploads, name);
            System.IO.File.Delete(filePath);
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Article
                .Include(a => a.Category).FirstOrDefaultAsync(m => m.ArticleId == id);

            if (Article == null)
            {
                return NotFound();
            }
           ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryID", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Article).State = EntityState.Modified;

            try
            {
                if(Image is not null){
                    if(Article.ImageName is not null){
                        RemoveFile(Article.ImageName);
                    }
                    
                    String fileName = await SaveFile(Image);
                    Article.ImageName = fileName;
                }
                _context.Update(Article);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.ArticleId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.ArticleId == id);
        }
    }
}
