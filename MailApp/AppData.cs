using System;
using System.Collections.Generic;
using System.Text;

namespace MailApp
{
    class AppData
    {
        public List<string> Emails { get; set; } = new List<string>();
    }
    class AppOwner
    {
        public string Login { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public AppOwner()
        {

         }
        public AppOwner(string email, string password, string login)
        {
            Email = email;
            Password = password;
            Login = login;
        }
    }
    class Email
    {
        public string Subject { get; set; }
        public string MessageText { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public Email(string Subject, string MessageText, List<string> From)
        {
            this.Subject = Subject;
            this.MessageText = MessageText;
            FromEmail = From[1];
            FromName = From[0];
        }
    }
}
