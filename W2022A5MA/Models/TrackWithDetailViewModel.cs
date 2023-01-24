using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A5MA.Models
{
    public class TrackWithDetailViewModel : TrackBaseViewModel
    {
        public TrackWithDetailViewModel()
        {
            AlbumNames = new List<string>();
        }

        [Display(Name = "Albums with this track")]
        public IEnumerable<string> AlbumNames { get; set; }

        [Display(Name = "Clerk who helps with album tracks")]
        public string Clerk { get; set; }
    }
}