using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;
using Ewu.WebUI.Models;

namespace Ewu.WebUI.Controllers
{
    public class FavoriteController : Controller
    {
        private ITreasuresRepository repository;
        private IOrderProcessor orderProcessor;

        public FavoriteController(ITreasuresRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }

        public ViewResult Index(Favorite favorite, string returnUrl)
        {
            return View(new FavoriteIndexViewModel
            {
                Favorite = favorite,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToFavorite(Favorite favorite, Guid treasureUID, string returnUrl)
        {
            Treasure treasure = repository.Treasures.FirstOrDefault(t => t.TreasureUID == treasureUID);
            if (treasure != null)
            {
                favorite.AddItem(treasure, 1);
            }
            return RedirectToAction(
                    actionName: "Index",
                    routeValues: new
                    {
                        returnUrl
                    }
                );
        }

        public RedirectToRouteResult RemoveFromFavorite(Favorite favorite, Guid treasureUID,string returnUrl)
        {
            Treasure treasure = repository.Treasures.FirstOrDefault(t => t.TreasureUID == treasureUID);
            if (treasure != null)
            {
                favorite.RemoveLine(treasure);
            }
            return RedirectToAction(
                    actionName: "Index",
                    routeValues: new
                    {
                        returnUrl
                    }
                );
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Favorite favorite, ShippingDetails shippingDetails)
        {
            if (favorite.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, Your favorite is empty");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(favorite, shippingDetails);
                favorite.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        private Favorite GetFavorite()
        {
            Favorite favorite = (Favorite)Session["Favorite"];
            if (favorite == null)
            {
                favorite = new Favorite();
                Session["Favorite"] = favorite;
            }
            return favorite;
        }

        public PartialViewResult Summary(Favorite favorite)
        {
            return PartialView(favorite);
        }
    }
}