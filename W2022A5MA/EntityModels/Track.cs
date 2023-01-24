using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A5MA.EntityModels
{
    public class Track
    {
        public Track()
        {
            Albums = new HashSet<Album>();

        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Composers { get; set; }

        public string Genre { get; set; }

        public string Clerk { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}