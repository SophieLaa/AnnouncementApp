using AnnouncementApp.Data;
using AnnouncementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnnouncementApp.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public AnnouncementsController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: Announcements
        public IActionResult Index()
        {
            var data = _context.Announcements.ToList();
            return View(data);
        }

        // GET: Announcements/Details/5
        public IActionResult Details(int id)
        {
            var announcement = _context.Announcements.FirstOrDefault(a => a.Id == id);

            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // GET: Announcements/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Announcements/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Title,Description,PictureURL,PhoneNumber")] Announcement announcement)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(announcement);
        //       // _context.SaveChanges();
        //        await _context.SaveChangesAsync();


        //        // Use the Web API to create the same announcement
        //        var httpClient = _httpClientFactory.CreateClient("AnnouncementAppAPI");

        //        try
        //        {
        //            var response = await httpClient.PostAsJsonAsync("api/AnnouncementsAPI", announcement);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Successfully created in the Web API, redirect to Index
        //                return RedirectToAction(nameof(Index));
        //            }
        //            else
        //            {
        //                // Log or handle the failure from the Web API
        //                ViewBag.ErrorMessage = "Error creating announcement in the Web API";
        //                return View("Error");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log or handle the exception
        //            ViewBag.ErrorMessage = "Error creating announcement in the Web API: " + ex.Message;
        //            return View("Error");
        //        }
        //    }
        //    return View(announcement);
        //}


        // GET: Announcements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Announcements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,PictureURL,PhoneNumber")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                // Add to local database
                _context.Announcements.Add(announcement);
                await _context.SaveChangesAsync();

                // Use the Web API to create the same announcement
                var httpClient = _httpClientFactory.CreateClient("AnnouncementAppAPI");

                try
                {
                    // Post to Web API
                    var response = await httpClient.PostAsJsonAsync("api/AnnouncementsAPI", announcement);

                    if (response.IsSuccessStatusCode)
                    {
                        // Successfully created in the Web API, redirect to Index
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Log or handle the failure from the Web API
                        ViewBag.ErrorMessage = "Error creating announcement in the Web API";
                        return View("Error");
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    ViewBag.ErrorMessage = "Error creating announcement in the Web API: " + ex.Message;
                    return View("Error");
                }
            }

            return View(announcement);
        }


        // GET: Announcements/Edit/5
        public IActionResult Edit(int id)
        {
            var announcement = _context.Announcements.FirstOrDefault(a => a.Id == id);

            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Announcements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,PictureURL,PhoneNumber")] Announcement updatedAnnouncement)
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

                    // Use the Web API to update the same announcement
                    var httpClient = _httpClientFactory.CreateClient("AnnouncementAppAPI");

                    try
                    {
                        var response = await httpClient.PutAsJsonAsync($"api/AnnouncementsAPI/{id}", updatedAnnouncement);

                        if (!response.IsSuccessStatusCode)
                        {
                            // Log or handle the failure from the Web API
                            ViewBag.ErrorMessage = "Error updating announcement in the Web API";
                            return View("Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception
                        ViewBag.ErrorMessage = "Error updating announcement in the Web API: " + ex.Message;
                        return View("Error");
                    }

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

        // GET: Announcements/Delete/5
        public IActionResult Delete(int id)
        {
            var announcement = _context.Announcements.FirstOrDefault(a => a.Id == id);

            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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

                // Use the Web API to delete the same announcement
                var httpClient = _httpClientFactory.CreateClient("AnnouncementAppAPI");

                try
                {
                    var response = await httpClient.DeleteAsync($"api/AnnouncementsAPI/{id}");

                    if (!response.IsSuccessStatusCode)
                    {
                        // Log or handle the failure from the Web API
                        ViewBag.ErrorMessage = "Error deleting announcement in the Web API";
                        return View("Error");
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    ViewBag.ErrorMessage = "Error deleting announcement in the Web API: " + ex.Message;
                    return View("Error");
                }

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
            return _context.Announcements.Any(e => e.Id == id);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
