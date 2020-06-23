using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComunitateaMea.Models
{
    public class VoteUserTicket
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string TicketId { get; set; }
    }
}
