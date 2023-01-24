using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A5MA.Models
{
    public class AlbumAddViewModel
    {
        public AlbumAddViewModel()
        {
            TrackIds = new List<int>();
            ArtistIds = new List<int>();
            ReleaseDate = DateTime.Now;
        }

        [Display(Name = "Album name")]
        public string Name { get; set; }

        [Display(Name = "Release date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "Album image (cover art)")]
        [StringLength(500)]
        public string UrlAlbum { get; set; }

        [Display(Name = "Album's primary genre")]
        public string Genre { get; set; }

        [Display(Name = "Coordinator who looks after this album")]
        public string Coordinator { get; set; }

        public IEnumerable<int> ArtistIds { get; set; }
        public IEnumerable<int> TrackIds { get; set; }


    }
}