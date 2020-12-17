using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LibrarySystem
{
    class ConnectionString
    {
        public string connectString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\LibraryDB.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        public string connectString2 = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|\\ooo.mdf;Integrated Security=True;User Instance=True";
    }
}
