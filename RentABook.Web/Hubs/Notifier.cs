using Microsoft.AspNet.SignalR;
using RentABook.Models.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentABook.Web.Hubs
{
    public class Notifier : Hub, INotifier
    {
        private IHubContext context;

        public Notifier()
        {
            context = GlobalHost.ConnectionManager.GetHubContext<Notifier>();
        }

        public void Test()
        {
            context.Clients.All.test();
        }

        public void BookChangedState(int bookId, BookState state)
        {
            context.Clients.All.bookChangedState(bookId, state.ToString());
        }
    }
}