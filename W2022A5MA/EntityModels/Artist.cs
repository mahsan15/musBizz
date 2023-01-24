using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A5MA.EntityModels
{
    public class Artist
    {
        public Artist()
        {
            Albums = new HashSet<Album>();
            BirthOrStartDate = DateTime.Now;
        }

        public int Id { get; set; }

        public string BirthName { get; set; }

        [Required]
        public string Name { get; set; }

        public string Executive { get; set; }

        public string Genre { get; set; }

        [StringLength(500)]
        public string UrlArtist { get; set; }

        public DateTime? BirthOrStartDate { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

    }
}