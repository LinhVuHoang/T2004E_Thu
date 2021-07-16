using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace T2004E_Thu.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên tác giả")]
        public string NameA { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập năm sinh của tác giả")]
        public int Year { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập mô tả của tác giả")]
        public string description { get; set; }
        public virtual ICollection<Book> Books { get; set; }//tạo Kết nối với bảng db khác

    }
}