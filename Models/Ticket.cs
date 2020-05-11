using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComunitateaMea.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        //user ID from AspNetUser table
        public string OwnerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }
        public int Votes { get; set; }

        public TicketStatusApproval StatusApproval { get; set; }
        public TicketStatus Status { get; set; }
    }

    public enum TicketStatusApproval
    {
        Submitted,
        Approved,
        Rejected
    }

    public enum TicketStatus
    {
        Done,
        InProgress,
        Rejected
    }
}
