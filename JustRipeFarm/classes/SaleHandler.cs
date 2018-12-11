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
                    "WHERE sales.sale_datetime BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' and '" + endDate.AddDays(1).ToString("yyyy-MM-dd") + "' " +
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

        public string InsertNewSale(Sale sale)
        {
            MySqlTransaction tr = null;

            try
            {
                tr = DbConnector.Instance.getConn().BeginTransaction();

                MySqlCommand sqlComm = DbConnector.Instance.getConn().CreateCommand();
                sqlComm.Connection = DbConnector.Instance.getConn();
                sqlComm.Transaction = tr;

                // insert buyer details - not inserted if buyer is existed
                string insertBuyerQuery = "INSERT INTO `buyers`(`buyer_id`, `first_name`, `last_name`, `email_address`, `phone_number`, `company_name`) " +
                        "SELECT * FROM(SELECT NULL AS `buyer_id`, '" + sale.Buyer.FirstName + "' AS `first_name`, '" + sale.Buyer.LastName + "' AS `last_name`, '" + sale.Buyer.EmailAddress + "' AS `email_address`, " + (sale.Buyer.PhoneNumber == "" ? "NULL" : "'" + sale.Buyer.PhoneNumber + "'") + " AS `phone_number`, " + (sale.Buyer.CompanyName == "" ? "NULL" : "'" + sale.Buyer.CompanyName + "'") + " AS `company_name`) AS tmp " +
                        "WHERE NOT EXISTS(SELECT 1 FROM `buyers` WHERE `buyers`.`email_address` = BINARY '" + sale.Buyer.EmailAddress + "')";
                sqlComm.CommandText = insertBuyerQuery;
                sqlComm.ExecuteNonQuery();

                // insert sale record
                string insertSaleQuery = "INSERT INTO `sales`(`sale_id`, `sale_datetime`, `buyer_id`, `total_price`) VALUES ('" + sale.SaleID + "', '" + sale.SaleDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', (SELECT `buyers`.`buyer_id` FROM `buyers` WHERE `buyers`.`email_address` = BINARY '" + sale.Buyer.EmailAddress + "'), " + sale.TotalPrice.ToString("N2") + ")";
                sqlComm.CommandText = insertSaleQuery;
                sqlComm.ExecuteNonQuery();

                // iterate through products list to insert and update product stock
                foreach (Product product in sale.SaleProducts)
                {
                    string sqlInsertProductQuery = "INSERT INTO `sale_product`(`sale_id`, `product_code`, `quantity`) VALUES ('" + sale.SaleID + "', '" + product.ProductCode + "', " + product.Quantity + ")";
                    string sqlUpdateProductStockQuery = "UPDATE `products` SET `qty_in_stock` = `qty_in_stock` - " + product.Quantity + " WHERE `product_code` = '" + product.ProductCode + "';";

                    sqlComm.CommandText = sqlInsertProductQuery;
                    sqlComm.ExecuteNonQuery();
                    sqlComm.CommandText = sqlUpdateProductStockQuery;
                    sqlComm.ExecuteNonQuery();
                }

                tr.Commit();

                tr.Dispose();
                sqlComm.Dispose();

                return "SUCCESS";
            }
            catch (MySqlException ex)
            {
                try
                {
                    tr.Rollback();
                }
                catch (MySqlException ex1)
                {
                    return ex1.ToString();
                }

                return ex.ToString();
            }
        }
    }
}