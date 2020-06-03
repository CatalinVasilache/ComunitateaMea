//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ComunitateaMea.Authorization;
//using ComunitateaMea.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;

//namespace ComunitateaMea.Data
//{
//    public static class SeedData
//    {
//        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
//        {
//            using (var context = new ApplicationDbContext(
//                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
//            {
//                // For sample purposes seed both with the same password.
//                // Password is set with the following:
//                // dotnet user-secrets set SeedUserPW <pw>
//                // The admin user can do anything

//                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@yahoo.com");
//                await EnsureRole(serviceProvider, adminID, Constants.TicketAdministratorsRole);

//                // allowed user can create and edit contacts that they create
//                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@yahoo.com");
//                await EnsureRole(serviceProvider, managerID, Constants.TicketManagersRole);

//                SeedDB(context, adminID);
//            }
//        }

//        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
//                                                    string testUserPw, string UserName)
//        {
//            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

//            var user = await userManager.FindByNameAsync(UserName);
//            if (user == null)
//            {
//                user = new IdentityUser
//                {
//                    UserName = UserName,
//                    EmailConfirmed = true
//                };
//                await userManager.CreateAsync(user, testUserPw);
//            }

//            if (user == null)
//            {
//                throw new Exception("The password is probably not strong enough!");
//            }

//            return user.Id;
//        }

//        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
//                                                                      string uid, string role)
//        {
//            IdentityResult IR = null;
//            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

//            if (roleManager == null)
//            {
//                throw new Exception("roleManager null");
//            }

//            if (!await roleManager.RoleExistsAsync(role))
//            {
//                IR = await roleManager.CreateAsync(new IdentityRole(role));
//            }

//            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

//            var user = await userManager.FindByIdAsync(uid);

//            if (user == null)
//            {
//                throw new Exception("The testUserPw password was probably not strong enough!");
//            }

//            IR = await userManager.AddToRoleAsync(user, role);

//            return IR;
//        }

//        public static void SeedDB(ApplicationDbContext context, string adminID)
//        {
//            if (context.Ticket.Any())
//            {
//                return;   // DB has been seeded
//            }

//            context.Ticket.AddRange(
//                new Ticket
//                {
//                    Title = "Capac de canalizare",
//                    Description = "Lipseste capacul de canalizare de pe strada Brailei!",
//                    PublishedDate = DateTime.Today,
//                    Votes = 4,
//                    StatusApproval = TicketStatusApproval.Approved,
//                    OwnerId = adminID
//                },
//                new Ticket
//                {
//                    Title = "Copac uscat",
//                    Description = "Pe bulevardul Siderurgistilor la bloc SD7A in fata scarii (vezi poza de mai jos) se afla un copac mare si uscat care la prima furtuna va cadea!",
//                    PublishedDate = DateTime.Today,
//                    Votes = 8,
//                    StatusApproval = TicketStatusApproval.Rejected,
//                    OwnerId = adminID
//                },
//                new Ticket
//                {
//                    Title = "Vandali de masini",
//                    Description = "In cartierul Tiglina la A-uri masinile locatarilor din cartier sunt des zgariate de catre anumite persoane neidentificate inca.",
//                    PublishedDate = DateTime.Today,
//                    Votes = 2,
//                    StatusApproval = TicketStatusApproval.Approved,
//                    OwnerId = adminID
//                },
//                 new Ticket
//                 {
//                     Title = "Largirea strazii Traian",
//                     Description = "Intrucat traficul este intens pe strada Traian, propun largirea strazii.",
//                     PublishedDate = DateTime.Today,
//                     Votes = 10,
//                     StatusApproval = TicketStatusApproval.Submitted,
//                     OwnerId = adminID
//                 },
//                 new Ticket
//                 {
//                     Title = "Deranjarea linistii publice",
//                     Description = "Tineri in jur de 18-25 de ani isi petrec timpul in parcul de la Spicu consumand bauturi alcoolice si facand galagie.",
//                     PublishedDate = DateTime.Today,
//                     Votes = 999,
//                     StatusApproval = TicketStatusApproval.Submitted,
//                     OwnerId = adminID
//                 }
//             );
//            context.SaveChanges();
//        }
//    }
//}
