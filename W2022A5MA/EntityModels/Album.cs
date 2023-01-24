using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A5MA.EntityModels
{
    public class Album
    {
        public Album()
        {
            Artists = new HashSet<Artist>();
            Tracks = new HashSet<Track>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Coordinator { get; set; }

        public string Genre { get; set; }

        [StringLength(500)]
        public string UrlAlbum { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public virtual ICollection<Artist> Artists { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }
}