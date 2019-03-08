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
    public class TreasureController : Controller
    {
        private ITreasuresRepository repository;
        public int PageSize = 2;

        public TreasureController(ITreasuresRepository treasuresRepository)
        {
            repository = treasuresRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            TreasureListViewModel model = new TreasureListViewModel
            {
                Treasures = repository.Treasures
                                    .Where(t => category == null || t.TreasureType == category)
                                    .OrderBy(t => t.TreasureName)
                                    .Skip((page - 1) * PageSize)
                                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItem = category == null
                                          ? repository.Treasures.Count()
                                          : repository.Treasures.Where(e => e.TreasureType == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);

            //IEnumerable<Treasure> treasures = repository.Treasures
            //                                            .OrderBy(t => t.BrowseNum)
            //                                            .Skip((page - 1) * PageSize)
            //                                            .Take(PageSize);
            //return View(treasures);
        }
    }
}