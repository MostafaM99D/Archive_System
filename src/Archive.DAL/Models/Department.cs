using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Archive.DAL.Models
{
    public class Department
    {
        public Department(int departmentID, string? name)
        {
            DepartmentID = departmentID;
            DepartmentName = name;
        }
        public Department()
        {
         
        }
        public int DepartmentID { get; set; }
        public string? DepartmentName { get; set; }

    }
}
