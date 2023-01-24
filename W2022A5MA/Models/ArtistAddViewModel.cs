using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A5MA.Models
{
    public class ArtistAddViewModel
    {
        public ArtistAddViewModel()
        {
            BirthOrStartDate = DateTime.Now;
        }
            [Display(Name = "If applicable, artist's birth name")]
        public string BirthName { get; set; }

        [Display(Name = "Artist name or stage name")]
        public string Name { get; set; }

        [Display(Name = "Executive who looks after this artist")]
        public string Executive { get; set; }

        //[Display(Name = "Artist's primary genre")]
        //public int GenreId { get; set; }

        [Display(Name = "Artist's primary genre")]
        public string Genre { get; set; }

        [Display(Name = "Artist photo")]
        [StringLength(500)]
        public string UrlArtist { get; set; }

        [Display(Name = "Birth date, or start date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? BirthOrStartDate { get; set; }



    }
}
