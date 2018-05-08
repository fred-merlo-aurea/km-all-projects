using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class User
    {
        private readonly string extension = "users/";

        public List<Entity.User> GetUsers(Entity.Enums.Client client, bool is_admin, int user_id = 0, string email = "", string first_name = "", string last_name = "", string password = "")
        {
            Response.User resp = new Response.User();
            try
            {
            //GET https://api.knowledgemarketing.com/2/users/
            Authentication auth = new Authentication();
            WebClient webClient = auth.GetClient(client);
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            string json = webClient.DownloadString(auth.BaseUri + extension);
                if (!string.IsNullOrEmpty(json) && json != "null")
                    resp = jf.FromJson<Response.User>(json);
            webClient.Dispose();

            if (user_id > 0)
                    resp.users = resp.users.Where(x => x.user_id == user_id).ToList();//webClient.QueryString.Add("user_id", user_id.ToString()); 
            if (!string.IsNullOrEmpty(email))
                    resp.users = resp.users.Where(x => x.email.Equals(email, StringComparison.CurrentCultureIgnoreCase)).ToList();//webClient.QueryString.Add("email", email.ToString()); 
            if (!string.IsNullOrEmpty(first_name))
                    webClient.QueryString.Add("first_name", first_name.ToString());
            if (!string.IsNullOrEmpty(last_name))
                    webClient.QueryString.Add("last_name", last_name.ToString());
            webClient.QueryString.Add("is_admin", is_admin.ToString());
            if (!string.IsNullOrEmpty(password))
                    webClient.QueryString.Add("password", password.ToString());
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return resp.users;
        }
        public Entity.User GetUser(Entity.Enums.Client client, int user_id)
        {
            Entity.User item = new Entity.User();
            try
            {
            //GET https://api.knowledgemarketing.com/2/users/{user_id}
            Authentication auth = new Authentication();
            WebClient webClient = auth.GetClient(client);
            //webClient.QueryString.Add("user_id", user_id.ToString());
            string json = webClient.DownloadString(auth.BaseUri + extension + user_id.ToString());
            Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                if (!string.IsNullOrEmpty(json) && json != "null")
                    item = jf.FromJson<Entity.User>(json);
            webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return item;
        }
        public int Create(Entity.Enums.Client client, Entity.User user)
        {
            if (!string.IsNullOrEmpty(user.email) && !string.IsNullOrEmpty(user.first_name) && !string.IsNullOrEmpty(user.last_name) && !string.IsNullOrEmpty(user.password))
            {
                int item = 0;
                try
                {
                //POST https://api.knowledgemarketing.com/2/users/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("email", user.email);
                reqparm.Add("first_name", user.first_name);
                reqparm.Add("last_name", user.last_name);
                reqparm.Add("password", user.password);
                reqparm.Add("is_admin", user.is_admin.ToString());

                byte[] responsebytes = webClient.UploadValues(auth.BaseUri + extension, Entity.Enums.HttpMethod.POST.ToString(), reqparm);
                    if (responsebytes != null)
                    {
                string[] json = Encoding.UTF8.GetString(responsebytes).Split(':');
                        if (json != null && json.Count() > 1)
                            item = jf.FromJson<int>(json[1].TrimEnd('}'));
                    }
                webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return item;
            }
            else
                return 0;
        }
        public int Create(Entity.Enums.Client client, string email, string first_name, string last_name, bool is_admin, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(first_name) && !string.IsNullOrEmpty(last_name) && !string.IsNullOrEmpty(password))
            {
                int item = 0;
                try
                {
                //POST https://api.knowledgemarketing.com/2/users/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("email", email);
                reqparm.Add("first_name", first_name);
                reqparm.Add("last_name", last_name);
                reqparm.Add("password", password);
                reqparm.Add("is_admin", is_admin.ToString());

                byte[] responsebytes = webClient.UploadValues(auth.BaseUri + extension, Entity.Enums.HttpMethod.POST.ToString(), reqparm);
                    if (responsebytes != null)
                    {
                string[] json = Encoding.UTF8.GetString(responsebytes).Split(':');
                        if (json != null && json.Count() > 1)
                            item = jf.FromJson<int>(json[1].TrimEnd('}'));
                    }
                webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return item;
            }
            else
                return 0;
        }
        public Object.ApiLoginToken GetLoginToken(Entity.Enums.Client client, int user_id, bool read_only)
        {
            if (user_id > 0)
            {
                Object.ApiLoginToken item = new Object.ApiLoginToken();
                try
                {
                //GET https://api.knowledgemarketing.com/2/users/{user_id}/token
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    //webClient.QueryString.Add("read_only", read_only.ToString());
                    webClient.QueryString.Add("read_only", read_only ? "1" : "0");


                string json = webClient.DownloadString(auth.BaseUri + extension + user_id.ToString() + "/token/");
                    if (!string.IsNullOrEmpty(json) && json != "null")
                        item = jf.FromJson<Object.ApiLoginToken>(json);
                webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return item;
            }
            else
                return null;
        }

        public string Update(Entity.Enums.Client client, Entity.User user)
        {
            if (user.user_id > 0)
            {
                byte[] responsebytes = new byte[] { };
                try
                {
                //PUT https://api.knowledgemarketing.com/2/users/{user_id}
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("email", user.email);
                reqparm.Add("first_name", user.first_name);
                reqparm.Add("last_name", user.last_name);
                reqparm.Add("password", user.password);
                reqparm.Add("is_admin", user.is_admin.ToString());
                    responsebytes = webClient.UploadValues(auth.BaseUri + extension + user.user_id.ToString(), Entity.Enums.HttpMethod.PUT.ToString(), reqparm);
                webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return Encoding.UTF8.GetString(responsebytes);
            }
            else
                return "must supply a user_id";
        }
        public string Update(Entity.Enums.Client client, int user_id, bool is_admin, string email = "", string first_name = "", string last_name = "", string password = "")
        {
            if (user_id > 0)
            {
                byte[] responsebytes = new byte[] { };
                try
                {
                //PUT https://api.knowledgemarketing.com/2/users/{user_id}
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("email", email);
                reqparm.Add("first_name", first_name);
                reqparm.Add("last_name", last_name);
                reqparm.Add("password", password);
                reqparm.Add("is_admin", is_admin.ToString());
                    responsebytes = webClient.UploadValues(auth.BaseUri + extension + user_id.ToString(), Entity.Enums.HttpMethod.PUT.ToString(), reqparm);
                webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return Encoding.UTF8.GetString(responsebytes);
            }
            else
                return "must supply a user_id";
        }
    }
}
