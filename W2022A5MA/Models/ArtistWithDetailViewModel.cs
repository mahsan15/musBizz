using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A5MA.Models
{
    public class ArtistWithDetailViewModel : ArtistBaseViewModel
    {
        public ArtistWithDetailViewModel()
        {
            Albums = new List<AlbumBaseViewModel>();
        }
        public virtual IEnumerable<AlbumBaseViewModel> Albums { get; set; }



    }
}