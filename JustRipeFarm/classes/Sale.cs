using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Sale
    {
        private string saleID;
        private DateTime saleDateTime;
        private decimal totalPrice;
        private Buyer buyer;
        private List<Product> saleProducts = new List<Product>();

        public Sale()
        {

        }

        public Sale(string sale_id, DateTime sale_dateTime, Buyer buyer, decimal total_price, List<Product> sale_products)
        {
            saleID = sale_id;
            saleDateTime = sale_dateTime;
            totalPrice = total_price;
            this.buyer = buyer;
            saleProducts = sale_products;
        }

        public string SaleID { get => saleID; set => saleID = value; }
        public DateTime SaleDateTime { get => saleDateTime; set => saleDateTime = value; }
        public decimal TotalPrice { get => totalPrice; set => totalPrice = value; }
        public Buyer Buyer { get => buyer; set => buyer = value; }
        internal List<Product> SaleProducts { get => saleProducts; set => saleProducts = value; }
    }
}