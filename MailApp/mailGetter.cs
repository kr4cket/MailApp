using Limilabs.Client.IMAP;
using Limilabs.Mail;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailApp
{
    class mailGetter
    {
        private string _Email;
        private string _Password;
        private Imap _getter;
        private Dictionary<int,List<Email>> _Messages;
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
            _Messages = new Dictionary<int, List<Email>>();
            Task.Run(() => getMessagePage());
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
        private async void getMessagePage()
        {
            _getter.SelectInbox();
            List<Email> messages = new List<Email>();
            List<long> uids = await _getUids();
            IMail email;
            uids.Reverse();
            int range = 0;
            int page = 1;
            foreach (long uid in uids)
            {
                if(range == 0)
                    messages = new List<Email>();
                email = new MailBuilder().CreateFromEml(_getter.GetMessageByUID(uid));
                messages.Add(new Email(email.Subject, email.Text, Separator(email.From.ToString())));
                if (range == 19)
                {
                    _Messages.Add(page, messages);
                    page++;
                    range = 0;
                }
                else
                    range++;
            }
        }
        public int GetPages()
        {
            try
            {
                return _Messages.Count;
            }
            catch
            {
                return 0;
            }
        }
        public List<Email> GetPage(int Index)
        {
            return _Messages[Index];
        }
        public string GetMessage(int indexPage, int indexMessage)
        {
            return _Messages[indexPage][indexMessage].MessageText;
        }
    }
}
