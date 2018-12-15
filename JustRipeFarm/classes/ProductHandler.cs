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
        public List<Product> FindAllProducts(string status = "ALL")
        {
            List<Product> products = null;
            string sqlString = "SELECT `products`.`product_code`, `products`.`product_name`, `products`.`qty_in_stock`, `products`.`price`, `products`.`on_sale_status` FROM `products`";

            if (status == "SALE") sqlString += " WHERE `products`.`on_sale_status` = 1";
            else if(status == "NOT SALE") sqlString += " WHERE `products`.`on_sale_status` = 0";

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

        public int UpdateProductData(Product product)
        {
            string sql = "UPDATE `products` SET `product_name` = '" + product.ProductName + "', `qty_in_stock` = " + product.Quantity.ToString() + ", `price` = " + product.Price.ToString("N2") + ", `on_sale_status` = " + (product.IsOnSale ? "1" : "0") + " WHERE `product_code` = '" + product.ProductCode + "'";
            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
            return sqlComm.ExecuteNonQuery();
        }

        public int CreateNewProduct(string pC, string pN, int QTY, decimal p, bool oSS)
        {
            string sql = "INSERT INTO products (product_code,product_name,qty_in_stock,price,on_sale_status) VALUES('" + pC + "','" + pN + "','" + QTY + "','" + p + "'," + (oSS ? "1" : "0") + ")";
            MySqlCommand sqlCommand = new MySqlCommand(sql, DbConnector.Instance.getConn());
            return sqlCommand.ExecuteNonQuery();
        }
    }
}