using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MailApp
{
    class fileChecker
    {
        public static void createFolder()
        {
            string curFile = @"Data";
            if(!Directory.Exists(curFile))
                Directory.CreateDirectory("Data");
        }
    }
}
