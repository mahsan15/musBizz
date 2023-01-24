using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W2022A5MA.Models;

namespace W2022A5MA.Controllers
{
    public class AlbumsController : Controller
    {
        private Manager m = new Manager();
        // GET: Albums
        public ActionResult Index()
        {
            return View(m.AlbumGetAll());
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            var o = m.AlbumGetByIdWithDetail(id.GetValueOrDefault());

            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Pass the object to the view
                return View(o);
            }
        }

        [Authorize(Roles = "Clerk")]
        [Route("album/{id}/addtrack")]
        public ActionResult AddTrack(int? id)
        {
            // Attempt to get the associated object
            var a = m.AlbumGetByIdWithDetail(id.GetValueOrDefault());

            if (a == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Add vehicle for a known manufacturer
                // We send the manufacturer identifier to the form
                // There, it is hidden... <input type=hidden
                // We also pass on the name, so that the browser user
                // knows which manufacturer they're working with

                // Create and configure a form object
                var o = new TrackAddFormViewModel();

                o.AlbumName = a.Name;
                o.AlbumId = a.Id;
                o.GenreList = new SelectList(m.GenreGetAll(), "Name", "Name");


                return View(o);
            }
        }

        [Authorize(Roles = "Clerk")]
        [Route("album/{id}/addtrack")]
        [HttpPost]
        public ActionResult AddTrack(TrackAddViewModel newItem)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.TrackAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                // TODO 25 - Must redirect to the Vehicles controller
                return RedirectToAction("details", "tracks", new { id = addedItem.Id });
            }
        }


    }
}
