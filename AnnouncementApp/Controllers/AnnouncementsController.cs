using AnnouncementApp.Data;
using AnnouncementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AnnouncementApp.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly AppDbContext _context;

        public AnnouncementsController(AppDbContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    var data = _context.Announcements.ToList();
        //    return View(data);
        //}


        public IActionResult Details(int id)
        {
            var announcement = _context.Announcements.FirstOrDefault(a => a.Id == id);

            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        public IActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Index(string searchString)
        //{
        //    var data = _context.Announcements.ToList();

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        var filteredAnnouncements = data
        //            .Where(a => string.Equals(a.Title, searchString, StringComparison.CurrentCultureIgnoreCase) ||
        //                        string.Equals(a.Description, searchString, StringComparison.CurrentCultureIgnoreCase))
        //            .ToList();

        //        return View(filteredAnnouncements);
        //    }

        //    return View(data);
        //}

        [HttpGet]
        public IActionResult Index()
        {
            var data = _context.Announcements.ToList();
            return View(data);
        }

      
        //[HttpGet]
        //public IActionResult Search(string searchString)
        //{
        //    var data = _context.Announcements.ToList();

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        var filteredAnnouncements = data
        //            .Where(a => string.Equals(a.Title, searchString, StringComparison.CurrentCultureIgnoreCase) ||
        //                        string.Equals(a.Description, searchString, StringComparison.CurrentCultureIgnoreCase))
        //            .ToList();

        //        return View("Index", filteredAnnouncements);
        //    }

        //    return View("Index", data);
        //}
        [HttpGet]
        public IActionResult Search(string searchString)
        {
            var data = _context.Announcements.ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim();
                var filteredAnnouncements = data
                    .Where(a => a.Title != null && a.Title.StartsWith(searchString, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();

                return View("Index", filteredAnnouncements);
            }

            return View("Index", data);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Description,PictureURL,PhoneNumber")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(announcement);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(announcement);
        }

        public IActionResult Edit(int id)
        {
            var announcement = _context.Announcements.FirstOrDefault(a => a.Id == id);

            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Description,PictureURL,PhoneNumber")] Announcement updatedAnnouncement)
        {
            if (id != updatedAnnouncement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingAnnouncement = _context.Announcements.Find(id);

                    if (existingAnnouncement == null)
                    {
                        return NotFound();
                    }

                    existingAnnouncement.Title = updatedAnnouncement.Title;
                    existingAnnouncement.Description = updatedAnnouncement.Description;
                    existingAnnouncement.PictureURL = updatedAnnouncement.PictureURL;
                    existingAnnouncement.PhoneNumber = updatedAnnouncement.PhoneNumber;

                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error updating announcement: " + ex.Message;
                    return View("Error");
                }
            }

            return View(updatedAnnouncement);
        }

        
        public IActionResult Delete(int id)
        {
            var announcement = _context.Announcements.FirstOrDefault(a => a.Id == id);

            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var announcement = _context.Announcements.FirstOrDefault(a => a.Id == id);

                if (announcement == null)
                {
                    return NotFound();
                }

                _context.Announcements.Remove(announcement);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error deleting announcement: " + ex.Message;
                return View("Error");
            }
        }



        private bool AnnouncementExists(int id)
        {
            return _context.Announcements.Any(a => a.Id == id);
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
 