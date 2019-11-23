using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.DataAccess.Models;
using KLTN.DataModels;
using KLTN.DataModels.Models.Contact;
using KLTN.Services.Repositories;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Serilog;

namespace KLTN.Services
{
    public class ContactService : IContactService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        private readonly HttpContext _httpContext;
        /// <summary>
        /// 
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor category service
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="httpContext"></param>
        public ContactService(IMapper mapper, IHttpContextAccessor httpContext, IUnitOfWork unitOfWork)
        {
            _httpContext = httpContext.HttpContext;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public Tuple<IEnumerable<ContactViewModel>, int, int> LoadContact(DTParameters dtParameters)
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

            var listContact = (from contact in _unitOfWork.ContactRepository.ObjectContext
                                join usc in _unitOfWork.UserRepository.ObjectContext on contact.CreateBy equals usc.Id
                                select new ContactViewModel
                                {
                                    Id = contact.Id,
                                    Title = contact.Title,
                                    CreateBy = usc.Email,
                                    ContactAt = contact.ContactAt,
                                    Status = contact.Status
                                });

            if (!string.IsNullOrEmpty(searchBy))
            {
                listContact = listContact.Where(r =>
                        searchBy != null && (r.Id.ToString().ToUpper().Contains(searchBy.ToUpper()) ||
                                             r.Title.ToString().ToUpper().Contains(searchBy.ToUpper())));
            }

            listContact = orderAscendingDirection
               ? listContact.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Asc)
               : listContact.AsQueryable().OrderByDynamic(orderCriteria, LinqExtensions.Order.Desc);

            var viewModels = listContact.OrderBy(x => x.Id).ToArray();
            var filteredResultsCount = viewModels.ToArray().Length;
            var totalResultsCount = viewModels.Length;

            var tuple = new Tuple<IEnumerable<ContactViewModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        public Tuple<ContactViewModel, int> GetContactById(int id)
        {
            try
            {
                var contact = (from cont in _unitOfWork.ContactRepository.ObjectContext
                               join usc in _unitOfWork.UserRepository.ObjectContext on cont.CreateBy equals usc.Id
                               where cont.Id == id
                               select new ContactViewModel
                               {
                                   Id = cont.Id,
                                   Title = cont.Title,
                                   Content = cont.Content,
                                   ContactAt = cont.ContactAt,
                                   CreateBy = usc.Email,
                                   ContentReply = cont.ContentReply,
                                   HandlerBy = usc.Email,
                                   ReplyContactAt = cont.ReplyContactAt,
                                   Status = cont.Status
                               }).FirstOrDefault();

                if (contact == null) return new Tuple<ContactViewModel, int>(null, 0);

                return new Tuple<ContactViewModel, int>(contact, 1);
            }
            catch(Exception e)
            {
                Log.Error("Have an error when get contact in service", e);
                return new Tuple<ContactViewModel, int>(null, -1);
            }
        }

        public bool SendContact(string title, string content)
        {
            try
            {
                var contact = new Contact
                {
                    Title = title,
                    Content = content,
                    ContactAt = DateTime.UtcNow,
                    CreateBy = GetClaimUserId()
                };

                _unitOfWork.ContactRepository.Create(contact);
                _unitOfWork.Save();
                return true;
            }catch(Exception e)
            {
                Log.Error("Have an error when send Contact by Customer", e);
                return false;
            }
        }

        public async Task<bool> ReplyContact(ContactViewModel contactModel)
        {
            try
            {
                var contact = _unitOfWork.ContactRepository.GetById(contactModel.Id);

                contact.ContentReply = contactModel.ContentReply;
                contact.HandlerId = GetClaimUserId();
                contact.ReplyContactAt = DateTime.UtcNow;

                var user = _unitOfWork.UserRepository.GetById(contact.CreateBy);

                var result = await SendMail(user.Email, user.LastName + " " + user.LastName, contact.ContentReply);

                if (result)
                {
                    _unitOfWork.Save();
                    return true;
                }

                return false;
            }
            catch(Exception e)
            {
                Log.Error("Have an error when reply contact in service", e);
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
        private async static Task<bool> SendMail(string Email, string Name,
            string content)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(EmailConfig.NameMailSend, EmailConfig.MailSend));
            message.To.Add(new MailboxAddress(Name,
                Email));
            message.Subject = "Trả lời liên hệ khách hàng";

            message.Body = new TextPart(Constants.Plain)
            {
                Text = content
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(Constants.SmtpClient, 587);


                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove(Constants.Xoauth2);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(EmailConfig.MailSend, EmailConfig.PasswordMailSend);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    return true;
                }
                catch (Exception e)
                {
                    Log.Error(e, "Have an error when send mail in contactService");
                }

                return false;
            }
        }

        public int GetClaimUserId()
        {
            var claimId = Convert.ToInt32(_httpContext.User.FindFirst(x => x.Type == "Id").Value);
            return claimId;
        }
    }
}