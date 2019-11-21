using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KLTN.Common.Datatables;
using KLTN.DataModels.Models.Contact;

namespace KLTN.Services
{
    public interface IContactService
    {
        Tuple<IEnumerable<ContactViewModel>, int, int> LoadContact(DTParameters dtParameters);

        Tuple<ContactViewModel, int> GetContactById(int id);

        bool SendContact(string title, string content);

        Task<bool> ReplyContact(ContactViewModel contactModel);
    }
}