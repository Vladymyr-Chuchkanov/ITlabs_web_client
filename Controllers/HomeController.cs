using ITlabs_web_client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ITlabs_web_client.Controllers
{
    public class HomeController : Controller
    {
        string server = "http://localhost:8000?wsdl";
        static Account acc;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Login Page";
            acc = new Account();
            return View(acc);
        }

        public ActionResult OpenDb(string dbs_list)
        {
            string post_str = "{ \"get_tables_names\":{\"login\":\"" + acc.login + "\",\"name\":\"" + dbs_list + "\" }}";
            

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(server);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(post_str);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var serializer = new JavaScriptSerializer();
                var result = streamReader.ReadToEnd();
                var json = serializer.Deserialize<string>(result);
                json = json.Replace("\"", "");
                json = json.Replace("[", "");
                json = json.Replace("]", "");
                var arr = json.Split(',');
                acc.tbls = new List<string>();
                acc.dbname = dbs_list;
                foreach (var x in arr)
                {                    
                    acc.tbls.Add(x.Trim());
                }
                
            }
            return View("DB", acc);
        }


        public PartialViewResult Table_mult(string left, string right)
        {
            string post_str = "{ \"table_mult\":{\"login\":\"" + acc.login + "\",\"dbname\":\"" + acc.dbname + "\",\"tbl1\":\"" + left + "\",\"tbl2\":\"" + right + "\" }}";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(server);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(post_str);
            }
            List<string> table_mult_result = new List<string>();
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var serializer = new JavaScriptSerializer();
                var result = streamReader.ReadToEnd();
                var json = serializer.Deserialize<string>(result);
                json = json.Replace("]]", "]");
                string spl = "],";
                json = json.Replace(spl,"]");
                var els = json.Split(']');


                var last = els.GetValue(els.Length - 2).ToString();
                string t = last;
                t = t.Replace("\"", "");
                t = t.Replace("[", "");
                t = t.Replace("]", "");
                table_mult_result.Add(t.Trim());
                foreach (var el in els)
                {
                    if (el != last)
                    {
                        t = el;
                        t = t.Replace("\"", "");
                        t = t.Replace("[", "");
                        table_mult_result.Add(t.Trim());
                    }
                }

            }
            acc.result = table_mult_result;
            return PartialView(acc);
        }


        public ActionResult Log_in(string login)
        {

            string post_str = "{ \"log_in\":{\"login\":\"" + login + "\" }}";


            var temp = new Dictionary<string, string>() { { "login", login } };
            Dictionary<string, Dictionary<string, string>> test = new Dictionary<string, Dictionary<string, string>>() {
                {"log_in",temp }
            };

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(server);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(post_str);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var serializer = new JavaScriptSerializer();
                var result = streamReader.ReadToEnd();
                string json = serializer.Deserialize<string>(result);
                acc.error = "";
                if (json == "\"OK\"")
                {
                    
                    acc.login = login;
                    string post_str2 = "{ \"get_dbs\":{\"login\":\"" + login + "\" }}";


                    var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(server);
                    httpWebRequest2.ContentType = "application/json";
                    httpWebRequest2.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                    {
                        streamWriter.Write(post_str2);
                    }                    
                   
                    var httpResponse2 = (HttpWebResponse)httpWebRequest2.GetResponse();
                    using (var streamReader2 = new StreamReader(httpResponse2.GetResponseStream()))
                    {
                        var serializer2 = new JavaScriptSerializer();
                        var result2 = streamReader2.ReadToEnd();
                        var json2 = serializer.Deserialize<string>(result2);
                        json2 = json2.Replace("\"", "");
                        json2 = json2.Replace("[", "");
                        json2 = json2.Replace("]", "");
                        var arr2 = json2.Split(',');
                        foreach (var x in arr2)
                        {                            
                            acc.dbs.Add(x.Trim());
                        }
                        int i = 0;
                    }
                    return View("Account", acc);
                }
                else
                {
                    acc.error = json;
                    return View("Login", acc);
                }
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}