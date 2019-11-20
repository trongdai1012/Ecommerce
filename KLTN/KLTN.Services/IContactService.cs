using System;
using System.Collections.Generic;
using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Contact;

namespace KLTN.Services
{
    public interface IContactService
    {
        Tuple<IEnumerable<ContactModel>, int, int> LoadContact(DTParameters dtParameters);

        Tuple<ContactModel, int> GetContactById(int? id);

        bool SendContact(string title, string content);
//
//        IEnumerable<BrandViewModel> ReplyContact();
    }
}