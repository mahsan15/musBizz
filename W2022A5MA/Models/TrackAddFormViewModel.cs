using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace W2022A5MA.Models
{
    public class TrackAddFormViewModel : TrackAddViewModel
    {
        [Display(Name = "Trak Genre")]
        public SelectList GenreList { get; set; }

        public string AlbumName { get; set; }

        
    }
}