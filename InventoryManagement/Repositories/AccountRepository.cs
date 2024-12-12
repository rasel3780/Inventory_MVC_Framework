using InventoryManagement.DbContexts;
using InventoryManagement.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InventoryManagement.Repositories
{
    public class AccountRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }

        public Account GetAccountByCredentials(string userName, string password)
        {
            using (var cmd = new SqlCommand("dbo.GetEmployee", _dbContext.Connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (_dbContext.Connection.State != ConnectionState.Open)
                {
                    _dbContext.Connection.Open();
                }
                cmd.Parameters.Add(new SqlParameter("@UserName", userName));
                cmd.Parameters.Add(new SqlParameter("@Password", password));

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Account
                        {
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = (UserRole)Enum.Parse(typeof(UserRole), reader["Role"].ToString())
                        };
                    }
                }
            }
            return null;
        }
        
    }
}