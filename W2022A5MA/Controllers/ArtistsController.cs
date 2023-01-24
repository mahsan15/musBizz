using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W2022A5MA.Models;

namespace W2022A5MA.Controllers
{
    public class ArtistsController : Controller
    {
        private Manager m = new Manager();
        // GET: Artists
        public ActionResult Index()
        {
            return View(m.ArtistGetAll());
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            var o = m.ArtistGetByIdWithDetail(id.GetValueOrDefault());

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

        [Authorize(Roles = "Executive")]
        // GET: Artists/Create
        public ActionResult Create()
        {
            var form = new ArtistAddFormViewModel();

            // Configure the SelectList for the item-selection element on the HTML Form

            form.GenreList = new SelectList(m.GenreGetAll(), "Name", "Name");
            // Configure the SelectList for the item-selection element on the HTML Form

            return View(form);
        }

        [Authorize(Roles = "Executive")]
        // POST: Tracks/Create
        [HttpPost]
        public ActionResult Create(ArtistAddViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.ArtistAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", new { id = addedItem.Id });
            }
        }

        [Authorize(Roles = "Coordinator")]
        [Route("artist/{id}/addalbum")]
        public ActionResult AddAlbum(int? id)
        {
            // Attempt to get the associated object
            var a = m.ArtistGetByIdWithDetail(id.GetValueOrDefault());

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
                var o = new AlbumAddFormViewModel();
                o.ArtistName = a.Name;
                o.GenreList = new SelectList(m.GenreGetAll(), "Name", "Name");

                var selectedValues = new List<int> { a.Id };

                o.AllArtists = new MultiSelectList
                   (items: m.ArtistGetAll(),
                   dataValueField: "Id",
                   dataTextField: "Name",
                   selectedValues: selectedValues
                   );

                o.AllTracks = new MultiSelectList
                    (items: m.TrackGetAllByArtistId(id.GetValueOrDefault()),
                    dataValueField: "Id",
                    dataTextField: "Name"
                    );

                return View(o);
            }
        }

        [Authorize(Roles = "Coordinator")]
        [Route("artist/{id}/addalbum")]
        [HttpPost]
        public ActionResult AddAlbum(AlbumAddViewModel newItem)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }

            // Process the input
            var addedItem = m.AlbumAdd(newItem);

            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                // TODO 25 - Must redirect to the Vehicles controller
                return RedirectToAction("details", "albums", new { id = addedItem.Id });
            }
        }

    }
}
