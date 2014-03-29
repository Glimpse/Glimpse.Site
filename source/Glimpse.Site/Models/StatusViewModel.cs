using System.Collections.Generic;
using System.Web.Mvc;

namespace Glimpse.Site.Models
{
    public class StatusViewModel
    {
        public Release.Release Release { get; set; }

        public IEnumerable<SelectListItem> Milestones { get; set; }
    }
}