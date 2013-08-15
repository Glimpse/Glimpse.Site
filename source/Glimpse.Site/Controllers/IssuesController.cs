using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Site.Models;

namespace Glimpse.Site.Controllers
{
    public class IssuesController : Controller
    {
        //
        // GET: /Issues/

        public ActionResult Index()
        {
            var issuesView = new IssuesIndexViewModel();
            issuesView.PackageCategories = CreatePackages();
            return View(issuesView);
        }

        private IEnumerable<PackageCategoryViewModel> CreatePackages()
        {
            var packages = new List<PackageCategoryViewModel>();
            packages.Add(CreateNewPackage("Glimpse.Core"));
            packages.Add(CreateNewPackage("Glimpse.MVC"));
            return packages;
        }

        private PackageCategoryViewModel CreateNewPackage(string packageName)
        {
            var package = new PackageCategoryViewModel();
            package.Name = packageName;
            package.Packages = new List<PackageViewModel>()
                               {
                                   new PackageViewModel()
                                   {
                                       Name = "Glimpse MVC3",
                                       Status = PackageStatus.Green,
                                       StatusDescription =
                                           "Something wrong"
                                   },
                                   new PackageViewModel()
                                   {
                                       Name = "Glimpse MVC4",
                                       Status = PackageStatus.Red,
                                       StatusDescription =
                                           "Something wrong"
                                   },
                               };
            package.CurrentRelease = "1.3.9";
            package.NextRelease = "1.4.0";
            package.AcknowledgedIssues = CreateIssues();
            package.CompletedIssues = CreateIssues();
            return package;
        }

        private static List<IssueViewModel> CreateIssues()
        {
            return new List<IssueViewModel>()
                   {
                       new IssueViewModel(){Category = "Bug",Description = "Fix where JSON.net could screw up self-reference loops", IssueId = "234234",IssueLinkUrl = "http://sdfsdfsd"},
                       new IssueViewModel(){Category = "Feature",Description = "Fix where JSON.net could screw up self-reference loops", IssueId = "234234",IssueLinkUrl = "http://sdfsdfsd"},
                   };
        }
    }
}
