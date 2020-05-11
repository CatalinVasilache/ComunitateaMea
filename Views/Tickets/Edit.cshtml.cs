using ComunitateaMea.Authorization;
using ComunitateaMea.Data;
using ComunitateaMea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComunitateaMea.Views.Tickets
{
    public class EditModel : DI_BasePageModel
    {
        public EditModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Ticket Ticket { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Ticket = await Context.Ticket.FirstOrDefaultAsync(
                                                 m => m.Id == id);

            if (Ticket == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                      User, Ticket,
                                                      TicketOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch Contact from DB to get OwnerID.
            var ticket = await Context
                .Ticket.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, ticket,
                                                     TicketOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Ticket.OwnerId = ticket.OwnerId;

            Context.Attach(Ticket).State = EntityState.Modified;

            if (Ticket.StatusApproval == TicketStatusApproval.Approved)
            {
                // If the contact is updated after approval, 
                // and the user cannot approve,
                // set the status back to submitted so the update can be
                // checked and approved.
                var canApprove = await AuthorizationService.AuthorizeAsync(User,
                                        Ticket,
                                        TicketOperations.Approve);

                if (!canApprove.Succeeded)
                {
                    Ticket.StatusApproval = TicketStatusApproval.Submitted;
                }
            }

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
