using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using T2004E_Thu.Context;
using T2004E_Thu.Models;
using System.Dynamic;
using System.IO;

namespace T2004E_Thu.Controllers
{
    public class BookController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Book
        public ActionResult Index(string search,string sortOrder,string actorId)
        {
            ViewBag.ActorID = 0;
            string sort = !String.IsNullOrEmpty(sortOrder) ? sortOrder : "asc";
            var books = from p in db.Books select p;
            if (!String.IsNullOrEmpty(search))
            {
                books = books.Where(p => p.NameB.Contains(search)).Include(p => p.Actor);


            }
            switch (sort)
            {
                case "asc": books = books.OrderBy(p => p.NameB).Include(p => p.Actor); break;
                case "desc": books = books.OrderByDescending(p => p.NameB).Include(p => p.Actor); break;
            }
            if (!String.IsNullOrEmpty(actorId))
            {
                var acId = Convert.ToInt32(actorId);
                books = books.Where(p => p.ActorID == acId);
                ViewBag.ActorId = acId;
            }
            var actors = db.Actors.ToList();
            dynamic data = new ExpandoObject();
            data.Actors = actors;
            data.Books = books;
            return View(data);
        }

        // GET: Book/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }
        public ActionResult AddToCart(int? id, int? qty) // hàm thêm product vào cart
        {
            try
            {
               Book book = db.Books.Find(id); // tạo biến theo product
                if (book == null)
                {
                    return HttpNotFound();
                }
                //them vao gio hang
                CartItem item = new CartItem(book, (int)qty); // tạo biến item theo cartitem
                //lay gio hang tu SessSion
                Cart cart = (Cart)Session["Cart"]; // truyền giỏ hàng
                if (cart == null)
                {
                    Customer customer = new Customer("Nguyễn Văn An", "0987654321", "Số 8 Tôn Thất Thuyết");
                    cart = new Cart();//tạo giỏ hàng mới
                    cart.Customer = customer;
                }
                cart.AddToCart(item);
                Session["cart"] = cart;//thêm session
            }
            catch (Exception e)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Cart");
        }
        public ActionResult Cart()
        {
            return View();
        }
        public ActionResult RemoveItem(int? id)
        {
            try
            {
                Cart cart = (Cart)Session["Cart"]; // ép kiểu Cart. Session là một từ phổ biến thường được dùng
                                                   // trong ngôn ngữ lập trình viên Website, Còn có kết nối với cơ sở dữ liệu
                                                   // như Database. Điều đặc biệt ở đây các chức năng đăng xuất, đăng nhập
                                                   // người dùng sẽ rất khó mà thực hiện nổi, nếu không sử dụng session
                                                   // tạo 1 tập tin lưu dữ liệu của Cart
                                                   //lúc đầu chưa có dữ liệu với Session Cart sau gán biến cart của Cart.cs để làm dữ liệu session mới
                if (cart == null)
                {
                    return HttpNotFound();

                }
                cart.RemoveItem((int)id);
                Session["cart"] = cart;//thêm session mới tên cart với biển cart có dữ liệu mới tạo ra 1 session mới 
            }
            catch (Exception e)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Cart");
        }
        // GET: Book/Create
        public ActionResult Create()
        {
            ViewBag.ActorID = new SelectList(db.Actors, "Id", "NameA");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NameB,Price,Description,ActorID")] Book book,HttpPostedFileBase Image)
        {
            String bookImage = "default.png";
            //upload file lên thư mục uploads
            //lưu tên file vào categoryImage
            if (Image != null)
            {
                string fileName = Path.GetFileName(Image.FileName);
                string path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                Image.SaveAs(path);//Lưu file xong
                bookImage = "Uploads/" + fileName;
            }
            book.Image = bookImage;
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ActorID = new SelectList(db.Actors, "Id", "NameA", book.ActorID);
            return View(book);
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActorID = new SelectList(db.Actors, "Id", "NameA", book.ActorID);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NameB,Image,Price,Description,ActorID")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ActorID = new SelectList(db.Actors, "Id", "NameA", book.ActorID);
            return View(book);
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult CheckOut()
        {
            return View();//trả về View của các controller
        }
        [HttpPost]//phương thức đẩy dữ liệu lên của wcp
        public ActionResult CheckOut(Order order) // tạo biến tên order của Order.cs
        {
            if (ModelState.IsValid) // hàm kiểm tra lỗi của wcp nếu ko lỗi thì chạy
            {
                var cart = (Cart)Session["cart"];//truyền session lưu dữ liệu của cart sang order
                order.GrandTotal = cart.GrandTotal;
                order.CreatedAt = DateTime.Now;
                order.Status = 1;
                db.Orders.Add(order);
                db.SaveChanges();
                foreach (var item in cart.CartItems)
                {
                    OrderItem orderItem = new OrderItem() { OrderID = order.Id, ProductID = item.Book.Id, Qty = item.Quantity, Price = item.Book.Price };
                    //tạo vòng lặp để tìm sản phẩm được order sau đó tạo biến của OderItem.cs để lưu vào
                    db.OrderItems.Add(orderItem);
                }
                db.SaveChanges(); //lưu sự thay đổi dữ liệu
                Session["cart"] = null; // xóa giỏ hàng sau đi đặt được hàng
            }
            return RedirectToAction("CheckOutSuccess");//trả về public string Checkoutsuccess
        }
        public string CheckOutSuccess()
        {
            return "Tạo đơn thành công . Xin cảm ơn và hẹn gặp lại quý khách...";

        }
    
}
}
