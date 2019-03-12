using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private ITreasuresRepository repository;

        public AdminController(ITreasuresRepository repo)
        {
            repository = repo;
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(repository.Treasures);
        }

        public ViewResult Edit(Guid treasureUID)
        {
            Treasure treasure = repository.Treasures.FirstOrDefault(t => t.TreasureUID == treasureUID);
            return View(treasure);
        }

        [HttpPost]
        public ActionResult Edit(Treasure treasure)
        {
            if (ModelState.IsValid)
            {
                repository.SaveTreasure(treasure);
                TempData["message"] = string.Format("{0} has been saved", treasure.TreasureName);
                return RedirectToAction("Index");
            }
            else
            {
                return View(treasure);
            }
        }
    }
}