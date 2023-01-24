using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A5MA.Models
{
    public class TrackAddViewModel
    {
        [Required]
        [Display(Name = "Track name")]
        [StringLength(200)]
        public string Name { get; set; }

        [Display(Name = "Composer name(s)")]
        public string Composers { get; set; }

        [Display(Name = "Track genre")]
        public string Genre { get; set; }

        public int AlbumId { get; set; }
    }
}