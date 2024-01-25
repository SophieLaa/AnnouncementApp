using AnnouncementApp.Models;

namespace AnnouncementApp.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();




                if (!context.Actors.Any())
                {
                    context.Actors.AddRange(new List<Announcement>()
                    {
                        new Announcement()
                        {
                            Title = "Anouns 1",
                            Description = "This is the Bio of the first one",
                            PictureURL = "http://dotnethow.net/images/actors/actor-1.jpeg",
                            PhoneNumber = "555888888"
                        },
                        new Announcement()
                        {
                            Title = "Anouns 2",
                            Description = "This is the Bio of the next",
                            PictureURL = "http://dotnethow.net/images/actors/actor-2.jpeg",
                            PhoneNumber = "555888888"

                        },
                        new Announcement()
                        {
                            Title = "Anouns 3",
                            Description = "This is the Bio of the next",
                            PictureURL = "http://dotnethow.net/images/actors/actor-3.jpeg",
                            PhoneNumber = "555888888"

                        },
                        new Announcement()
                        {
                           Title = "Anouns 4",
                            Description = "This is the Bio of the next",
                            PictureURL = "http://dotnethow.net/images/actors/actor-3.jpeg",
                            PhoneNumber = "555888888"
                        },
                        new Announcement()
                        {
                            Title = "Anouns 5",
                            Description = "This is the Bio of the next",
                            PictureURL = "http://dotnethow.net/images/actors/actor-3.jpeg",
                            PhoneNumber = "555888888"
                         }
                     });
                    context.SaveChanges();
                }
            }
        }
    }
}
