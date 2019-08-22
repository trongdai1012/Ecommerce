using AutoMapper;
using KLTN.Common;
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

                _unitOfWork.CustomerRepository.Create(customer);

                _unitOfWork.Save();

                var userConfirm = new UserConfirm
                {
                    UserId = userRegister.Id,
                    ConfirmString = InitConfirmString()
                };

                _unitOfWork.UserConfirmRepository.Create(userConfirm);

                var contentMail =
                    "Cảm ơn bạn đã đăng ký tài khoản trên website của chúng tôi!"
                    + @"https://localhost:44338"
                    + Environment.NewLine
                    + Environment.NewLine
                    + "Vui lòng click vào link bên dưới để kích hoạt tài khoản của bạn"
                    + Environment.NewLine
                    + @"https://localhost:44338"+@"/Admin/User/ConfirmUser/"+userRegister.Id+"="+userConfirm.ConfirmString;
                SendMailConfirm(register.Email,register.FirstName,contentMail);

                _unitOfWork.Save();
                return 1;
            }
            catch (Exception e)
            {
                Log.Error("Have an error when register user in UserService", e);
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
