using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FrameworkSubGen.BusinessLogic.API
{
    public class Publication
    {
        private readonly string extension = "publications/";
        public List<Entity.Publication> GetPublications(Entity.Enums.Client client)
        {
            List<Entity.Publication> retList = new List<Entity.Publication>();
            try
            {
                //GET https://api.knowledgemarketing.com/2/publications/
                Authentication auth = new Authentication();
                WebClient webClient = auth.GetClient(client);
                string json = webClient.DownloadString(auth.BaseUri + extension);
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                //Response.Publication resp = jf.FromJson<Response.Publication>(json);
                if (!string.IsNullOrEmpty(json) && json != "null")
                    retList = jf.FromJson<List<Entity.Publication>>(json);
                webClient.Dispose();
            }
            catch (Exception ex)
            {
                Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return retList;// resp.publications;
        }
        public Entity.Publication GetPublication(Entity.Enums.Client client, int publication_id)
        {
            if (publication_id > 0)
            {
                Entity.Publication item = new Entity.Publication();
                try
                {
                    //GET https://api.knowledgemarketing.com/2/publications/{publication_id}
                    Authentication auth = new Authentication();
                    WebClient webClient = auth.GetClient(client);
                    //webClient.QueryString.Add("publication_id", publication_id.ToString());
                    string json = webClient.DownloadString(auth.BaseUri + extension + publication_id.ToString());
                    Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                    if (!string.IsNullOrEmpty(json) && json != "null")
                        item = jf.FromJson<Entity.Publication>(json);
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

        public bool IssueClose(Entity.Enums.Client client, int publicationId)
        {
            bool success = true;
            //Close on Subgen side  **Grace is always free

            //1. Call MailingList.EstimateListSize
            //a. Pass publicationId
            //b. Pass Grace date
            //c. pass Graace issues

            //example:  call first time with 0 grace issues --> get estimated count
            //call again with 1 grace issue --> get new count - should be higher

            //2. Call GenerateList - will get a mailing ListId
            //3. Call ListDetail every minute with MailingListId until status comes back "complete"
            //4. Call SubscriptionList to get list of subscribers
            //** can take a long time depnding on spot in que (common que with all SubGen clients)


            return success;
        }
    }
}
