using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class ProductHandler
    {
        public List<Product> FindAllProducts()
        {
            List<Product> products = null;
            string sqlString = "SELECT `products`.`product_code`, `products`.`product_name`, `products`.`qty_in_stock`, `products`.`price`, `products`.`on_sale_status` FROM `products`;";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                products = new List<Product>();
                while (reader.Read())
                {
                    string productCode = reader.GetString(0);
                    string productName = reader.GetString(1);
                    int quantity = reader.GetInt32(2);
                    decimal price = reader.GetDecimal(3);
                    bool isOnSale = reader.GetBoolean(4);

                    products.Add(new Product(productCode, productName, quantity, price, isOnSale));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return products;
        }
    }
}