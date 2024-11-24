using BCMT_Associates.Models;
using Microsoft.AspNetCore.Mvc;

namespace BCMT_Associates.Controllers
{
    public class UserContactController : Controller
    {
        public IActionResult Index()
        {
            // Return the current contact info from the static repository
            return View(ContactRepository.ContactInfo);
        }
    }
}
