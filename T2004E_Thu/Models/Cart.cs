using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T2004E_Thu.Models
{
    public class Cart
    {
        private Customer customer;
        private List<CartItem> cartItems;
        private decimal grandTotal;
        public Cart()
        {
            cartItems = new List<CartItem>();
        }
        public List<CartItem> CartItems { get => cartItems; }
        public decimal GrandTotal { get => grandTotal; set => grandTotal = value; }
        public Customer Customer { get => customer; set => customer = value; }
        public CartItem this[int index]// Một indexer trong C# cho phép một
                                       //đối tượng để được lập chỉ mục, ví dụ như
                                       //một mảng. Khi bạn định nghĩa một indexer
                                       //cho một lớp, thì lớp này vận hành tương
                                       //tự như một virtual array (mảng ảo).
                                       //Sau đó, bạn có thể truy cập instance
                                       //(sự thể hiện) của lớp này bởi sử
                                       //dụng toán tử truy cập mảng trong
                                       //C# là ([ ]). 
        {
            get => CartItems[index];
            set => CartItems[index] = value;
        }
        public bool AddToCart(CartItem item)
        {
            int check = CheckExists(item);
            if (check >= 0) //có sản phẩm
            {
                CartItems[check].Quantity += item.Quantity;//tăng số lượng
            }
            else
            {
                CartItems.Add(item);// thêm item vài list cartiem
            }
            CalculateGrandTotal();//gọi hàm tính tiền
            return true;
        }
        public void RemoveItem(int id)
        {
            // hàm xóa cart
            foreach (var item in CartItems) // biến var chương trình sẽ tự ép kiểu cho biến
            {
                if (item.Book.Id == id) // check id truyền vào có đúng id trong product cần remove
                {
                    CartItems.Remove(item); // xóa item đã gọi
                    CalculateGrandTotal(); // tính tiền lại
                    return;
                }
            }
        }
        public int CheckExists(CartItem item) // hàm check xem có tồn tại item ko
        {
            for (int i = 0; i < CartItems.Count; i++)
            {
                if (CartItems[i].Book.Id == item.Book.Id) // kiếm tra id trong list cartitem có bằng id truyền vào
                {
                    return i;
                }
            }
            return -1; // ko có trả về -1 vì ko có product cần tìm
        }
        public void CalculateGrandTotal()//tính tổng tiền sản phẩm
        {
            decimal grand = 0;
            foreach (CartItem item in CartItems)
            {
                grand += item.Book.Price * item.Quantity;
            }
            grandTotal = grand;
        }

    }
}