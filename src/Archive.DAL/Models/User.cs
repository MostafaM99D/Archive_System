using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archive.DAL.Models
{
    public class User
    {
        public User()
        {

        }
        public User(int userID, string? firstName, string? lastName, string? username, string? password, long permissions, bool isActive)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            Permissions = permissions;
            IsActive = isActive;
        }


        public int UserID { get; set; }
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? Username {  get; set; }
        public string? Password { get; set; }
        public long Permissions {  get; set; }
        public bool IsActive {  get; set; }
    }
}
