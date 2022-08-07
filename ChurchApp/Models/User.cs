using System;
using System.Collections.Generic;

namespace ChurchApp.Models
{
    public partial class User
    {
        public User()
        {
            IdProfils = new HashSet<Profil>();
        }

        public decimal IdUser { get; set; }
        public decimal IdChurch { get; set; }
        public decimal IdMember { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsDefault { get; set; }

        public virtual Church IdChurchNavigation { get; set; } = null!;
        public virtual Member IdMemberNavigation { get; set; } = null!;

        public virtual ICollection<Profil> IdProfils { get; set; }
    }
}
