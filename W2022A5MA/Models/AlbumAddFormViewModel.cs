using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace W2022A5MA.Models
{
    public class AlbumAddFormViewModel : AlbumAddViewModel
    {
        public string ArtistName { get; set; }

        [Display(Name = "Album's Primary Genre")]
        public SelectList GenreList { get; set; }

        [Display(Name = "All artists")]
        public MultiSelectList AllArtists { get; set; }

        [Display(Name = "All tracks")]
        public MultiSelectList AllTracks { get; set; }


    }
}