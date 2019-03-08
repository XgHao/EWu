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

        public FavoriteController(ITreasuresRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new FavoriteIndexViewModel
            {
                Favorite = GetFavorite(),
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToFavorite(Guid treasureUID, string returnUrl)
        {
            Treasure treasure = repository.Treasures.FirstOrDefault(t => t.TreasureUID == treasureUID);
            if (treasure != null)
            {
                GetFavorite().AddItem(treasure, 1);
            }
            return RedirectToAction(
                    actionName: "Index",
                    routeValues: new
                    {
                        returnUrl
                    }
                );
        }

        public RedirectToRouteResult RemoveFromFavorite(Guid treasureUID,string returnUrl)
        {
            Treasure treasure = repository.Treasures.FirstOrDefault(t => t.TreasureUID == treasureUID);
            if (treasure != null)
            {
                GetFavorite().RemoveLine(treasure);
            }
            return RedirectToAction(
                    actionName: "Index",
                    routeValues: new
                    {
                        returnUrl
                    }
                );
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
    }
}