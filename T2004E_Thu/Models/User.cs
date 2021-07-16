using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace T2004E_Thu.Models
{
    public class User
    {
        [Key]//khóa chính
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//Sự đồng nhất với Database tạo khóa chính
        public int idUser { get; set; }
        [Required] //nhắc nếu để trống
        public string FullName { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]//xác định email nếu sai sẽ hiện email không xác định

        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password")]//xét xem có đúng trùng với password vừa nhập ko
        public string ConfirmPassword { get; set; }
    }
}