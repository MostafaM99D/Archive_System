using Archive.DAL.Models;
using Archive.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Archive.BLL.Services
{
    public class DepartmentService
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;

        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }

        public DepartmentService(Department department, enMode mode = enMode.AddNew)
        {
            DepartmentID = department.DepartmentID;
            DepartmentName = department.DepartmentName;
            _Mode = mode;
        }

        public Department DepartmentDTO => new Department
        {
            DepartmentID = this.DepartmentID,
            DepartmentName = this.DepartmentName
        };

        public static async Task<IEnumerable<Department>> GetAllDepartments()
        {
            return await DepartmentRepository.GetAllDepartmentsAsync();
        }

        public static async Task<DepartmentService?> GetDepartmentByID(int departmentID)
        {
            var result = await DepartmentRepository.GetDepartmentByIDAsync(departmentID);
            return result != null ? new DepartmentService(result, enMode.Update) : null;
        }

        private async Task<bool> _AddNewDepartment()
        {
            this.DepartmentID = await DepartmentRepository.AddNewDepartmentAsync(DepartmentDTO);
            return this.DepartmentID != -1;
        }

        private async Task<bool> _UpdateDepartment()
        {
            return await DepartmentRepository.UpdateDepartmentAsync(DepartmentDTO);
        }

        public async Task<bool> Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (await _AddNewDepartment())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return await _UpdateDepartment();

                default:
                    return false;
            }
        }

        public static async Task<bool> DeleteDepartment(int departmentID)
        {
            return await DepartmentRepository.DeleteDepartmentAsync(departmentID);
        }

        public static async Task<bool> IsDepartmentExist(int departmentID)
        {
            return await DepartmentRepository.IsDepartmentExistAsync(departmentID);
        }
    }
}
