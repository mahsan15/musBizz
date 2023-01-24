using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace W2022A5MA.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class LoadDataController : Controller
    {
        // Reference to the manager object
        Manager m = new Manager();

        // GET: LoadData
        public ActionResult Index()
        {
            if (m.LoadData())
            {
                return Content("data has been loaded");
            }
            else
            {
                return Content("data exists already");
            }
        }

        public ActionResult Remove()
        {
            if (m.RemoveData())
            {
                return Content("All data has been removed");
            }
            else
            {
                return Content("could not remove data");
            }
        }

        public ActionResult RemoveDatabase()
        {
            if (m.RemoveDatabase())
            {
                return Content("database has been removed");
            }
            else
            {
                return Content("could not remove database");
            }
        }

        public ActionResult LoadGenre()
        {
            if (m.LoadGenreData())
            {
                return Content("Genres has been loaded");
            }
            else
            {
                return Content("Genres could not be loaded");
            }
        }

        public ActionResult LoadArtists()
        {
            if (m.LoadArtistData())
            {
                return Content("Artists has been loaded");
            }
            else
            {
                return Content("Artists could not be loaded");
            }
        }

        public ActionResult LoadAlbums()
        {
            if (m.LoadAlbumData())
            {
                return Content("Albums has been loaded");
            }
            else
            {
                return Content("Albums could not be loaded");
            }
        }

        public ActionResult LoadTracks()
        {
            if (m.LoadTracksData())
            {
                return Content("Tracks has been loaded");
            }
            else
            {
                return Content("Tracks could not be loaded");
            }
        }

        public ActionResult LoadAll()
        {
             
            var ld = m.LoadGenreData();
            if (ld)
            {
                ld = m.LoadArtistData();
                if (ld)
                {
                    ld = m.LoadAlbumData();
                    if (ld)
                    {
                        ld = m.LoadTracksData();
                    }
                    else
                    {
                        ld = false;
                    }
                }
                else
                {
                    ld = false;
                }
            }
            else
            {
                ld = false;
            }
           
          

            if (ld)
            {
                return Content("All data has been loaded");
            }
            else
            {
                return Content("All data could not be loaded");
            }
        }


    }
}