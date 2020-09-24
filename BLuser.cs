using System.Collections.Generic;

namespace TTT.BL
{
    public class User
    {
        public User()
        {
        }

        public User(string fname, string lname, string address, string city, string postal, string email, string phone, List<Item> cart)
        {
            Fname = fname;
            Lname = lname;
            Address = address;
            City = city;
            Postal = postal;
            Email = email;
            Phone = phone;
            Cart = cart;
        }

        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postal { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<Item> Cart { get; set; }

        public List<Item> GetCart()
        {
            return Cart;
        }
    }
}