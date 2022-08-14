using Limilabs.Client.IMAP;
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
            List<long> uids = await _getter.SearchAsync(Flag.Unseen);
            return uids;
        }
        private async Task<List<MessageInfo>> _getMessageList()
        {
            List<MessageInfo> messages = await _getter.GetMessageInfoByUIDAsync(await _getUids());
            return messages;
        }
        public async Task<List<string>> openInbox()
        {
            _getter.SelectInbox();
            List<string> Inbox = new List<string>();
            var messages = await _getMessageList();
            foreach (MessageInfo message in messages)
            {
                //Inbox.Add($"Subject {message.Envelope.Subject}/ From {message.Envelope.From}/ To {message.Envelope.To}");
                Inbox.Add($"Subject {message.Envelope.Subject}");
            }
            return Inbox;
        }
        



    }
}
