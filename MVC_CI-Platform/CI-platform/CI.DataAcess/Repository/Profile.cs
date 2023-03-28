using CI.DataAcess.Repository.IRepository;
using CI.Models;
using CI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.DataAcess.Repository
{
    public class Profile : IProfile
    {
        private readonly CiPlatformContext _db;

        public Profile(CiPlatformContext db)
        {
            _db = db;
        }
        public ProfileViewModel Get_Initial_Details(int country)
        {
            if (country == 0)
            {
                List<Country> countries = _db.Countries.ToList();
                List<Skill> skills = _db.Skills.ToList();
                List<City> cities = _db.Cities.ToList();
                return new ProfileViewModel { Countries = countries, Cities = cities, Skills = skills };
            }
            else
            {
                List<City> cities = _db.Cities.Where(c => c.CountryId == country).ToList();
                return new ProfileViewModel { Cities = cities };
            }
        }
    }
}
