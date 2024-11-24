using BCMT_Associates.Context;
using BCMT_Associates.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BCMT_Associates.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDBContext _context;

        public CoursesController(AppDBContext context)
        {
            _context = context;
        }

        //******************************************************************Public Index******************************************************************************** 

        public IActionResult PublicIndex()
        {
            var courses = _context.CoursesA
                .Where(c => c.IsVisibleOnMainPage) // Only show courses marked visible for general users
                .ToList();

            return View(courses); // View that excludes CRUD options
        }

        //******************************************************************Index******************************************************************************** 
        // Admin dashboard - restricted to Admin role only
        public IActionResult Index()
        {
            var courses = _context.CoursesA.ToList();
            return View(courses);
        }

        //******************************************************************Add View Get******************************************************************************** 
        public IActionResult Add()
        {
            return PartialView("_AddCoursePartial");
        }

        //******************************************************************Add View Post******************************************************************************** 
        [HttpPost]
        public IActionResult Add(Course course, IFormFile? ImageData)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"IsVisibleOnMainPage: {course.IsVisibleOnMainPage}");
                if (ImageData != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        ImageData.CopyTo(memoryStream);
                        course.ImageData = memoryStream.ToArray();
                    }
                }

                _context.CoursesA.Add(course);
                _context.SaveChanges();

                return Json(new { success = true, course = new { id = course.Id, name = course.Name, description = course.Description, imageData = course.ImageData != null ? Convert.ToBase64String(course.ImageData) : null } });
            }

            return Json(new { success = false, message = "Invalid data" });
        }

        //******************************************************************Edit Get******************************************************************************** 
        public IActionResult Edit(int id)
        {
            var course = _context.CoursesA.Find(id);
            return PartialView("_EditCoursePartial", course);
        }

        //******************************************************************Edit Post******************************************************************************** 
        [HttpPost]
        public IActionResult Edit(Course course, IFormFile? ImageData)
        {
            var existingCourse = _context.CoursesA.Find(course.Id);
            if (existingCourse != null)
            {
                existingCourse.Name = course.Name;
                existingCourse.Description = course.Description;
                existingCourse.IsVisibleOnMainPage = course.IsVisibleOnMainPage;

                if (ImageData != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        ImageData.CopyTo(memoryStream);
                        existingCourse.ImageData = memoryStream.ToArray();
                    }
                }

                _context.SaveChanges();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Course not found" });
        }

        //******************************************************************Delete Get******************************************************************************** 
        public IActionResult Delete(int id)
        {
            var course = _context.CoursesA.Find(id);
            return PartialView("_DeleteCoursePartial", course);
        }

        //******************************************************************Delete Post******************************************************************************** 
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var course = await _context.CoursesA.FindAsync(id);
                if (course == null)
                {
                    return Json(new { success = false, message = "Course not found or has already been deleted." });
                }

                _context.CoursesA.Remove(course);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Course deleted successfully.", courseId = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while deleting the course." });
            }
        }
    }
}
