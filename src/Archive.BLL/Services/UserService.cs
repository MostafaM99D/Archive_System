using Archive.DAL.Models;
using Archive.DAL.Repositories;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Archive.BLL.Services
{
    public class UserService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode _Mode = enMode.AddNew;
        public User UserDTO => new User(this.UserID, this.FirstName, this.LastName, this.Username, this.Password, this.Permissions, this.IsActive);
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public long Permissions { get; set; }
        public bool IsActive { get; set; }

        public UserService(User user, enMode mode = enMode.AddNew)
        {
            UserID = user.UserID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Password = user.Password;
            Permissions = user.Permissions;
            IsActive = user.IsActive;
            Username = user.Username;

            _Mode = mode;
        }

        public static async Task<List<User>> GetAllUsers()
        {
            return await UserRepository.GetAllUsersAsync();
        }
        public static async Task<UserService> GetUserByID(int userId)
        {
            User res = await UserRepository.GetUserByIDAsync(userId);

            if (res != null)
                return new UserService(res, enMode.Update);
            else
                return null!;
        }
        private async Task<bool> _AddNewUser()
        {
            this.UserID = await UserRepository.AddNewUserAsync(UserDTO);
            return this.UserID != -1;
        }
        private async Task<bool> _UpdateUser()
        {
            return await UserRepository.UpdateUserAsync(UserDTO);
        }
        public async Task<bool> Save()
        {

            switch (_Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewUser())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return await _UpdateUser();
                default:
                    return false;
            }
        }
        public static async Task<bool> DeleteUser(int UserId)
        {
            return await UserRepository.DeleteUserAsync(UserId);
        }
        public static async Task<bool>IsUserExist(int UserId)
        {
            return await UserRepository.IsUserExist(UserId);
        }
    }
}