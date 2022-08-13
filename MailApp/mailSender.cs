using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mail;

namespace MailApp
{
    class mailSender
    {
        private string _ownEmail;
        private string _passWord;
        private string _name;
        private MailAddress _sender;
        private MailMessage _message;
        private SmtpClient _smtpClient;
        public mailSender(AppOwner user)
        {
            _ownEmail = user.Email;
            _passWord = user.Password;
            _name = user.Login;

            if(_ownEmail != "")
                Init();
        }
        private void Init()
        {
            _sender = new MailAddress(_ownEmail, _name);
            _smtpClient = new SmtpClient(getSmtpConnection(), getSmtpPort())
            {
                Credentials = new NetworkCredential(_ownEmail, _passWord),
                EnableSsl = true
            };
        }
        private string getSmtpConnection()
        {
            return "smtp." + _ownEmail.Split('@')[1];
        }
        private int getSmtpPort()
        {
            if (_ownEmail.Split('@')[1] == "mail.ru")
                return 2525;
            else
                return 465;
        }
        public void addFiles(List<string> filePath)
        {
            if (filePath != null)
                foreach(string path in filePath)
                {
                    _message.Attachments.Add(new Attachment(path));
                }
        }
        public async Task sendMessage(string recName, string title, string text, List<string> path = null)
        {
            MailAddress receiver = new MailAddress(recName);
            _message = new MailMessage(_sender, receiver)
            {
                Subject = title,
                Body = text
            };
            addFiles(path);

            await _smtpClient.SendMailAsync(_message);
        }
    }
}
