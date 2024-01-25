using AnnouncementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementApp.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Announcement> Actors { get; set; }
    }
}
