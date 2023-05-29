using CI_Platform.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CI_Platform.Entities.ViewModels
{
    public class UserAdminModel
    {
        public User userbyAdmin  { get; set; } = new User();

        public List<SelectListItem> cityList { get; set; } = new List<SelectListItem> { };

        public List<SelectListItem> countryList { get; set; } = new List<SelectListItem> { };
    }
}
