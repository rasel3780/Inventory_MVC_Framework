using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Equipment
    {
        public string Name { get; set; }
        public int EqCount { get; set; }
        public DateTime EntryDate { get; set; }

        public List<Equipment> ListEquipment { get; set; }

        public Equipment()
        {
            ListEquipment = new List<Equipment>();
        }

        public static List<Equipment> GetEquipmentData()
        {
            List<Equipment> equipmentList = new List<Equipment>();
            string conString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;

            SqlConnection _connection = new SqlConnection(conString);
            _connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = "[dbo].[spInventory_GetEquipments]";
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            SqlDataReader reader = cmd.ExecuteReader();

            if(reader.HasRows)
            {
                while(reader.Read()) 
                {
                    Equipment obj = new Equipment();
                    obj.Name = reader["EquipmentName"].ToString();
                    obj.EqCount = Convert.ToInt32(reader["Quantity"].ToString());
                    obj.EntryDate = Convert.ToDateTime(reader["EntryDate"].ToString());
                    equipmentList.Add(obj);
                }
            }
            cmd.Dispose();
            _connection.Close();
            return equipmentList;
        }

        public int SaveEquipment()
        {
           
            string conString = ConfigurationManager.ConnectionStrings["InventoryConString"].ConnectionString;

            SqlConnection _connection = new SqlConnection(conString);
            _connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = _connection;
            cmd.CommandText = "[dbo].[spInventory_InsertEquipment]";
            cmd.Parameters.Clear();

            cmd.Parameters.Add(new SqlParameter("@Name", this.Name));
            cmd.Parameters.Add(new SqlParameter("@EqCount", this.EqCount));
            cmd.Parameters.Add(new SqlParameter("@EntryDate", this.EntryDate));

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;

            int result= cmd.ExecuteNonQuery();

            cmd.Dispose();
            _connection.Close();

            return result;
        }
    }
}