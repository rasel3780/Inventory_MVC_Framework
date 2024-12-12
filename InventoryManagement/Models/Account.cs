﻿using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
namespace InventoryManagement.Models
{
    public class Account
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }

    }
}