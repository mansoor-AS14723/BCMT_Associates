using BCMT_Associates.Context;
using BCMT_Associates.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BCMT_Associates.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly AppDBContext _context;

        public PublicationsController(AppDBContext context)
        {
            _context = context;
        }

        //******************************************************************Public Index******************************************************************************** 

        public IActionResult PublicIndex()
        {
            var publications = _context.Publications
                .Where(c => c.IsVisibleOnMainPage) // Only show publications marked visible for general users
                .ToList();

            return View(publications); // View that excludes CRUD options
        }

        //******************************************************************Index******************************************************************************** 
        public IActionResult Index()
        {
            var publications = _context.Publications.ToList();
            return View(publications); // Main view for listing publications
        }

        //******************************************************************Add View Get******************************************************************************** 
        public IActionResult Add()
        {
            return PartialView("_AddPublicationPartial");
        }

        //******************************************************************Add View Post******************************************************************************** 
        [HttpPost]
        public IActionResult Add(Publication publication)
        {
            if (ModelState.IsValid)
            {
                _context.Publications.Add(publication);
                _context.SaveChanges();

                return Json(new { success = true, publication = new { id = publication.Id, title = publication.Title, journalName = publication.JournalName, datePublished = publication.DatePublished, issn = publication.ISSN } });
            }

            return Json(new { success = false, message = "Invalid data" });
        }

        //******************************************************************Edit Get******************************************************************************** 
        public IActionResult Edit(int id)
        {
            var publication = _context.Publications.Find(id);
            return PartialView("_EditPublicationPartial", publication);
        }

        //******************************************************************Edit Post******************************************************************************** 
        [HttpPost]
        public IActionResult Edit(Publication publication)
        {
            var existingPublication = _context.Publications.Find(publication.Id);
            if (existingPublication != null)
            {
                existingPublication.Title = publication.Title;
                existingPublication.JournalName = publication.JournalName;
                existingPublication.DatePublished = publication.DatePublished;
                existingPublication.ISSN = publication.ISSN;
                existingPublication.IsVisibleOnMainPage = publication.IsVisibleOnMainPage;

                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Publication not found" });
        }

        //******************************************************************Delete Get******************************************************************************** 
        public IActionResult Delete(int id)
        {
            var publication = _context.Publications.Find(id);
            return PartialView("_DeletePublicationPartial", publication);
        }

        //******************************************************************Delete Post******************************************************************************** 
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var publication = await _context.Publications.FindAsync(id);
                if (publication == null)
                {
                    return Json(new { success = false, message = "Publication not found or already deleted." });
                }

                _context.Publications.Remove(publication);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Publication deleted successfully.", publicationId = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while deleting the publication." });
            }
        }
    }
}
