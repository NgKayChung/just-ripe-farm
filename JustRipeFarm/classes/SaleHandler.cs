using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class SaleHandler
    {
        public List<Sale> GetSalesForDates(DateTime startDate, DateTime endDate)
        {
            List<Sale> sales = null;
            string sqlString = "SELECT sales.sale_id, sales.sale_datetime, buyers.first_name, buyers.last_name, products.product_name, sale_product.quantity, products.price,  sales.total_price " +
                    "FROM `sales` " +
                    "INNER JOIN buyers ON sales.buyer_id = buyers.buyer_id " +
                    "INNER JOIN sale_product ON sales.sale_id = sale_product.sale_id " +
                    "INNER JOIN products ON sale_product.product_code = products.product_code " +
                    "WHERE sales.sale_datetime BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' and '" + endDate.ToString("yyyy-MM-dd") + "' " +
                    "ORDER BY sale_datetime";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                sales = new List<Sale>();

                while (reader.Read())
                {
                    string saleID = reader.GetString(0);
                    DateTime saleDateTime = reader.GetDateTime(1);
                    string buyerFirstName = reader.GetString(2);
                    string buyerLastName = reader.GetString(3);
                    string productName = reader.GetString(4);
                    int quantity = reader.GetInt32(5);
                    decimal productPrice = reader.GetDecimal(6);
                    decimal totalPrice = reader.GetDecimal(7);

                    if(sales.Count > 0)
                    {
                        if(sales.Last().SaleID.Equals(saleID))
                        {
                            sales.Last().SaleProducts.Add(new Product(product_name: productName, quantity: quantity, price: productPrice));
                        }
                        else
                        {
                            sales.Add(new Sale(saleID, saleDateTime, new Buyer(first_name: buyerFirstName, last_name: buyerLastName), totalPrice, new List<Product>() { new Product(product_name: productName, quantity: quantity, price: productPrice) } ));
                        }
                    }
                    else
                    {
                        sales.Add(new Sale(saleID, saleDateTime, new Buyer(first_name: buyerFirstName, last_name: buyerLastName), totalPrice, new List<Product>() { new Product(product_name: productName, quantity: quantity, price: productPrice) } ));
                    }
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return sales;
        }
    }
}