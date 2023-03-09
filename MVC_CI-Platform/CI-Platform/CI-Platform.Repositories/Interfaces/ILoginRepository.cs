using CI_Platform.Models;
using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        User getUserByEmail(string email);
        User getUserByPhone(string Phonenumber);
        UserToken getTokenByEmail(string email);
        void InsertUser(Register user);
        void InsertToken(UserToken token);
        void UpdateUser(User user);
        void UpdateToken(UserToken token);
        void Save();
        string TokenGenerate();
        void SendMail(string body,string mailid);


    }
}
