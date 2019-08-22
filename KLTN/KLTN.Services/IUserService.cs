using KLTN.DataModels.Models.Users;
using System.Collections.Generic;

namespace KLTN.Services
{
    public interface IUserService
    {
        int Register(RegisterUserViewModel register);

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
    }
}
