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
}
