using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace W2022A5MA.Controllers
{
    public class GenresController : Controller
    {
        // GET: Genres
        private Manager m = new Manager();

        public ActionResult Index()
        {
            return View(m.GenreGetAll());
        }

        
    }
}
