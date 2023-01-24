using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A5MA.Models
{
    public class GenreBaseViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}