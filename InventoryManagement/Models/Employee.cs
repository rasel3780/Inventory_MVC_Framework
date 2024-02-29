using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.Models
{
    public class Employee
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public double Salary { get; set; }
        public string Designation { get; set; }
        public DateTime JoinDate { get; set; }

        public List<Employee> EmpList { get; set; }
        public Employee()
        {
            EmpList = new List<Employee>();
        }

        public static List<Employee> GetEmpData()
        {
            string[] allDesignations = { "Computer Technician","IT Support Specialist",
                "Computer Sales Associate","System Administrator", "Network Engineer",
                "Computer Repair Technician","Technical Support Specialist","Store Manager",
                "Inventory Manager","Customer Service Representative","Sales Manager",
                "Product Specialist","E-commerce Manager","Web Developer", "Database Administrator",
                "Business Development Manager","Marketing Executive", "Market Research Analyst",
                "Business Analyst", "Business Consultant", "Marketing Manager",
                "Business Development Executive","Marketing Director","Sales Executive","Salesman"};

            DateTime startDate = new DateTime(2015, 01, 01);
            int joinDateRange = (DateTime.Today - startDate).Days;
            Random rand = new Random();


            List<Employee> empData = new List<Employee>();

            for (int i = 1; i < 101; i++)
            {
                Employee emp = new Employee();
                emp.Name = "Emp_" + i.ToString();
                emp.Age = rand.Next(18, 41);
                emp.Email = "emxaple_" + i.ToString() + "@gmail.com";
                emp.Salary = rand.Next(15000, 165000);
                emp.Designation = allDesignations[rand.Next(allDesignations.Length)];
                emp.JoinDate = startDate.AddDays(rand.Next(joinDateRange));
                empData.Add(emp);
            }
            return empData;
        }
    }



}