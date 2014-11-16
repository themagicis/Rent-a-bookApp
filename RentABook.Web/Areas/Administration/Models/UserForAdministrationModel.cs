using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentABook.Web.Areas.Administration.Models
{
    public class UserForAdministrationModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        public string UserName { get; set; }

        [Editable(false)]
        public string FullName { get; set; }

        public int Score { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsDeactivated { get; set; }
    }
}