using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITlabs_web_client.Models
{
    public class Account
    {
        public string login = "";

        public string dbname = "";

        public List<string> dbs = new List<string>();

        public List<string> tbls = new List<string>();

        public List<string> result = new List<string>();

        public string error = "";
    }
}