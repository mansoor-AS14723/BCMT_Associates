using Microsoft.AspNetCore.Mvc;
using BCMT_Associates.Models;


namespace BCMT_Associates.Controllers
{
    public class AdminContactController : Controller
    {
        public IActionResult Index()
        {
            // Pass the current static contact information to the admin view
            return View(ContactRepository.ContactInfo);
        }

        // Edit contact (GET method to load the form inside the modal)
        [HttpGet]
        public IActionResult Edit()
        {
            return PartialView("_CreateEditContact", ContactRepository.ContactInfo); // Returns the modal content
        }

        // Edit contact (POST method to save changes via AJAX)
        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                // Update the static ContactInfo with new values
                ContactRepository.ContactInfo = contact;

                // Return a success response as JSON
                return Json(new { success = true });
            }

            // Return validation errors if the model state is invalid
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
        }

        // API endpoint for AJAX to fetch the latest contact info (used for user view)
        [HttpGet]
        public JsonResult GetContactInfo()
        {
            return Json(ContactRepository.ContactInfo); // Returns the updated contact information
        }
    }
}
