using AutoMapper;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.Common.Infrastructure;
using KLTN.DataAccess.Models;
using KLTN.DataModels;
using KLTN.DataModels.Models.Users;
using KLTN.Services.Repositories;
using MailKit.Net.Smtp;
using MimeKit;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KLTN.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public int Register(RegisterUserViewModel register)
        {
            try
            {
                if (CheckEmailExisted(register.Email)) return 0;
                var user = new User
                {
                    Email = register.Email,
                    Password = register.Password,
                    Role = (byte)EnumRole.Customer,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Gender = register.Gender,
                    BirthDay = register.BirthDay,
                    Phone = register.Phone,
                    Address = register.Address,
                    CreateAt = DateTime.UtcNow
                };

                var userRegister = _unitOfWork.UserRepository.Create(user);

                var customer = new Customer
                {
                    UserId = userRegister.Id,
                    Rank = 0,
                    CreateAt = DateTime.UtcNow,
                    Status = false
                };

                var customerRegister = _unitOfWork.CustomerRepository.Create(customer);

                _unitOfWork.Save();

                var userConfirm = new UserConfirm
                {
                    UserId = userRegister.Id,
                    ConfirmString = InitConfirmString()
                };

                var userConfirmRegister = _unitOfWork.UserConfirmRepository.Create(userConfirm);

                var contentMail =
                    "Cảm ơn bạn đã đăng ký tài khoản trên website của chúng tôi!"
                    + @"https://localhost:44338"
                    + Environment.NewLine
                    + Environment.NewLine
                    + "Vui lòng click vào link bên dưới để kích hoạt tài khoản của bạn"
                    + Environment.NewLine
                    + @"https://localhost:44338" + @"/Admin/User/ConfirmUser/" + userRegister.Id + "=" + userConfirm.ConfirmString;
                SendMailConfirm(register.Email, register.FirstName, contentMail);

                _unitOfWork.Save();

                if (userRegister != null && customerRegister != null && userConfirmRegister != null) return 1;
                return 0;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when register user in UserService", e);
                return -1;
            }

        }

        public int CreateEmployeeAccount(CreateEmployeeViewModel register)
        {
            try
            {
                if (CheckEmailExisted(register.Email)) return 0;
                var user = new User
                {
                    Email = register.Email,
                    Password = register.PassEmail,
                    Role = register.Role,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Gender = register.Gender,
                    BirthDay = register.BirthDay,
                    Phone = register.Phone,
                    Address = register.Address,
                    CreateBy = 1,
                    CreateAt = DateTime.UtcNow
                };

                var userRegister = _unitOfWork.UserRepository.Create(user);

                var employee = new Employee
                {
                    Gmail = register.Gmail,
                    PassGmail = register.PassGmail,
                    Status = true,
                    StoreId = register.StoreId
                };

                var employeeRegister = _unitOfWork.EmployeeRepository.Create(employee);

                _unitOfWork.Save();

                if (userRegister != null && employeeRegister != null) return 1;
                return 0;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when create customer in UserService", e);
                return -1;
            }
        }

        public int CreateAdminAccount(CreateAdminViewModel register)
        {
            try
            {
                if (CheckEmailExisted(register.Email)) return 0;
                var user = new User
                {
                    Email = register.Email,
                    Password = register.PassEmail,
                    Role = (byte)EnumRole.Admin,
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Gender = register.Gender,
                    BirthDay = register.BirthDay,
                    Phone = register.Phone,
                    Address = register.Address,
                    CreateBy = 1,
                    CreateAt = DateTime.UtcNow
                };

                var userRegister = _unitOfWork.UserRepository.Create(user);

                var employee = new Employee
                {
                    UserId = userRegister.Id,
                    Gmail = register.Gmail,
                    PassGmail = register.PassGmail,
                    Status = true
                };

                var adminRegister = _unitOfWork.EmployeeRepository.Create(employee);

                _unitOfWork.Save();

                if (userRegister != null && adminRegister != null) return 1;
                return 0;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when create customer in UserService", e);
                return -1;
            }
        }

        public AuthenticationViewModel Authentication(AuthenticationViewModel authenticationViewModel)
        {
            try
            {
                var user = _unitOfWork.UserRepository.Get(x => x.Email == authenticationViewModel.Email && x.Password == authenticationViewModel.Password);
                var authenticationView = _mapper.Map<AuthenticationViewModel>(user);
                return authenticationView;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when Authentication", e);
                return null;
            }
        }

        public IEnumerable<CustomerViewModel> GetAllCustomer()
        {
            try
            {
                var listCustomer = _unitOfWork.UserRepository.GetMany(x => x.Role == (int)EnumRole.Customer);
                var listCustomerView = _mapper.Map<IEnumerable<CustomerViewModel>>(listCustomer);
                return listCustomerView;
            }
            catch (Exception e)
            {
                Log.Error("Have error when GetAllCustomer on UserService", e);
                return null;
            }
        }

        public IEnumerable<EmployeeViewModel> GetAllEmployee()
        {
            try
            {
                var listEmployee = _unitOfWork.UserRepository.GetMany(x => x.Role != (int)EnumRole.Admin && x.Role != (int)EnumRole.Customer);
                var listEmployeeView = _mapper.Map<IEnumerable<EmployeeViewModel>>(listEmployee);
                return listEmployeeView;
            }
            catch (Exception e)
            {
                Log.Error("Have error when GetAllEmployee on UserService", e);
                return null;
            }
        }

        public IEnumerable<AdminViewModel> GetAllAdmin()
        {
            try
            {
                var listAdmin = _unitOfWork.UserRepository.GetMany(x => x.Role == (int)EnumRole.Admin);
                var listAdminView = _mapper.Map<IEnumerable<AdminViewModel>>(listAdmin);
                return listAdminView;
            }
            catch (Exception e)
            {
                Log.Error("Have error when GetAllAdmin on UserService", e);
                return null;
            }
        }

        public bool UpdateInfoUser(EditUserViewModel customerModel)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(customerModel.Id);
                if (user == null) return false;
                user.FirstName = customerModel.FirstName;
                user.LastName = customerModel.LastName;
                user.Phone = customerModel.Phone;
                user.Gender = customerModel.Gender;
                user.Address = customerModel.Address;
                user.UpdateAt = DateTime.UtcNow;

                _unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when UpdateInfoCustomer", e);
                return false;
            }
        }

        /// <summary>
        /// Method ChangeStatus User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ChangeStatus(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            user.Status = !user.Status;
            _unitOfWork.Save();
            return user.Status;
        }

        public bool UpdateEmployee(UpdateEmployeeViewModel employeeModel)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(employeeModel.Id);
                var employee = _unitOfWork.EmployeeRepository.Get(x => x.UserId == employeeModel.Id);
                if (user == null || employee == null) return false;
                user.Role = employeeModel.Role;
                employee.StoreId = employeeModel.StoreId;
                _unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when Update Employee in UserService", e);
                return false;
            }
        }

        /// <summary>
        /// Method send mail from Customer to Admin, config with method SMTP by MaiKit
        /// </summary>
        /// <param name="contactViewModel"></param>
        /// <param name="emailConfigModel"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private static bool SendMailConfirm(string Email, string Name,
            string content)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(EmailConfig.NameMailSend, EmailConfig.MailSend));
            message.To.Add(new MailboxAddress(Name,
                Email));
            message.Subject = "Xác thực tài khoản trên website...";

            message.Body = new TextPart(Constants.Plain)
            {
                Text = content
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(Constants.SmtpClient, 587);


                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove(Constants.Xoauth2);

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(EmailConfig.MailSend, EmailConfig.PasswordMailSend);

                    client.Send(message);
                    client.Disconnect(true);
                    return true;
                }
                catch (Exception e)
                {
                    Log.Error(e, "Have an error when send mail in UserService");
                }

                return false;
            }
        }

        private string InitConfirmString()
        {
            var confirmString = Guid.NewGuid();
            return confirmString.ToString();
        }

        public bool ConfirmUser(string confirmString)
        {
            try
            {
                var id = Convert.ToInt32(confirmString.Split("=")[0]);
                var confirmStringTrue = confirmString.Split("=")[1];
                var user = _unitOfWork.UserRepository.GetById(id);
                var userConfirm = _unitOfWork.UserConfirmRepository.Get(x => x.UserId == id);
                if (userConfirm.ConfirmString != confirmStringTrue) return false;
                user.IsConfirm = true;
                user.Status = true;
                _unitOfWork.UserConfirmRepository.Delete(userConfirm.Id);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when ConfirmUser in UserServie", e);
                return false;
            }
        }

        public UpdateEmployeeViewModel GetEmployee(int id)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(id);
                var employee = _unitOfWork.EmployeeRepository.Get(x => x.UserId == id);
                var employeeModel = new UpdateEmployeeViewModel
                {
                    Id = id,
                    StoreId = employee.StoreId,
                    Role = user.Role
                };

                return employeeModel;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when GetEmployee in UserService", e);
                return null;
            }

        }

        private bool CheckEmailExisted(string email)
        {
            var user = _unitOfWork.UserRepository.Get(x => x.Email == email);
            return user != null ? true : false;
        }

        /// <summary>
        ///Method LoadData return parameter provide properties for Action LoadDataController 
        /// </summary>
        /// <param name="dtParameters"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<EmployeeViewModel>, int, int> LoadEmployee(DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            string orderCriteria;
            bool orderAscendingDirection;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == ParamConstants.Asc;
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = ParamConstants.Id;
                orderAscendingDirection = true;
            }

            var user = GetAllEmployee();
            var usertViewModels = _mapper.Map<IEnumerable<EmployeeViewModel>>(user);

            if (!string.IsNullOrEmpty(searchBy))
            {
                usertViewModels = usertViewModels.Where(r =>
                        r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Role.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.FirstName.ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Email.ToUpper().Contains(searchBy.ToUpper()) ||
                        r.BirthDay.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Status.ToString().ToUpper().Equals(searchBy.ToUpper()));
            }

            usertViewModels = orderAscendingDirection
                ? usertViewModels.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : usertViewModels.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var viewModels = usertViewModels.OrderByDescending(x => x.Email).ToArray();
            var filteredResultsCount = viewModels.ToArray().Length;
            var totalResultsCount = user.Count();

            var tuple = new Tuple<IEnumerable<EmployeeViewModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        ///Method LoadData return parameter provide properties for Action LoadDataController 
        /// </summary>
        /// <param name="dtParameters"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<AdminViewModel>, int, int> LoadAdmin(DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            string orderCriteria;
            bool orderAscendingDirection;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == ParamConstants.Asc;
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = ParamConstants.Id;
                orderAscendingDirection = true;
            }

            var user = GetAllAdmin();
            var usertViewModels = _mapper.Map<IEnumerable<AdminViewModel>>(user);

            if (!string.IsNullOrEmpty(searchBy))
            {
                usertViewModels = usertViewModels.Where(r =>
                        r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.FirstName.ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Email.ToUpper().Contains(searchBy.ToUpper()) ||
                        r.BirthDay.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Status.ToString().ToUpper().Equals(searchBy.ToUpper()));
            }

            usertViewModels = orderAscendingDirection
                ? usertViewModels.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : usertViewModels.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var viewModels = usertViewModels.OrderByDescending(x => x.Email).ToArray();
            var filteredResultsCount = viewModels.ToArray().Length;
            var totalResultsCount = user.Count();

            var tuple = new Tuple<IEnumerable<AdminViewModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        /// <summary>
        ///Method LoadData return parameter provide properties for Action LoadDataController 
        /// </summary>
        /// <param name="dtParameters"></param>
        /// <returns></returns>
        public Tuple<IEnumerable<CustomerViewModel>, int, int> LoadCustomer(DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;
            string orderCriteria;
            bool orderAscendingDirection;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == ParamConstants.Asc;
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = ParamConstants.Id;
                orderAscendingDirection = true;
            }

            var user = GetAllCustomer();
            var usertViewModels = _mapper.Map<IEnumerable<CustomerViewModel>>(user);

            if (!string.IsNullOrEmpty(searchBy))
            {
                usertViewModels = usertViewModels.Where(r =>
                        r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.FirstName.ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Email.ToUpper().Contains(searchBy.ToUpper()) ||
                        r.BirthDay.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                        r.Status.ToString().ToUpper().Equals(searchBy.ToUpper()));
            }

            usertViewModels = orderAscendingDirection
                ? usertViewModels.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
                : usertViewModels.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var viewModels = usertViewModels.OrderByDescending(x => x.Email).ToArray();
            var filteredResultsCount = viewModels.ToArray().Length;
            var totalResultsCount = user.Count();

            var tuple = new Tuple<IEnumerable<CustomerViewModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        //public string GetClaimUserMail()
        //{
        //    var claimEmail = _httpContext.User.FindFirst(c => c.Type == "Email").Value;
        //    return claimEmail;
        //}

        //public int GetClaimUserId()
        //{
        //    var claimId = Convert.ToInt32(_httpContext.User.FindFirst(c => c.Type == "Id").Value);
        //    return claimId;
        //}
    }
}
