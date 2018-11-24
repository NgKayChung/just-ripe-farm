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
                                        "`crops`.`crop_name`, " +
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
                    string crop_name = (reader.IsDBNull(1) ? "-" : reader.GetString(1));
                    string status = reader.GetString(2);
                    DateTime sow_date = (reader.IsDBNull(3) ? new DateTime(1900, 1, 1) : reader.GetDateTime(3));
                    DateTime harvest_date = (reader.IsDBNull(4) ? new DateTime(1900, 1, 1) : reader.GetDateTime(4));

                    farmSections.Add(new FarmSection(sect_id, crop_name, status, sow_date, harvest_date));
                }
            }

            return farmSections;
        }
    }
}
