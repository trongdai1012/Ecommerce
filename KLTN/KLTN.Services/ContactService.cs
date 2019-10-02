using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using KLTN.Common;
using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Contact;
using KLTN.Services.Repositories;
using Microsoft.AspNetCore.Http;

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
        
        public Tuple<IEnumerable<ContactModel>, int, int> LoadContact(DTParameters dtParameters)
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
                                select new ContactModel
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

            var tuple = new Tuple<IEnumerable<ContactModel>, int, int>(viewModels, filteredResultsCount,
                totalResultsCount);

            return tuple;
        }

        public Tuple<ContactModel, int> GetContactById(int? id)
        {
            throw new NotImplementedException();
        }
    }
}