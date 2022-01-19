using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using lab12.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using lab12.Models;
using System.IO;

namespace lab12.Pages_ArticlesViewModel
{
    public class CreateModel : PageModel
    {
        private readonly lab12.Data.MyDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(lab12.Data.MyDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<String> SaveFile(IFormFile file){
            string uploads = Path.Combine(_environment.WebRootPath, "upload");
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploads, fileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryID", "Name");
            return Page();
        }

        [BindProperty]
        public ArticleViewModel ArticleViewModel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IFormFile file = ArticleViewModel.Image;
            Article article = new Article(){
                ArticleId = ArticleViewModel.ArticleId,
                CategoryId = ArticleViewModel.CategoryId,
                Money = ArticleViewModel.Money,
                Name = ArticleViewModel.Name
            };

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(file is not null){
                String fileName = await SaveFile(file);
                article.ImageName = fileName;
            }
            _context.Add(article);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
