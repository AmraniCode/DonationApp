using System;
using System.Collections.Generic;

namespace ChurchApp.Models
{
    public partial class Church
    {
        public Church()
        {
            Donations = new HashSet<Donation>();
            Members = new HashSet<Member>();
            Users = new HashSet<User>();
        }

        public decimal IdChurch { get; set; }
        public string ChurchName { get; set; } = null!;
        public string? Address { get; set; }

        public virtual ICollection<Donation> Donations { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
