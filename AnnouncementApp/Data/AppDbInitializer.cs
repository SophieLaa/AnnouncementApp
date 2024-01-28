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




                if (!context.Announcements.Any())
                {
                    context.Announcements.AddRange(new List<Announcement>()
                    {
                        new Announcement()
                        {
                            Title = "Anouns 1",
                            Description = "This is the Bio of the first one",
                            PictureURL = "https://images.examples.com/wp-content/uploads/2018/06/Standard-Meeting-Announcement.jpg",
                            PhoneNumber = "555888888"
                        },
                        new Announcement()
                        {
                            Title = "Anouns 2",
                            Description = "This is the Bio of the next",
                            PictureURL = "https://c8.alamy.com/comp/R7YDR4/marketing-megaphone-announcement-promo-promotion-infographics-template-for-website-and-presentation-line-gray-icon-with-orange-infographic-style-R7YDR4.jpg",
                            PhoneNumber = "555888888"

                        },
                        new Announcement()
                        {
                            Title = "Anouns 3",
                            Description = "This is the Bio of the next",
                            PictureURL = "https://www.aplustopper.com/wp-content/uploads/2021/03/Sales-Announcement-Letters.png",
                            PhoneNumber = "555888888"

                        },
                        new Announcement()
                        {
                           Title = "Anouns 4",
                            Description = "This is the Bio of the next",
                            PictureURL = "https://marketplace.canva.com/EADan2tY9ls/2/0/1143w/canva-orange-agency-job-vacancy-announcement-Ym3ERvvTg2k.jpg",
                            PhoneNumber = "555888888"
                        },
                        new Announcement()
                        {
                            Title = "Anouns 5",
                            Description = "This is the Bio of the next",
                            PictureURL = "https://i.pinimg.com/originals/b0/6c/8f/b06c8f08837f13e379cdf52481cafa12.jpg",
                            PhoneNumber = "555888888"
                         }
                     });
                    context.SaveChanges();
                }
            }
        }
    }
}
