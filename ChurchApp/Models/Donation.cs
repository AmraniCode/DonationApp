using System;
using System.Collections.Generic;

namespace ChurchApp.Models
{
    public partial class Donation
    {
        public decimal IdDonation { get; set; }
        public decimal IdMember { get; set; }
        public decimal? IdEvent { get; set; }
        public decimal IdChurch { get; set; }
        public string? Observation { get; set; }
        public DateTime DateDonation { get; set; }
        public decimal Price { get; set; }

        public virtual Church IdChurchNavigation { get; set; } = null!;
        public virtual Event? IdEventNavigation { get; set; }
        public virtual Member IdMemberNavigation { get; set; } = null!;
    }
}
