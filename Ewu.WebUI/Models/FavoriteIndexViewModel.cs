using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Models
{
    public class FavoriteIndexViewModel
    {
        public Favorite Favorite { get; set; }
        public string ReturnUrl { get; set; }
    }
}