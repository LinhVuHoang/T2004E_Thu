using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T2004E_Thu.Context;
using T2004E_Thu.Models;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
namespace T2004E_Thu.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private DataContext dataContext = new DataContext();
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult FormLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var check = dataContext.Users.FirstOrDefault(s => s.Email == user.Email);
                if(check == null)
                {
                    user.Password = GetMD5(user.Password);//Mã hóa password theo GetMD5(string std)
                    dataContext.Users.Add(user); //add vào dtb
                    dataContext.Configuration.ValidateOnSaveEnabled = false;
                    dataContext.SaveChanges();//lưu sự thay đổi
                    return RedirectToAction("Index");//trả về View index
                }
                else
                {
                    ViewBag.error = "Email already exists";
                }
            }
            return View();
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();//tạo biến md5 của service để mã hóa password
            byte[] frData = Encoding.UTF8.GetBytes(str);//tạo mảng byte để lấy chuỗi string
            byte[] tgData = md5.ComputeHash(frData);//tạo mảng byte để mã hõa mảng byte chữa password tăng tính bảo mật
            string hashString = ""; //tạo biến để + ra chuỗi đưa vào database
            for (int i = 0; i < tgData.Length; i++)
            {
                hashString += tgData[i].ToString("x2");//chuyển thành chuỗi string nhị phân
            }
            return hashString;
        }
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email,string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = dataContext.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count > 0)
                {
                    var u = data.FirstOrDefault();
                    FormsAuthentication.SetAuthCookie(u.FullName, true);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}