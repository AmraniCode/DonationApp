using System;
using System.Collections.Generic;

namespace ChurchApp.Models
{
    public partial class Profil
    {
        public Profil()
        {
            IdUsers = new HashSet<User>();
        }

        public decimal IdProfil { get; set; }
        public string Label { get; set; } = null!;

        public virtual ICollection<User> IdUsers { get; set; }
    }
}
