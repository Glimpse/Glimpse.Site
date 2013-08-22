using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace Glimpse.Site.Models
{
    public class IssuesIndexViewModel
    {
        public IssuesIndexViewModel()
        {
            PackageCategories = new List<PackageCategoryViewModel>();    
        }

        public List<PackageCategoryViewModel> PackageCategories { get; set; }
    }

    public enum PackageStatus
    {
        Green, Red
    }
}