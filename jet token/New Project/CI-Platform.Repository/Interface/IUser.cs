using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IUser
    {
        User login(User obj);

        User forgot(User obj);

        PasswordReset reset(User obj, string token);

        User register(User obj);

        UserProfileModel GetUserProfile(int userid);

        UserProfileModel GetCityListbyCountry(int countryid);

        bool ChangeUserPassword(int userid, string oldpass, string newpass);

        bool UserProfileUpdate(UserDetailsModel model, int userid);

        void ContactUs(int userid, ContactU model);

        List<CmsPage> GetPrivacyData();

        List<Banner> GetCrouselImages();
    }
}
