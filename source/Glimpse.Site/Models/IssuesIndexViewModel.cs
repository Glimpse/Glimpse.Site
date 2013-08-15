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

        public IEnumerable<PackageCategoryViewModel> PackageCategories { get; set; }
    }

    public class PackageCategoryViewModel
    {
        public PackageCategoryViewModel()
        {
            AcknowledgedIssues = new List<IssueViewModel>();
            CompletedIssues = new List<IssueViewModel>();
        }
        public string Name { get; set; }
        public string CurrentRelease { get; set; }
        public string NextRelease { get; set; }
        public IEnumerable<PackageViewModel> Packages { get; set; }
        public IEnumerable<IssueViewModel> AcknowledgedIssues { get; set; }
        public IEnumerable<IssueViewModel> CompletedIssues { get; set; }
    }

    public class PackageViewModel
    {
        public string Name { get; set; }
        public PackageStatus Status { get; set; }

        public string StatusImage
        {
            get
            {
                return Status == PackageStatus.Red ? "redIcon.png" : "greenIcon.png";
            }
        }

        public string StatusDescription { get; set; }
    }

    public enum PackageStatus
    {
        Green, Red
    }

    public class IssueViewModel
    {
        public string IssueId { get; set; }
        public string IssueLinkUrl { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}