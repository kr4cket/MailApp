using Limilabs.Client.IMAP;
using Limilabs.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MailApp
{
    class mailGetter
    {
        private string _Email;
        private string _Password;
        private Imap _getter;
        public mailGetter(string email, string password)
        {
            _Email = email;
            _Password = password;
            Init();
        }
        public string getIMAPConnection()
        {
            return "imap." + _Email.Split('@')[1];
        }
        private void Init()
        {
            _getter = new Imap();
            _getter.Connect(getIMAPConnection());
            _getter.UseBestLogin(_Email,_Password);
        }
        private async Task<List<long>> _getUids()
        {
            List<long> uids = await _getter.SearchAsync(Flag.All);
            return uids;
        }
        private List<string> Separator(string text)
        {
            List<string> Data = new List<string>();
            text = text.Replace("\'", "");
            text = text.Replace("Name=", "");
            Data.AddRange(text.Split(" Address="));
            return Data;
        }
        private async Task<List<Email>> _getMessageList()
        {
            List<Email> messages = new List<Email>();
            List<long> uids = await _getUids();
            int range = 0;
            foreach (long uid in uids)
            {
                IMail email = new MailBuilder().CreateFromEml(_getter.GetMessageByUID(uid));
                messages.Add(new Email(email.Subject, email.Text, Separator(email.From.ToString())));
                if (range == 20)
                    break;
                else
                    range++;
            }
            return messages;
        }
        public async Task<List<Email>> openInbox()
        {
            _getter.SelectInbox();
            return await _getMessageList();
        }
        



    }
}
