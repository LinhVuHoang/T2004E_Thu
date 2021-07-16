using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T2004E_Thu.Models
{
    public class CartItem
    {
      public CartItem(Book book,int qty)
        {
            Book = book;
            Quantity = qty;
        }
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}