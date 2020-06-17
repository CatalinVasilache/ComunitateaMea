using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public TicketType Type{ get; set; }
        public TicketStatusApproval StatusApproval { get; set; }
        public TicketStatus Status { get; set; }
        public string County { get; set; }
        [DisplayName("Upload Name")]
        public string ImageName { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }

    public enum TicketStatusApproval
    {
        Submitted,
        Approved,
        Rejected    
    }

    public enum TicketStatus
    {
        Todo,
        InProgress,
        Done,
        NotPossible
    }

    public enum TicketType
    {
        Idea,
        Matter
    }
}
