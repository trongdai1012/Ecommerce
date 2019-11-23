using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels.Models.Users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KLTN.Services
{
    public interface IUserService
    {
        UserViewModel GetUserById(int id);

        Task<int> Register(RegisterUserViewModel register);

        int CreateEmployeeAccount(CreateEmployeeViewModel register);

        int CreateAdminAccount(CreateAdminViewModel register);

        Tuple<AuthenticationViewModel, int> Authentication(AuthenticationViewModel authenticationViewModel);

        /// <summary>
        /// Method ChangeStatus User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool ChangeStatus(int id);

        bool ConfirmUser(string confirmString);

        Tuple<AdminViewModel, int> GetAdmin(int id);
        
        Tuple<UpdateAdminViewModel, int> GetAdminUpdate(int id);

        Tuple<EmployeeViewModel, int> GetEmployee(int id);

        Tuple<CustomerViewModel, int> GetCustomer(int id);

        int UpdateAdmin(UpdateAdminViewModel adminModel);

        int UpdateEmployee(UpdateEmployeeViewModel employeeModel);

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

        int UpdateInfoUser(UpdateUserViewModel updateUser);

        int ForgotPassword(string email);

        bool ConfirmForgotPassword(RetypePassword retypePassword);
    }
}
