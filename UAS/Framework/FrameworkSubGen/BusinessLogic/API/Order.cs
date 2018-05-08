using System;
using System.Linq;
using System.Net;
using System.Text;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class Order
    {
        private readonly string extension = "orders/";
        //method no longer exists on SubGen side
        //public List<Entity.Order> GetOrders(Entity.Enums.Client client, int subscriber_id)
        //{
        //    if (subscriber_id > 0)
        //    {
        //        //GET https://api.knowledgemarketing.com/2/orders/
        //        Authentication auth = new Authentication();
        //        WebClient webClient = auth.GetClient(client);
        //        webClient.QueryString.Add("subscriber_id", subscriber_id.ToString()); 
        //        string json = webClient.DownloadString(auth.BaseUri + extension);
        //        Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
        //        Response.Order resp = jf.FromJson<Response.Order>(json);
        //        webClient.Dispose();
        //        return resp.orders;
        //    }
        //    else
        //        return null;
        //}
        public Entity.Order GetOrder(Entity.Enums.Client client, int order_id)
        {
            if (order_id > 0)
            {
                Entity.Order resp = new Entity.Order();
                try
                {
                    //GET https://api.knowledgemarketing.com/2/orders/{order_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    //webClient.QueryString.Add("order_id", order_id.ToString());
                    string json = webClient.DownloadString(auth.BaseUri + extension + order_id.ToString());
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    if (!string.IsNullOrEmpty(json) && json != "null")
                        resp = jf.FromJson<Entity.Order>(json);
                    webClient.Dispose();
                }
                catch (Exception ex)
                {
                    Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
                return resp;
            }
            else
                return null;
        }
        public int CreateOrder(Entity.Enums.Client client, Entity.Order order)
        {
            if (order.billing_address_id > 0 && order.mailing_address_id > 0 && order.subscriber_id > 0 && order.order_items.Count > 0)
            {
                int item = 0;
                try
                {
                    //POST https://api.knowledgemarketing.com/2/orders/
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("billing_address_id", order.billing_address_id.ToString());
                    reqparm.Add("channel_id", order.channel_id.ToString());
                    reqparm.Add("grand_total", order.grand_total.ToString());
                    reqparm.Add("import_name", order.import_name);
                    reqparm.Add("is_gift", order.is_gift.ToString());
                    reqparm.Add("mailing_address_id", order.mailing_address_id.ToString());
                    reqparm.Add("order_date", order.order_date.ToShortDateString());
                    reqparm.Add("order_id", order.order_id.ToString());
                    foreach (Entity.OrderItem oi in order.order_items)
                    {
                        reqparm.Add("bundle_id", oi.bundle_id.ToString());
                        reqparm.Add("fulfilled_date", oi.fulfilled_date.ToString());
                        reqparm.Add("grand_total", oi.grand_total.ToString());
                        reqparm.Add("order_item_id", oi.order_item_id.ToString());
                        reqparm.Add("refund_date", oi.refund_date.ToString());
                        reqparm.Add("sub_total", oi.sub_total.ToString());
                        reqparm.Add("tax_total", oi.tax_total.ToString());
                    }
                    reqparm.Add("sub_total", order.sub_total.ToString());
                    reqparm.Add("subscriber_id", order.subscriber_id.ToString());
                    reqparm.Add("tax_total", order.tax_total.ToString());
                    reqparm.Add("user_id", order.user_id.ToString());

                    byte[] responsebytes = webClient.UploadValues(auth.BaseUri + extension, reqparm);
                    if (responsebytes != null)
                    {
                        Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
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
        public int CreateOrder(Entity.Enums.Client client, Object.CreateOrder newOrder)
        {
            int orderId = 0;
            try
            {
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string jsonOrder = jf.ToJson<Object.CreateOrder>(newOrder);
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                string url = auth.BaseUri;//auth.BaseUri   "https://api.subscriptiongenius.com/2/kmtest.php/"
                string[] json = webClient.UploadString(url + extension, "POST", jsonOrder).Split(':');
                if (json != null && json.Count() > 1)
                    orderId = jf.FromJson<int>(json[1].TrimEnd('}'));
                webClient.Dispose();

                //System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();
                //nvc.Add("billing_address_id", newOrder.billing_address_id.ToString());
                //nvc.Add("mailing_address_id", newOrder.mailing_address_id.ToString());
                //nvc.Add("subscriber_id", newOrder.subscriber_id.ToString());
                //nvc.Add("order_date", newOrder.order_date.ToString());
                //nvc.Add("is_gift", newOrder.is_gift.ToString());
                //nvc.Add("payment[amount]", newOrder.payment.amount.ToString());
                //nvc.Add("payment[notes]", newOrder.payment.notes);
                //nvc.Add("payment[transaction_id]", newOrder.payment.transaction_id.ToString());
                //nvc.Add("payment[type]", newOrder.payment.type.ToString());

                ////string liJson = jf.ToJson<List<Object.CreateOrderLineItem>>(newOrder.line_items);
                ////nvc.Add("line_items", liJson);
                //foreach(var li in newOrder.line_items)
                //{
                //    nvc.Add("line_items[bundle_id]", li.bundle_id.ToString());
                //    nvc.Add("line_items[price]", li.price.ToString());
                //}


                //byte[] responseArray = webClient.UploadValues(auth.BaseUri + extension, "POST", nvc);
                //string[] json = Encoding.ASCII.GetString(responseArray).Split(':');
                //int.TryParse(json[1].ToString().TrimEnd('}'), out orderId);
                //webClient.Dispose();

                #region WEbRequest
                //try
                //{
                //    Authentication auth = new Authentication();
                //    WebClient webClient = auth.GetClient(client);
                //    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                //    string jsonOrder = jf.ToJson<Object.CreateOrder>(newOrder);
                //    string url = auth.BaseUri + extension;
                //    string data = jsonOrder;

                //    WebRequest myReq = WebRequest.Create(url);
                //    myReq.Method = "POST";
                //    myReq.ContentLength = data.Length;
                //    myReq.ContentType = "application/json; charset=UTF-8; ";

                //    UTF8Encoding enc = new UTF8Encoding();
                //    myReq.Headers.Add("Authorization", webClient.Headers["Authorization"].ToString());
                //    using (Stream ds = myReq.GetRequestStream())
                //    {
                //        ds.Write(enc.GetBytes(data), 0, data.Length);
                //    }

                //    WebResponse wr = myReq.GetResponse();
                //    Stream receiveStream = wr.GetResponseStream();
                //    StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                //    string content = reader.ReadToEnd();
                //}
                //catch (Exception ex)
                //{
                //    string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return orderId;

            #region other tries
            //try
            //{
            //    webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
            //    string json = webClient.UploadString(auth.BaseUri + extension, "POST", jsonOrder);
            //    int item = jf.FromJson<int>(json);
            //    webClient.Dispose();
            //    //return item;
            //}
            //catch (Exception ex)
            //{
            //    string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //}

            //try
            //{
            //    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            //    byte[] postArray = Encoding.ASCII.GetBytes(jsonOrder);
            //    byte[] responseArray = webClient.UploadData(auth.BaseUri + extension, "POST", postArray);
            //    string json = Encoding.ASCII.GetString(responseArray);
            //    int item = jf.FromJson<int>(json);
            //    webClient.Dispose();
            //    //return item;
            //}
            //catch (Exception ex)
            //{
            //    string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //}

            //var baseAddress = "https://api.subscriptiongenius.com/2/" + extension;
            //Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            //string jsonOrder = jf.ToJson<Object.CreateOrder>(newOrder);
            //var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            //http.ContentType = "application/x-www-form-urlencoded";
            //http.Method = "POST";
            //http.Headers.Add("Authorization", "Basic OGUwYjkwN2E5NWJjOTFkYTY1OGVjOGQyODY4MTU4Njc6ZGFlMjk1NTg0MDFiZTNkYjhlNWJjMjhmNjhjZGQ0NjI=");

            //try
            //{
            //    string parsedContent = jsonOrder;
            //    ASCIIEncoding encoding = new ASCIIEncoding();
            //    Byte[] bytes = encoding.GetBytes(parsedContent);
            //    Stream newStream = http.GetRequestStream();
            //    newStream.Write(bytes, 0, bytes.Length);
            //    newStream.Close();
            //    var response = http.GetResponse();
            //    var stream = response.GetResponseStream();
            //    var sr = new StreamReader(stream);
            //    var content = sr.ReadToEnd();
            //}
            //catch (Exception ex)
            //{
            //    string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //}


            //try
            //{
            //    var json = baseAddress.PostStringToUrl(jsonOrder, "application/json", "*/*", responseFilter: httpRes =>
            //    {
            //        httpRes.Headers.Add(HttpRequestHeader.Authorization, webClient.Headers["Authorization"].ToString());
            //    });
            //}
            //catch (Exception ex)
            //{
            //    string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //}

            //try
            //{
            //    var json = baseAddress.PostJsonToUrl(newOrder,responseFilter: httpRes =>
            //    {
            //        httpRes.Headers.Add(HttpRequestHeader.Authorization, webClient.Headers["Authorization"].ToString());
            //    });
            //}
            //catch (Exception ex)
            //{
            //    string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //}

            //try
            //{
            //    var json = baseAddress.PostToUrl(newOrder, responseFilter: httpRes =>
            //    {
            //        httpRes.Headers.Add(HttpRequestHeader.Authorization, webClient.Headers["Authorization"].ToString());
            //    });
            //}
            //catch (Exception ex)
            //{
            //    string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
            //}


            //if (newOrder.line_items.First() != null && newOrder.payment != null)
            //{
            //    Authentication auth = new Authentication();
            //    WebClient webClient = auth.GetClient(client);
            //    System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
            //    reqparm.Add("billing_address_id", newOrder.billing_address_id.ToString());
            //    reqparm.Add("mailing_address_id", newOrder.mailing_address_id.ToString());
            //    reqparm.Add("subscriber_id", newOrder.subscriber_id.ToString());
            //    reqparm.Add("is_gift", newOrder.is_gift.ToString());
            //    reqparm.Add("order_date", newOrder.order_date.ToString());

            //    reqparm.Add("bundle_id", newOrder.line_items.First().bundle_id.ToString());//line_items.
            //    reqparm.Add("price", newOrder.line_items.First().price.ToString());//line_items.
            //    reqparm.Add("amount", newOrder.payment.amount.ToString());//payment.
            //    reqparm.Add("notes", newOrder.payment.notes.ToString());//payment.
            //    reqparm.Add("transaction_id", newOrder.payment.transaction_id.ToString());//payment.
            //    reqparm.Add("type", newOrder.payment.type.ToString());//payment.

            //    byte[] responsebytes = webClient.UploadValues(auth.BaseUri + extension,"POST", reqparm);
            //    string[] json = Encoding.UTF8.GetString(responsebytes).Split(':');
            //    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
            //    int item = jf.FromJson<int>(json[1].TrimEnd('}'));
            //    webClient.Dispose();
            //    return item;
            //}
            //else
            //    return 0;
            #endregion
        }

        public Entity.OrderTotal Calculate(Entity.Enums.Client client, int line_items_bundle_id = 0, double line_items_price = 0, int line_items_product_id = 0, string mailing_address_address_line_1 = "",
            string mailing_address_city = "", string mailing_address_country = "", string mailing_address_postal_code = "", string mailing_address_state = "", int mailing_address_id = 0)
        {
            Entity.OrderTotal resp = new Entity.OrderTotal();
            try
            {
                //POST https://api.knowledgemarketing.com/2/orders/calculate
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();

                if (line_items_bundle_id > 0)
                    reqparm.Add("line_items.bundle_id", line_items_bundle_id.ToString());
                if (line_items_price > 0)
                    reqparm.Add("line_items.price", line_items_price.ToString());
                if (line_items_product_id > 0)
                    reqparm.Add("line_items.product_id", line_items_product_id.ToString());
                if (!string.IsNullOrEmpty(mailing_address_address_line_1))
                    reqparm.Add("mailing_address.address_line_1", mailing_address_address_line_1.ToString());
                if (!string.IsNullOrEmpty(mailing_address_city))
                    reqparm.Add("mailing_address.city", mailing_address_city.ToString());
                if (!string.IsNullOrEmpty(mailing_address_country))
                    reqparm.Add("mailing_address.country", mailing_address_country.ToString());
                if (!string.IsNullOrEmpty(mailing_address_postal_code))
                    reqparm.Add("mailing_address.postal_code", mailing_address_postal_code.ToString());
                if (!string.IsNullOrEmpty(mailing_address_state))
                    reqparm.Add("mailing_address.state", mailing_address_state.ToString());
                if (mailing_address_id > 0)
                    reqparm.Add("mailing_address_id", mailing_address_id.ToString());

                byte[] responsebytes = webClient.UploadValues(auth.BaseUri + extension + "calculate/", Entity.Enums.HttpMethod.POST.ToString(), reqparm);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                if (responsebytes != null)
                    resp = jf.FromJson<Entity.OrderTotal>(Encoding.UTF8.GetString(responsebytes));
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return resp;
        }
    }
}
