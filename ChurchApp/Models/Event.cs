using System;
using System.Collections.Generic;

namespace ChurchApp.Models
{
    public partial class Event
    {
        public Event()
        {
            Donations = new HashSet<Donation>();
        }

        public decimal IdEvent { get; set; }
        public string LabelEvent { get; set; } = null!;
        public string? Observation { get; set; }

        public virtual ICollection<Donation> Donations { get; set; }
    }
}
