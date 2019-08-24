using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Users;
using System;
using System.Collections.Generic;

namespace KLTN.Services
{
    public interface IUserService
    {
        int Register(RegisterUserViewModel register);

        int CreateEmployeeAccount(CreateEmployeeViewModel register);

        int CreateAdminAccount(CreateAdminViewModel register);

        AuthenticationViewModel Authentication(AuthenticationViewModel authenticationViewModel);

        IEnumerable<AdminViewModel> GetAllAdmin();

        IEnumerable<EmployeeViewModel> GetAllEmployee();

        IEnumerable<CustomerViewModel> GetAllCustomer();

        bool UpdateInfoUser(EditUserViewModel customerModel);

        /// <summary>
        /// Method ChangeStatus User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool ChangeStatus(int id);

        UpdateEmployeeViewModel GetEmployee(int id);

        bool UpdateEmployee(UpdateEmployeeViewModel employeeModel);

        bool ConfirmUser(string confirmString);

        /// <summary>
        /// Method LoadData return tuple UserViewModel int int
        /// </summary>
        /// <param name="dtParameters"></param>
        /// <returns></returns>
        Tuple<IEnumerable<AdminViewModel>, int, int> LoadAdmin(DTParameters dtParameters);

        /// <summary>
        /// Method LoadData return tuple UserViewModel int int
        /// </summary>
        /// <param name="dtParameters"></param>
        /// <returns></returns>
        Tuple<IEnumerable<CustomerViewModel>, int, int> LoadCustomer(DTParameters dtParameters);

        /// <summary>
        /// Method LoadData return tuple UserViewModel int int
        /// </summary>
        /// <param name="dtParameters"></param>
        /// <returns></returns>
        Tuple<IEnumerable<EmployeeViewModel>, int, int> LoadEmployee(DTParameters dtParameters);
    }
}
