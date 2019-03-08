using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;

namespace Ewu.WebUI.Controllers
{
    public class NavController : Controller
    {
        private ITreasuresRepository repository;

        public NavController(ITreasuresRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categorise = repository.Treasures
                                                       .Select(x => x.TreasureType)
                                                       .Distinct()
                                                       .OrderBy(x => x);
            return PartialView(categorise);
        }
    }
}