using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace T2004E_Thu.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NameB { get; set; }
        public string Image { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; }
        public int ActorID { get; set; }//Khóa ngoài
        public virtual Actor Actor { get; set; }
        
    }
}