using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class FarmSectionHandler
    {
        public List<FarmSection> FindFarmSections()
        {
            List<FarmSection> farmSections = null;
            string queryString = "SELECT `farm_fields`.`field_id`, " +
                                        "`crops`.`crop_id`, " +
                                        "`crops`.`crop_name`, " +
                                        "`farm_fields`.`dimension_x`, " +
                                        "`farm_fields`.`dimension_y`, " +
                                        "`farm_fields`.`status`, " +
                                        "`farm_fields`.`date_sowed`, " +
                                        "`farm_fields`.`expected_harvest_date` " +
                                "FROM `farm_fields` " +
                                "LEFT JOIN `crops` " +
	                                "ON `farm_fields`.`crop_id` = `crops`.`crop_id`";
            MySqlCommand sqlCommand = new MySqlCommand(queryString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if(reader.HasRows)
            {
                farmSections = new List<FarmSection>();
                while(reader.Read())
                {
                    string sect_id = reader.GetString(0);
                    string crop_id = (reader.IsDBNull(1) ? "-" : reader.GetString(1));
                    string crop_name = (reader.IsDBNull(2) ? "-" : reader.GetString(2));
                    decimal dimension_x = reader.GetDecimal(3);
                    decimal dimension_y = reader.GetDecimal(4);
                    string status = reader.GetString(5);
                    DateTime sow_date = (reader.IsDBNull(6) ? new DateTime(1900, 1, 1) : reader.GetDateTime(6));
                    DateTime harvest_date = (reader.IsDBNull(7) ? new DateTime(1900, 1, 1) : reader.GetDateTime(7));

                    farmSections.Add(new FarmSection(sect_id, crop_id, crop_name, dimension_x, dimension_y, status, sow_date, harvest_date));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return farmSections;
        }

        public FarmSection FindFarmSectionWithID(string field_id)
        {
            FarmSection farmSection = null;
            string queryString = "SELECT `farm_fields`.`field_id`, " +
                                        "`crops`.`crop_id`, " +
                                        "`crops`.`crop_name`, " +
                                        "`farm_fields`.`dimension_x`, " +
                                        "`farm_fields`.`dimension_y`, " +
                                        "`farm_fields`.`status`, " +
                                        "`farm_fields`.`date_sowed`, " +
                                        "`farm_fields`.`expected_harvest_date` " +
                                "FROM `farm_fields` " +
                                "LEFT JOIN `crops` " +
                                    "ON `farm_fields`.`crop_id` = `crops`.`crop_id`" +
                                "WHERE `farm_fields`.`field_id` = '" + field_id +"';";
            MySqlCommand sqlCommand = new MySqlCommand(queryString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    string sect_id = reader.GetString(0);
                    string crop_id = (reader.IsDBNull(1) ? "-" : reader.GetString(1));
                    string crop_name = (reader.IsDBNull(2) ? "-" : reader.GetString(2));
                    decimal dimension_x = reader.GetDecimal(3);
                    decimal dimension_y = reader.GetDecimal(4);
                    string status = reader.GetString(5);
                    DateTime sow_date = (reader.IsDBNull(6) ? new DateTime(1900, 1, 1) : reader.GetDateTime(6));
                    DateTime harvest_date = (reader.IsDBNull(7) ? new DateTime(1900, 1, 1) : reader.GetDateTime(7));

                    farmSection = new FarmSection(sect_id, crop_id, crop_name, dimension_x, dimension_y, status, sow_date, harvest_date);
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return farmSection;
        }

        // function for updating farm section when crops are ready to harvest
        // by checking expected harvest date
        public int UpdateFarmSection()
        {
            string updateQuery = "UPDATE `farm_fields` SET `status` = 'HARVEST' WHERE CURDATE() >= `expected_harvest_date` AND `status` = 'CULTIVATING'";
            MySqlCommand sqlCommand = new MySqlCommand(updateQuery, DbConnector.Instance.getConn());
            return sqlCommand.ExecuteNonQuery();
        }

        public int UpdateFarmSection(FarmSection section)
        {
            string updateQuery = "UPDATE `farm_fields` SET `crop_id` = " + (section.CropID == "" ? "NULL" : "'" + section.CropID + "'") + ", `status` = '" + section.Status + "', `date_sowed` = " + (section.SowDate == DateTime.MinValue ? "NULL" : "'" + section.SowDate.ToString("yyyy-MM-dd") + "'") + ", `expected_harvest_date` = " + (section.ExpHarvestDate == DateTime.MinValue ? "NULL" : "'" + section.ExpHarvestDate.ToString("yyyy-MM-dd") + "'") + " WHERE `field_id` = '" + section.SectionID + "';";
            MySqlCommand sqlCommand = new MySqlCommand(updateQuery, DbConnector.Instance.getConn());
            return sqlCommand.ExecuteNonQuery();
        }
    }
}