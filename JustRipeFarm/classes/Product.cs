using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Product
    {
        private string productCode;
        private string productName;
        private int quantity;
        private decimal price;
        private bool isOnSale;

        public Product()
        {

        }

        public Product(string product_code = "", string product_name = "", int quantity = 0, decimal price = 0, bool is_on_sale = false)
        {
            productCode = product_code;
            productName = product_name;
            this.quantity = quantity;
            this.price = price;
            isOnSale = is_on_sale;
        }

        public override string ToString()
        {
            return ProductName + " - RM " + Price.ToString("N2");
        }

        public string ProductCode { get => productCode; set => productCode = value; }
        public string ProductName { get => productName; set => productName = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public decimal Price { get => price; set => price = value; }
        public bool IsOnSale { get => isOnSale; set => isOnSale = value; }
    }
}