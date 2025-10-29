using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.DAL.Models
{
    public class EmailType
    {
        public EmailType()
        {
        }
        public EmailType(int emailTypeID, string? typeName)
        {
            EmailTypeID = emailTypeID;
            TypeName = typeName;
        }

        public int EmailTypeID { get; set; }
        public string? TypeName { get; set; }
    }
}
