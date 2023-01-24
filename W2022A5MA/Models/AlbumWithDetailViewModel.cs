using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A5MA.Models
{
    public class AlbumWithDetailViewModel : AlbumBaseViewModel
    {
        public AlbumWithDetailViewModel()
        {
            Artists = new List<ArtistBaseViewModel>();
            Tracks = new List<TrackBaseViewModel>();
        }

        [Display(Name = "Number of tracks on this album")]
        public virtual ICollection<TrackBaseViewModel> Tracks { get; set; }

        [Display(Name = "Number of artists on this album")]
        public virtual IEnumerable<ArtistBaseViewModel> Artists { get; set; }

        
    }
}