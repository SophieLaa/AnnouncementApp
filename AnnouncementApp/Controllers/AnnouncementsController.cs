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

        public IActionResult Index()
        {
            var data = _context.Announcements.ToList();
            return View(data);
        }

     
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,PictureURL,PhoneNumber")] Announcement announcement)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                    _context.Announcements.Add(announcement);
                    await _context.SaveChangesAsync();

                  
                    var httpClient = _httpClientFactory.CreateClient("AnnouncementAppAPI");

                    try
                    {
                      
                        var response = await httpClient.PostAsJsonAsync("api/AnnouncementsAPI", announcement);

                        if (response.IsSuccessStatusCode)
                        {
                           
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            
                            ViewBag.ErrorMessage = $"Error creating announcement in the Web API. Status code: {response.StatusCode}";
                            return View("Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = $"Error creating announcement in the Web API: {ex.Message}";
                        return View("Error");
                    }
                }
                return View(announcement);
            }
            catch (Exception ex)
            {
               
                ViewBag.ErrorMessage = $"Error creating announcement: {ex.Message}";
                return View("Error");
            }
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

                  
                    var httpClient = _httpClientFactory.CreateClient("AnnouncementAppAPI");

                    try
                    {
                        var response = await httpClient.PutAsJsonAsync($"api/AnnouncementsAPI/{id}", updatedAnnouncement);

                        if (!response.IsSuccessStatusCode)
                        {
                            ViewBag.ErrorMessage = "Error updating announcement in the Web API";
                            return View("Error");
                        }
                    }
                    catch (Exception ex)
                    {
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

      
        public IActionResult Delete(int id)
        {
            var announcement = _context.Announcements.FirstOrDefault(a => a.Id == id);

            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

       
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

                
                var httpClient = _httpClientFactory.CreateClient("AnnouncementAppAPI");

                try
                {
                    var response = await httpClient.DeleteAsync($"api/AnnouncementsAPI/{id}");

                    if (!response.IsSuccessStatusCode)
                    {
                       
                        ViewBag.ErrorMessage = "Error deleting announcement in the Web API";
                        return View("Error");
                    }
                }
                catch (Exception ex)
                {
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
        public IActionResult Search(string searchString)
        {
            searchString = searchString?.ToLower();

            var announcements = _context.Announcements
                .Where(a => a.Title.ToLower().Contains(searchString) || a.Description.ToLower().Contains(searchString))
                .ToList();

            return View("Index", announcements);
        }


    }
}
