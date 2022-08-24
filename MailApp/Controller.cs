using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MailApp
{
    class Controller
    {
        private AppData _emailData;
        private AppOwner _owner;
        private List<Email> _messages;
        private mailSender _mailUser;
        private mailGetter _mailGetter;

        //Controller initialisation
        public Controller() {
            _emailData = new AppData();
            _owner = new AppOwner();
            getUserData();
        }
        public async Task<List<string>> openEmailFile()
        {
            FileStream jsonFile;
            try 
            {
                jsonFile = new FileStream("Data\\Emails.json", FileMode.OpenOrCreate);
            }
            catch
            {
                fileChecker.createFolder();
                jsonFile = new FileStream("Data\\Emails.json", FileMode.OpenOrCreate);
            }
            try
            {
                _emailData = await JsonSerializer.DeserializeAsync<AppData>(jsonFile);
            }
            catch
            {
                _emailData = new AppData();
                jsonFile.Close();
                saveEmail("");
            }
            finally
            {
                jsonFile.Close();
            }
            return _emailData.Emails;
        }
        public async void getUserData()
        {
            FileStream userFile;
            try
            {
                userFile = new FileStream("Data\\User.json", FileMode.OpenOrCreate);
            }
            catch
            {
                fileChecker.createFolder();
                userFile = new FileStream("Data\\User.json", FileMode.OpenOrCreate);
            }

            try
            {
                _owner = await JsonSerializer.DeserializeAsync<AppOwner>(userFile);
                _mailUser = new mailSender(_owner);
                _mailGetter = new mailGetter(_owner.Email, _owner.Password);
            }
            catch
            {
                userFile.Close();
                saveUserData("", "", "");
            }
            finally
            {
                userFile.Close();
            }
        }
        public async void saveEmail(string emailJson)
        {
            addSavedEmail(emailJson);
            var jsonFile = new FileStream("Data\\Emails.json", FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync<AppData>(jsonFile, _emailData);
            jsonFile.Close();
        }
        public async void saveUserData(string email, string password, string login)
        {
            var userFile = new FileStream("Data\\User.json", FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync<AppOwner>(userFile, new AppOwner(email, password, login));
            userFile.Close();
        }
        public void sendMessage(string emailTo, string emailFrom, string Message, List<string> FilePath, bool saving)
        {
            if (saving)
                saveEmail(emailTo);
            _mailUser.sendMessage(emailTo, emailFrom, Message, FilePath).GetAwaiter();
        }
        public void addSavedEmail(string savingEmail)
        {
            if(_emailData.Emails.IndexOf(savingEmail) < 0)
                _emailData.Emails.Add(savingEmail);
        }
        
        //Work with View
        public List<string> getInbox(int Index)
        {
            List<string> Data = new List<string>();
            var messages = _mailGetter.GetPage(Index);
            foreach(Email message in messages)
            {
                Data.Add($"{message.FromEmail}/{message.FromName}/{message.Subject}");
            }
            return Data;
        }
        public string getMessage(int indexPage, int indexMessage)
        {
            //добавить индекс страницы
            return _mailGetter.GetMessage(indexPage, indexMessage);
        }
        public string getIncodeUserPassword()
        {
            string incodePassword = "";
            for (int i = 0; i < _owner.Password.Length; i++)
            {
                incodePassword += '*';
            }
            return incodePassword;
        }
        public bool checkUser()
        {
            if (_owner.Email == "" && _owner.Password == "" && _owner.Login == "")
            {
                return false;
            }
            return true;
        }
        public string getUserEmail()
        {
            return _owner.Email;
        }
        public string getUserPassword()
        {
            return _owner.Password;
        }
        public string getUserLogin()
        {
            return _owner.Login;
        }
        public int GetPages()
        {
            try
            {
                if (_mailGetter is null)
                    return 0;
                else
                    return _mailGetter.GetPages();
            }
            catch
            {
                return 0;
            }
        }
    }
}
