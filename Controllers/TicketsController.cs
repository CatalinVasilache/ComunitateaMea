using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComunitateaMea.Data;
using ComunitateaMea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ComunitateaMea.Authorization;
using ComunitateaMea.Extensions;
using System.Diagnostics;
using System.Web;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNet.Identity;

namespace ComunitateaMea.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<AppUser> _userManager;


        public TicketsController(
            ApplicationDbContext context, 
            IAuthorizationService authorizationService, 
            UserManager<AppUser> userManager)
            : base()
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
        }

        public Ticket Ticket { get; set; }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var tickets = from c in _context.Ticket
                          select c;
            System.Diagnostics.Debug.WriteLine("tickets index get:", tickets);
            var isAuthorizedAdministrator = User.IsInRole(Constants.TicketAdministratorsRole);
            var isAuthorizedManager = User.IsInRole(Constants.TicketManagersRole);

            var currentUserId = _userManager.GetUserId(User);

            // Only approved tickets are shown UNLESS you're authorized to see them
            // or you are the owner.

            if (!(isAuthorizedAdministrator || isAuthorizedManager))
            {
                tickets = tickets.Where(c => (c.StatusApproval == TicketStatusApproval.Approved && c.County == User.GetCounty())
                                            || (c.OwnerId == currentUserId && c.County == User.GetCounty()));
            }

            if(isAuthorizedManager)
            {
                tickets = tickets.Where(c => (c.County == User.GetCounty()) || (c.OwnerId == currentUserId && c.County == User.GetCounty())).AsQueryable().OrderBy(c => c.Votes);
            }

            //Ticket = await tickets.ToListAsync();
            return View(await tickets.OrderByDescending(c => c.Votes).ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            Ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Ticket == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            var isAuthorized = User.IsInRole(Constants.TicketManagersRole) ||
                               User.IsInRole(Constants.TicketAdministratorsRole);

            var currentUserId = _userManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != Ticket.OwnerId
                && Ticket.StatusApproval != TicketStatusApproval.Approved)
            {
                return Forbid();
            }

            return View(Ticket);
        }

        // POST: Tickets/Details/Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(Guid id, TicketStatusApproval status, TicketStatus statusProgress)
        {
            var ticket = await _context.Ticket.FirstOrDefaultAsync(
                                                      m => m.Id == id);
            System.Diagnostics.Debug.WriteLine("ticket edit:", ticket);


            if (ticket == null)
            {
                return NotFound();
            }

            var ticketOperation = (status == TicketStatusApproval.Approved)
                                                       ? TicketOperations.Approve
                                                       : TicketOperations.Reject;

            if (statusProgress != TicketStatus.Todo)
            {
                status = TicketStatusApproval.Approved;
            }


            ticket.StatusApproval = status;
            ticket.Status = statusProgress;
            _context.Ticket.Update(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description")] Ticket ticket, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                ticket.Id = Guid.NewGuid();
                ticket.PublishedDate = DateTime.Today;
                ticket.Votes = 0;
                ticket.StatusApproval = TicketStatusApproval.Submitted;
                ticket.Status = TicketStatus.Todo;
                ticket.OwnerId = _userManager.GetUserId(User);
                ticket.County = User.GetCounty();
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Ticket = await _context.Ticket.FindAsync(id);
            if (Ticket == null)
            {
                return NotFound();
            }
            return View(Ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description,PublishedDate,Votes,OwnerId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ticket == null)
                    {
                        return NotFound();
                    }

                    //Ticket.OwnerId = ticket.OwnerId;

                    //_context.Attach(Ticket).State = EntityState.Modified;

                    //if (ticket.StatusApproval == TicketStatusApproval.Approved)
                    //{
                    //    // If the contact is updated after approval, 
                    //    // and the user cannot approve,
                    //    // set the status back to submitted so the update can be
                    //    // checked and approved.
                    //    var canApprove = await _authorizationService.AuthorizeAsync(User,
                    //                            ticket,
                    //                            TicketOperations.Approve);

                    //    if (!canApprove.Succeeded)
                    //    {
                    //        ticket.StatusApproval = TicketStatusApproval.Submitted;
                    //    }
                    //}
                  
                    ticket.PublishedDate = DateTime.Today;
                    ticket.OwnerId = _userManager.GetUserId(User);
                    ticket.County = User.GetCounty();
                    ticket.Status = TicketStatus.Todo;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            if (!(await _authorizationService.AuthorizeAsync(User, ticket, TicketOperations.Delete)).Succeeded)
            {
                return Forbid();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ticket = await _context.Ticket.FindAsync(id);

            //var isAuthorized = await _authorizationService.AuthorizeAsync(
            //                                         User, ticket,
            //                                         TicketOperations.Delete);
            //if (!isAuthorized.Succeeded)
            //{
            //    return Forbid();
            //}

            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Vote/5
        public async Task<IActionResult> Vote(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            var votesUserTicket = await _context.VoteUserTicket.FirstOrDefaultAsync(m => (m.UserId == _userManager.GetUserId(User) && m.TicketId == ticket.Id.ToString()));
            if(!(votesUserTicket == null))
            {
                return Forbid();
            }

            return View(ticket);
        }

        // POST: Tickets/Vote/5
        [HttpPost, ActionName("Vote")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VoteConfirmed(Guid id, VoteUserTicket voteUserTicket)
        {
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            //var ticket = await _context.Ticket
            //    .FirstOrDefaultAsync(m => m.Id == id);
            System.Diagnostics.Debug.WriteLine("ticket index post:", ticket);
            var votesUserTicket = await _context.VoteUserTicket.FirstOrDefaultAsync(m => (m.UserId == _userManager.GetUserId(User) && m.TicketId == ticket.Id.ToString()));
            if (votesUserTicket == null)
            {
                ticket.Votes += 1;
                voteUserTicket.UserId = _userManager.GetUserId(User);
                voteUserTicket.TicketId = ticket.Id.ToString();
                _context.VoteUserTicket.Add(voteUserTicket);
                _context.Ticket.Update(ticket);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
