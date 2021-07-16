using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using T2004E_Thu.Models;
namespace T2004E_Thu.Context
{
    public class DataContext : DbContext
    {
        public DataContext(): base("T2004E_3")
        {

        }
        //Tạo Database trong sql
        public DbSet<Book> Books { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }
    }
}