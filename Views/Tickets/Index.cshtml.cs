using ComunitateaMea.Authorization;
using ComunitateaMea.Data;
using ComunitateaMea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComunitateaMea.Views.Tickets
{
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IList<Ticket> Ticket { get; set; }

        public async Task OnGetAsync()
        {
            var tickets = from c in Context.Ticket
                           select c;

            var isAuthorized = User.IsInRole(Constants.TicketManagersRole) ||
                               User.IsInRole(Constants.TicketAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            // Only approved contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                tickets = tickets.Where(c => c.StatusApproval == TicketStatusApproval.Approved
                                            || c.OwnerId == currentUserId);
            }

            Ticket = await tickets.ToListAsync();
        }
    }
}
