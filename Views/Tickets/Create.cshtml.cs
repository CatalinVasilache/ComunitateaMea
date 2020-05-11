using ComunitateaMea.Authorization;
using ComunitateaMea.Data;
using ComunitateaMea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComunitateaMea.Views.Tickets
{
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(
        ApplicationDbContext context,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager)
        : base(context, authorizationService, userManager)
        {
        }

        public IActionResult OnGet()
        {
            Ticket = new Ticket
            {
                Title = "Copac uscat",
                Description = "Pe bulevardul Siderurgistilor la bloc SD7A in fata scarii (vezi poza de mai jos) se afla un copac mare si uscat care la prima furtuna va cadea!",
                PublishedDate = DateTime.Today,
                Votes = 8
            };
            return Page();
        }
        public Ticket Ticket { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Ticket.OwnerId = UserManager.GetUserId(User);

            // requires using TicketManager.Authorization;
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, Ticket,
                                                        TicketOperations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Context.Ticket.Add(Ticket);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
