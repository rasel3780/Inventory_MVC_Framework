using InventoryManagement.DbContexts;
using InventoryManagement.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InventoryManagement.Repositories
{
    public class VendorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VendorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Vendor>> GetVendorList()
        {
            var vendorList = new List<Vendor>();
            using(var cmd = new SqlCommand("dbo.GetVendorList", _dbContext.Connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if(_dbContext.Connection.State != ConnectionState.Open)
                {
                    _dbContext.Connection.Open();
                }

                using(var reader = await cmd.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                        vendorList.Add(new Vendor
                        {
                            VendorID = Convert.ToInt32(reader["VendorID"]),
                            VendorName = reader["VendorName"].ToString(),
                            ContactNumber = reader["ContactNumber"].ToString(),
                            Address = reader["Address"].ToString()
                        });
                    }
                }
            }
            return vendorList;
        }
    }
}