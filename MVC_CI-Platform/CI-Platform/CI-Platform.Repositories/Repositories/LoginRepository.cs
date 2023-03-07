using System.Net.Mail;
using System.Net;
using CI_Platform.Models.Models;
using CI_Platform.Repositories.Interfaces;
using CI_Platform.Data;

namespace CI_Platform.Repositories.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly CiPlatformContext _db;

        public LoginRepository(CiPlatformContext db)
        {
            _db = db;
        }

        public Token getTokenByEmail(string email)
        {
            return _db.Tokens.Where(x => x.Email == email).FirstOrDefault();
        }

        public User getUserByEmail(string email)
        {
            return _db.Users.Where(x => x.Email == email).FirstOrDefault();
        }

        public User getUserByPhone(string Phonenumber)
        {
            return _db.Users.Where(x => x.PhoneNumber == Phonenumber).FirstOrDefault();
        }

        public void InsertUser(User user)
        {
            _db.Users.Add(user);
        }
        
        public void InsertToken(Token token)
        {
            _db.Tokens.Add(token);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _db.Users.Update(user);
        }

        public void UpdateToken(Token token)
        {
            _db.Tokens.Update(token);
        }

        public string TokenGenerate()
        {
            string[] str = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "_", "*", "$", "!" };
            string token = "";
            Random r = new Random();
            for (int i = 0; i < 20; i++)
            {
                token += str[r.Next(0, str.Length)];
            }
            return token;
        }

        public void SendMail(string body, string mailid)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("tatvasoft51@gmail.com");
                message.To.Add(new MailAddress(mailid));
                message.Subject = "Your Password For CI-Platform";
                message.IsBodyHtml = true;
                message.Body = body;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("tatvasoft51@gmail.com", "vlpzyhibrvpaewte");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }
    }
}
