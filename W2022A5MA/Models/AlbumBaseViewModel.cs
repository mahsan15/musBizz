using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A5MA.Models
{
    public class AlbumBaseViewModel : AlbumAddViewModel
    {
        [Key]
        public int Id { get; set; }
    }
}