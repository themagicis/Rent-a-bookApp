using RentABook.Models.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentABook.Web.Hubs
{
    public interface INotifier
    {
        void BookChangedState(int bookId, BookState state);
    }
}
