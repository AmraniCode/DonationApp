using System;
using System.Collections.Generic;

namespace ChurchApp.Models
{
    public partial class Member
    {
        public Member()
        {
            Donations = new HashSet<Donation>();
            Users = new HashSet<User>();
        }

        public decimal IdMember { get; set; }
        public decimal IdChurch { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Email { get; set; } = null!;

        public virtual Church IdChurchNavigation { get; set; } = null!;
        public virtual ICollection<Donation> Donations { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
