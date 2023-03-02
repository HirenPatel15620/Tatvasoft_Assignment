using CI_Platform.Models;
using CI_Platform.Models.Models;
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
        Token getTokenByEmail(string email);
        void InsertUser(User user);
        void InsertToken(Token token);
        void UpdateUser(User user);
        void UpdateToken(Token token);
        void Save();
        string TokenGenerate();
        void SendMail(string body,string mailid);
    }
}
