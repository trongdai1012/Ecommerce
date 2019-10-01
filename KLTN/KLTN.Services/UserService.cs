using AutoMapper;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.Common.Infrastructure;
using KLTN.DataAccess.Models;
using KLTN.DataModels;
using KLTN.DataModels.Models.Users;
using KLTN.Services.Repositories;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
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
        private readonly HttpContext _httpContext;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public UserViewModel GetUserById(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            var userModel = _mapper.Map<UserViewModel>(user);
            return userModel;
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
                    ProvinceId = register.ProvinceId,
                    DistrictId = register.DistrictId,
                    PrecinctId = register.PrecinctId,
                    ProvinceName = register.ProvinceName,
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
                    + @"http://"+_httpContext.Request.Host
                    + Environment.NewLine
                    + Environment.NewLine
                    + "Vui lòng click vào link bên dưới để kích hoạt tài khoản của bạn"
                    + Environment.NewLine
                    + @"https://"+_httpContext.Request.Host + @"/Admin/User/ConfirmUser/" + userRegister.Id + "=" + userConfirm.ConfirmString;
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

        public int ForgotPassword(string email)
        {
            var forgot = new ConfirmForgot();
            try
            {
                var user = _unitOfWork.UserRepository.Get(x => x.Email == email);
                if (user == null) return 0;

                var confirmString = InitConfirmString();

                var confirmPas = new ConfirmForgot
                {
                    UserId = user.Id,
                    ConfirmString = confirmString
                };

                forgot = _unitOfWork.ForgotRepository.Create(confirmPas);
                _unitOfWork.Save();

                var content =
                    "Bạn đã gửi một yêu cầu lấy lại mật khẩu từ website "
                    + _httpContext.Request.Scheme + @"://" + _httpContext.Request.Host
                    + Environment.NewLine
                    + "Vui lòng bấm vào link sau để thiết lập lại mật khẩu"
                    + Environment.NewLine
                    + _httpContext.Request.Scheme + @"://" + _httpContext.Request.Host + @"/Account/RetypePassword/" + user.Id + "=" + confirmPas.ConfirmString;

                SendMailConfirmForgot(user.Email, user.FirstName + " " + user.LastName, content);
                return 1;
            }
            catch(Exception e)
            {
                var result = _unitOfWork.ForgotRepository.GetById(forgot.Id);
                if(result==null) _unitOfWork.ForgotRepository.Delete(_unitOfWork.ForgotRepository.GetById(forgot.Id));
                Log.Error("Have an error when ForgotPassword in UserService", e);
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
                    CreateBy = GetClaimUserId(),
                    IsConfirm = true,
                    CreateAt = DateTime.UtcNow
                };

                var userRegister = _unitOfWork.UserRepository.Create(user);

                var employee = new Employee
                {
                    PassEmail = register.PassEmail
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
                    CreateBy = GetClaimUserId(),
                    IsConfirm = true,
                    CreateAt = DateTime.UtcNow
                };

                var userRegister = _unitOfWork.UserRepository.Create(user);

                var employee = new Employee
                {
                    UserId = userRegister.Id,
                    PassEmail = register.PassEmail
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

        public Tuple<AuthenticationViewModel, int> Authentication(AuthenticationViewModel authenticationViewModel)
        {
            try
            {
                var user = _unitOfWork.UserRepository.Get(x => x.Email == authenticationViewModel.Email);

                if (user == null) return new Tuple<AuthenticationViewModel, int>(null, -1);

                if (user.Password != authenticationViewModel.Password)
                    return new Tuple<AuthenticationViewModel, int>(null, -2);

                if (user.IsConfirm == false) return new Tuple<AuthenticationViewModel, int>(null, 0);

                var authenticationView = _mapper.Map<AuthenticationViewModel>(user);

                return new Tuple<AuthenticationViewModel, int>(authenticationView,1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when Authentication", e);
                return new Tuple<AuthenticationViewModel, int>(null,-3);
            }
        }

        private IEnumerable<CustomerViewModel> GetAllCustomer()
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

        private IEnumerable<EmployeeViewModel> GetAllEmployee()
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

        private IEnumerable<AdminViewModel> GetAllAdmin()
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

        private static bool SendMailConfirmForgot(string Email, string Name,
            string content)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(EmailConfig.NameMailSend, EmailConfig.MailSend));
            message.To.Add(new MailboxAddress(Name,
                Email));
            message.Subject = "Lấy lại mật khẩu";

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

        public bool ConfirmForgotPassword(RetypePassword retypePassword)
        {

            try
            {
                var id = Convert.ToInt32(retypePassword.ConfirmString.Split("=")[0]);
                var confirmStringTrue = retypePassword.ConfirmString.Split("=")[1];
                var user = _unitOfWork.UserRepository.GetById(id);
                var userConfirm = _unitOfWork.ForgotRepository.Get(x => x.UserId == id);
                if (userConfirm.ConfirmString != confirmStringTrue) return false;
                user.Password = retypePassword.Password;
                _unitOfWork.ForgotRepository.Delete(userConfirm.Id);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when ConfirmUser in UserServie", e);
                return false;
            }
        }

        public Tuple<AdminViewModel, int> GetAdmin(int id)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(id);
                if (user == null) return new Tuple<AdminViewModel, int>(null, 3);

                var admin = _unitOfWork.AdminRepository.Get(x => x.UserId == id);
                if (admin == null) return new Tuple<AdminViewModel, int>(null, 2);

                var adminModel = _mapper.Map<AdminViewModel>(user);
                return new Tuple<AdminViewModel, int>(adminModel, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when GetEmployee in UserService", e);
                return new Tuple<AdminViewModel, int>(null, -1);
            }
        }
        
        public Tuple<UpdateAdminViewModel, int> GetAdminUpdate(int id)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(id);
                if (user == null) return new Tuple<UpdateAdminViewModel, int>(null, 3);

                var admin = _unitOfWork.AdminRepository.Get(x => x.UserId == id);
                if (admin == null) return new Tuple<UpdateAdminViewModel, int>(null, 2);

                var adminModel = _mapper.Map<UpdateAdminViewModel>(user);
                return new Tuple<UpdateAdminViewModel, int>(adminModel, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when GetEmployee in UserService", e);
                return new Tuple<UpdateAdminViewModel, int>(null, -1);
            }
        }

        public Tuple<EmployeeViewModel, int> GetEmployee(int id)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(id);
                if (user == null) return new Tuple<EmployeeViewModel, int>(null, 2);

                var employee = _unitOfWork.EmployeeRepository.Get(x => x.UserId == id);
                if (employee == null) return new Tuple<EmployeeViewModel, int>(null, 3);

                var employeeModel = _mapper.Map<EmployeeViewModel>(user);

                return new Tuple<EmployeeViewModel, int>(employeeModel, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when GetEmployee in UserService", e);
                return new Tuple<EmployeeViewModel, int>(null, 0);
            }

        }

        public Tuple<CustomerViewModel, int> GetCustomer(int id)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(id);
                if (user == null) return new Tuple<CustomerViewModel, int>(null, 2);

                var customer = _unitOfWork.CustomerRepository.Get(x => x.UserId == id);
                if (customer == null) return new Tuple<CustomerViewModel, int>(null, 3);

                var customerModel = _mapper.Map<CustomerViewModel>(user);

                return new Tuple<CustomerViewModel, int>(customerModel, 1);
            }
            catch (Exception e)
            {
                Log.Error("Have an error when GetEmployee in UserService", e);
                return new Tuple<CustomerViewModel, int>(null, 0);
            }
        }

        public int UpdateAdmin(UpdateAdminViewModel adminModel)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(adminModel.Id);
                if (user == null) return 2;
                var admin = _unitOfWork.AdminRepository.Get(x => x.UserId == adminModel.Id);
                if (admin == null) return 3;
                user.Password = adminModel.Password;
                admin.PassEmail = adminModel.PassEmail;
                user.UpdateAt = DateTime.Now;
                user.UpdateBy = Convert.ToInt32(_httpContext.User.Identities);

                _unitOfWork.Save();
                return 1;
            }
            catch (Exception)
            {
                Log.Error("Have an error when update admin in UserService");
                return 0;
            }
        }

        public int UpdateEmployee(UpdateEmployeeViewModel employeeModel)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(employeeModel.Id);
                if (user == null) return 2;
                var employee = _unitOfWork.EmployeeRepository.Get(x => x.UserId == employeeModel.Id);
                if (employee == null) return 3;
                user.Role = employeeModel.Role;
                user.Password = employeeModel.Password;
                employee.PassEmail = employeeModel.PassEmail;
                user.UpdateAt = DateTime.Now;
                user.UpdateBy = Convert.ToInt32(_httpContext.User.Identities);

                _unitOfWork.Save();
                return 1;
            }
            catch (Exception)
            {
                Log.Error("Have an error when update employee in UserService");
                return 0;
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

        public int UpdateInfoUser(UpdateUserViewModel updateUser)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetById(GetClaimUserId());
                if (user == null) return 0;
                user.FirstName = updateUser.FirstName;
                user.LastName = updateUser.LastName;
                user.Phone = updateUser.Phone;
                user.Address = updateUser.Address;
                user.BirthDay = updateUser.BirthDay;
                user.Gender = updateUser.Gender;

                _unitOfWork.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when update info user", e);
                return -1;
            }
        }

//        public UpdateAdminViewModel GetAdminUpdate(int id)
//        {
//            var user = _unitOfWork.UserRepository.GetById(id);
//            var adminView = new UpdateAdminViewModel
//            {
//                Email = user.Email
//            };
//            return adminView;
//        }

        public string GetClaimUserMail()
        {
            var claimEmail = _httpContext.User.FindFirst(c => c.Type == "Email").Value;
            return claimEmail;
        }

        public int GetClaimUserId()
        {
            var claimId = Convert.ToInt32(_httpContext.User.Identities);
            return claimId;
        }
    }
}
