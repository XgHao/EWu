using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Controllers
{
    [Authorize]
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
        public ActionResult Edit(Treasure treasure, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    treasure.ImageMimeType = image.ContentType;
                    treasure.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(treasure.ImageData, 0, image.ContentLength);
                }

                repository.SaveTreasure(treasure);
                TempData["message"] = string.Format("{0} has been saved", treasure.TreasureName);
                return RedirectToAction("Index");
            }
            else
            {
                return View(treasure);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Treasure());
        }

        [HttpPost]
        public ActionResult Delete(Guid treasureUID)
        {
            Treasure deletedTreasure = repository.DeleteTreasure(treasureUID);
            if (deletedTreasure != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedTreasure.TreasureName);
            }
            return RedirectToAction("Index");
        }
    }
}