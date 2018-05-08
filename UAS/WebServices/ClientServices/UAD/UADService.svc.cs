using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml;
using Core_AMS.Utilities;
using FrameworkUAD.Object;
using FrameworkUAS.Service;
using KM.Common;
using Newtonsoft.Json;
using WebServiceFramework;
using AddressApiWorker = FrameworkSubGen.BusinessLogic.API.Address;
using AddressWorker = FrameworkSubGen.BusinessLogic.Address;
using DemographicDetail = FrameworkSubGen.Entity.SubscriberDemographicDetail;
using CustomFieldApiWorker = FrameworkSubGen.BusinessLogic.API.CustomField;
using CustomFieldWorker = FrameworkSubGen.BusinessLogic.CustomField;
using EntityAddress = FrameworkSubGen.Entity.Address;
using EntityCustomField = FrameworkSubGen.Entity.CustomField;
using EntityFieldUpdate = FrameworkSubGen.Object.SubscriberFieldUpdate;
using EntityOrder = FrameworkSubGen.Object.CreateOrder;
using EntityPaidSubscription = FrameworkUAD.Object.PaidSubscription;
using EntitySubscriberField = FrameworkSubGen.Object.SubscriberField;
using EnumsClient = FrameworkSubGen.Entity.Enums.Client;
using StringFunctions = Core_AMS.Utilities.StringFunctions;
using SubGenPayment = FrameworkSubGen.Entity.Enums.PaymentType;
using UadPayment = FrameworkUAD_Lookup.Enums.PaymentType;

namespace ClientServices.UAD
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [SoapDocumentService(ParameterStyle = SoapParameterStyle.Bare)]
    //[ConditionalDataBehavior]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UADService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UADService.svc or UADService.svc.cs at the Solution Explorer and start debugging.
    public class UADService : ServiceBase, IUADService
    {
        private const string NamePaidSubscription = "PaidSubscription";
        private const string MethodSavePaidSubscriber = "SavePaidSubscriber";
        private const string DateFormat = "yyyy-MM-dd";
        private const string NameObjectCustomField = "Object.CustomField";
        private const string MethodGetCustomFields = "GetCustomFields";
        private const string MethodGetCustomFieldValues = "GetCustomFieldValues";
        private const string MethodGetConsensusCustomFieldValues = "GetConsensusCustomFieldValues";
        private const string NameObjectGetConsensusCustomFields = "Object.GetConsensusCustomFields";
        private const string MethodGetConsensusCustomFields = "GetConsensusCustomFields";

        System.IO.StreamWriter logWriter;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="newSubscriber"></param>
        /// <returns></returns>
        public Response<FrameworkUAD.ServiceResponse.SavedSubscriber> SaveSubscriber(Guid accessKey, FrameworkUAD.Object.SaveSubscriber newSubscriber)
        {
            //string test = RequestBody();
            Response<FrameworkUAD.ServiceResponse.SavedSubscriber> response = new Response<FrameworkUAD.ServiceResponse.SavedSubscriber>();

            string test;
            try
            {
                Core_AMS.Utilities.JsonFunctions jf = new Core_AMS.Utilities.JsonFunctions();
                string param = jf.ToJson<FrameworkUAD.Object.SaveSubscriber>(newSubscriber);
                try
                {
                    test = OperationContext.Current.RequestContext.RequestMessage.ToString();
                }
                catch (Exception ex)
                {
                    response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    LogError(accessKey, ex, this.GetType().Name.ToString());
                    response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
                    test = "";
                }
                // string messagetext;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscription", "SaveSubscriber");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    //FrameworkUAD.BusinessLogic.Subscription worker = new FrameworkUAD.BusinessLogic.Subscription();
                    response.Message = "AccessKey Validated";
                    ADMS.Services.Validator.Validator val = new ADMS.Services.Validator.Validator();
                    response.Result = val.SaveSubscriber(auth.Client, newSubscriber);

                    if (response.Result != null)
                    {
                        response.ProcessCode = response.Result.ProcessCode;
                        if (response.Result.IsPubCodeValid == false || response.Result.IsCodeSheetValid == false)
                        {
                            FileLog(response.Message, response.ProcessCode);
                            response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                            response.Message = "PubCode or CodeSheet not valid - LogMessage: " + response.Result.LogMessage + " - PubCodeMsg: " + response.Result.PubCodeMessage + " - CodeSheetMsg: " + response.Result.CodeSheetMessage + " - SubscriberProductMessage: " + response.Result.SubscriberProductMessage;
                        }
                        else if (response.Result.IsProductSubscriberCreated == false)
                        {
                            FileLog(response.Message, response.ProcessCode);
                            response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                            response.Message = "Your request was unsuccessful due to a formatting issue in your object. Please review your data and resubmit";
                            response.Result.CodeSheetMessage = "Your request was unsuccessful due to a formatting issue in your object. Please review your data and resubmit";
                            response.Result.IsCodeSheetValid = false;
                            response.Result.IsPubCodeValid = false;
                        }
                        else
                        {
                            response.Message = "Success";
                            response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                        }
                    }
                    else
                    {
                        FileLog(response.Message, response.ProcessCode);
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
                else
                    response.Message = "AccessKey Invalid";

                string clientList = System.Configuration.ConfigurationManager.AppSettings["logClientList"].ToString();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(test);
                //acesskey, processcode, request, response, errormessage, methodname, linebreak \\newtonsoft 
                if (clientList.Contains(auth.Client.ClientID.ToString()) || clientList == "All")
                {
                    WriteToLog("AccessKey:" + accessKey + "|||ProcessCode:" + response.ProcessCode + "|||RawXML:" + test + "|||Request:" + JsonConvert.SerializeXmlNode(doc) + "|||KMObject:" + param + "|||Response:" + JsonConvert.SerializeObject(response) + "|||ErrorMessage:No Error", auth.Client.FtpFolder);
                }

            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<List<FrameworkUAD.Object.Subscription>> GetSubscriber(Guid accessKey, string emailAddress)
        {
            Response<List<FrameworkUAD.Object.Subscription>> response = new Response<List<FrameworkUAD.Object.Subscription>>();
            try
            {
                string param = "EmailAddress:" + emailAddress;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "Subscription", "GetSubscriber");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.ClientSubscription worker = new FrameworkUAD.BusinessLogic.ClientSubscription();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.Select(emailAddress, auth.Client.ClientConnections, auth.Client.DisplayName, true).ToList();
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        FileLog(response.Message, response.ProcessCode);
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
                else
                    response.Message = "AccessKey Invalid";
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<List<FrameworkUAD.Object.SubscriberConsensus>> GetDemographics(Guid accessKey, string emailAddress, List<FrameworkUAD.Object.SubscriberConsensusDemographic> dimensions = null)
        {
            Response<List<FrameworkUAD.Object.SubscriberConsensus>> response = new Response<List<FrameworkUAD.Object.SubscriberConsensus>>();
            try
            {
                string param = "EmailAddress:" + emailAddress;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberConsensus", "GetDemographics");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Objects worker = new FrameworkUAD.BusinessLogic.Objects();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetDemographics(auth.Client.ClientConnections, emailAddress, dimensions).ToList();
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        FileLog(response.Message, response.ProcessCode);
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
                else
                    response.Message = "AccessKey Invalid";
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }
        public Response<List<FrameworkUAD.Object.SubscriberProduct>> GetProductDemographics(Guid accessKey, string emailAddress, string productCode, List<FrameworkUAD.Object.SubscriberProductDemographic> dimensions = null)
        {
            Response<List<FrameworkUAD.Object.SubscriberProduct>> response = new Response<List<FrameworkUAD.Object.SubscriberProduct>>();
            try
            {
                string param = "EmailAddress:" + emailAddress + " ProductCode:" + productCode;
                FrameworkUAS.Service.Authentication auth = Authenticate(accessKey, false, param, "SubscriberProduct", "GetProductDemographics");
                response.Status = auth.Status;

                if (auth.IsAuthenticated == true)
                {
                    FrameworkUAD.BusinessLogic.Objects worker = new FrameworkUAD.BusinessLogic.Objects();
                    response.Message = "AccessKey Validated";
                    response.Result = worker.GetProductDemographics(auth.Client.ClientConnections, emailAddress, productCode, dimensions).ToList();
                    if (response.Result != null)
                    {
                        response.Message = "Success";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success;
                    }
                    else
                    {
                        FileLog(response.Message, response.ProcessCode);
                        response.Message = "Error";
                        response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                    }
                }
                else
                    response.Message = "AccessKey Invalid";
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, this.GetType().Name.ToString());
                response.Message = Core_AMS.Utilities.StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        public Response<List<CustomField>> GetCustomFields(Guid accessKey, string productCode = "")
        {
            var param = $"ProductCode:{productCode}";
            var model = new ServiceRequestModel<List<CustomField>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = NameObjectCustomField,
                AuthenticateMethod = MethodGetCustomFields,
                WorkerFunc = request => new FrameworkUAD.BusinessLogic.Objects().GetCustomFields(request.ClientConnection, productCode)
            };

            return GetResponse(model);
        }

        public Response<List<CustomField>> GetConsensusCustomFields(Guid accessKey)
        {
            var model = new ServiceRequestModel<List<CustomField>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = string.Empty,
                AuthenticateEntity = NameObjectGetConsensusCustomFields,
                AuthenticateMethod = MethodGetConsensusCustomFields,
                WorkerFunc = request => new FrameworkUAD.BusinessLogic.Objects().GetConsensusCustomFields(request.ClientConnection)
            };

            return GetResponse(model);
        }

        public Response<List<CustomFieldValue>> GetCustomFieldValues(Guid accessKey, string productCode = "", string customFieldName = "")
        {
            var param = $"ProductCode:{productCode} CustomFieldName:{customFieldName}";
            var model = new ServiceRequestModel<List<CustomFieldValue>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = NameObjectCustomField,
                AuthenticateMethod = MethodGetCustomFieldValues,
                WorkerFunc = request => new FrameworkUAD.BusinessLogic.Objects()
                    .GetCustomFieldValues(request.ClientConnection, productCode, customFieldName)
            };

            return GetResponse(model);
        }

        public Response<List<CustomFieldValue>> GetConsensusCustomFieldValues(Guid accessKey, string customFieldName = "")
        {
            var param = $"CustomFieldName:{customFieldName}";
            var model = new ServiceRequestModel<List<CustomFieldValue>>
            {
                AccessKey = accessKey,
                AuthenticateRequestData = param,
                AuthenticateEntity = NameObjectCustomField,
                AuthenticateMethod = MethodGetConsensusCustomFieldValues,
                WorkerFunc = request => new FrameworkUAD.BusinessLogic.Objects()
                    .GetConsensusCustomFieldValues(request.ClientConnection, customFieldName)
            };

            return GetResponse(model);
        }

        void WriteToLog(string msg,string clientName)
        {

            //acesskey, processcode, request, response, errormessage, methodname, linebreak
            string logPath = System.Configuration.ConfigurationManager.AppSettings["Log"].ToString();
            if (!System.IO.Directory.Exists(logPath))
                System.IO.Directory.CreateDirectory(logPath);
            logWriter = new System.IO.StreamWriter(logPath + clientName +"_UASWS_" + DateTime.Now.ToString("MMddyyyy") + ".txt", true);
            WriteToFile(msg, logWriter);
        }
        void WriteToFile(string text, System.IO.StreamWriter WriteFile)
        {
            try
            {
                WriteFile.AutoFlush = true;
                WriteFile.WriteLine(text);
                WriteFile.WriteLine("");
                WriteFile.Flush();
                WriteFile.Close();
                System.GC.Collect();
                
            }
            catch { }
        }
        public static string RequestBody()
        {
            var bodyStream = new System.IO.StreamReader(HttpContext.Current.Request.InputStream);
            bodyStream.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();
            return bodyText;
            
        }


        #region Paid methods
        public Response<int> SavePaidSubscriber(Guid accessKey, EntityPaidSubscription newPaidSubscription)
        {
            //per Joel 3/10/16 - call Create Order method - this will automatically create a subscription
            //returns order_id int

            var response = new Response<int>();
            try
            {
                var param = new JsonFunctions().ToJson(newPaidSubscription);
                var auth = Authenticate(accessKey, false, param, NamePaidSubscription, MethodSavePaidSubscriber);
                response.Status = auth.Status;
                response.ProcessCode = StringFunctions.GenerateProcessCode();

                if (auth.IsAuthenticated)
                {
                    response.Message = "AccessKey Validated";
                    ProcessSavePaidSubscriber(accessKey, auth, response, newPaidSubscription);
                }
                else
                {
                    response.Message = "AccessKey Invalid";
                }

                auth.LogEntry.ResponseData = response.Result.ToString();
                SaveLog(auth.LogEntry);
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                LogError(accessKey, ex, GetType().Name);
                response.Message = StringFunctions.FriendlyServiceError();
            }
            return response;
        }

        private void ProcessSavePaidSubscriber(Guid accessKey, Authentication auth, Response<int> response, EntityPaidSubscription paidSubscription)
        {
            Guard.NotNull(auth, nameof(auth));
            Guard.NotNull(response, nameof(response));
            Guard.NotNull(paidSubscription, nameof(paidSubscription));

            //1 - try to get subscriber from KM SugGen db - no match - create on SG side
            //2 - try to get billing from KM SugGen db - no match - create on SG side
            //3 - try to get mailing from KM SugGen db - no match - create on SG side
            //--------------this is only for T-shirts or those type things 4 - get Product by name from KM SugGen db
            //4 - get Bundle by name from KM SugGen db
            //5 - call CreateOrder SubGen method
            //6 - after Order created then save CustomFields

            var sgClient = FrameworkSubGen.Entity.Enums.GetClient(auth.Client.DisplayName);
            var kmClient = FrameworkUAS.BusinessLogic.Enums.GetClient(auth.Client.ClientName);
            var account = new FrameworkSubGen.BusinessLogic.Account().Select(kmClient);

            if (account == null)
            {
                return;
            }

            try
            {
                var billingAddressId = 0;
                var mailingAddressId = 0;
                bool isNewSubscriber;
                var subscriberId = GetSubscriberId(response, paidSubscription, sgClient, account.account_id, out isNewSubscriber);

                SaveAddresses(
                    response.ProcessCode,
                    paidSubscription,
                    sgClient,
                    subscriberId,
                    account.account_id,
                    isNewSubscriber,
                    out billingAddressId,
                    out mailingAddressId);

                var newOrder = new EntityOrder();

                LoadBundleItems(newOrder, paidSubscription, account.account_id, response.ProcessCode);
                var subGenOrderId = SaveOrder(
                    newOrder,
                    paidSubscription,
                    account.account_id,
                    subscriberId,
                    billingAddressId,
                    mailingAddressId,
                    sgClient,
                    response.ProcessCode);
                response.Result = subGenOrderId;

                if (subGenOrderId > 0)
                {
                    SaveCustomFields(accessKey, response, paidSubscription, sgClient, account.account_id, subscriberId);
                }
            }
            catch (Exception ex)
            {
                response.Status = FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Error;
                response.Message = StringFunctions.FormatException(ex);
                LogError(accessKey, ex, GetType().Name);
            }
        }

        private int GetSubscriberId(
            Response<int> response,
            EntityPaidSubscription paidSubscription,
            EnumsClient sgClient,
            int accountId,
            out bool isNewSubscriber)
        {
            Guard.NotNull(response, nameof(response));
            Guard.NotNull(paidSubscription, nameof(paidSubscription));

            // 1 - try to get subscriber from KM SugGen db - no match - create on SG side
            var subscriberId = 0;
            isNewSubscriber = true;
            var kmSubWorker = new FrameworkSubGen.BusinessLogic.Subscriber();
            var subs = kmSubWorker.FindSubscribers(paidSubscription.SubscriberEmail, paidSubscription.SubscriberFirstName, paidSubscription.SubscriberLastName);
            FileLog($"SubsCount:{subs.Count}", response.ProcessCode);

            if (subs.Any())
            {
                subscriberId = subs.First().subscriber_id;
                isNewSubscriber = false;
            }
            else
            {
                var sgWorker = new FrameworkSubGen.BusinessLogic.API.Subscriber();
                subscriberId = sgWorker.CreateSubscriber(
                    sgClient,
                    paidSubscription.SubscriberFirstName,
                    paidSubscription.SubscriberLastName,
                    paidSubscription.SubscriberEmail,
                    string.Empty,
                    paidSubscription.Source);
                FileLog($"SubGen new SubscriberId:{subscriberId}", response.ProcessCode);

                //lets save our new Subscriber in db
                var newSub = new FrameworkSubGen.Entity.Subscriber
                {
                    account_id = accountId,
                    create_date = DateTime.Now,
                    email = paidSubscription.SubscriberEmail,
                    first_name = paidSubscription.SubscriberFirstName,
                    last_name = paidSubscription.SubscriberLastName,
                    source = paidSubscription.Source,
                    subscriber_id = subscriberId
                };

                var subWorker = new FrameworkSubGen.BusinessLogic.Subscriber();
                subWorker.Save(newSub);
                FileLog($"Saved SubscriberId:{subscriberId}", response.ProcessCode);
            }

            return subscriberId;
        }

        private void SaveAddresses(
            string processCode,
            EntityPaidSubscription paidSubscription,
            EnumsClient sgClient,
            int subscriberId,
            int accountId,
            bool isNewSubscriber,
            out int billingAddressId,
            out int mailingAddressId)
        {
            Guard.NotNull(paidSubscription, nameof(paidSubscription));

            // 2 and 3 - try to get billing/mailing from KM SugGen db - no match - create on SG side
            var mail = GetMailingAddress(paidSubscription, subscriberId, accountId);
            var bill = GetBillingAddress(paidSubscription, subscriberId, accountId);

            if (isNewSubscriber)
            {
                CreateAndSaveAddresses(mail, bill, sgClient, processCode);
            }
            else
            {
                //pull by subscriberId
                FileLog($"Get Address SubscriberId:{subscriberId}", processCode);
                var addresses = new AddressWorker().Select(subscriberId) ?? new List<EntityAddress>();
                FileLog($"Addresses Count:{addresses.Count}", processCode);

                if (addresses.Any())
                {
                    //find to match billing/mailing - if not found create
                    FileLog($"Found address count:{addresses.Count}", processCode);
                    mail.address_id = FindOrCreateAddressId(addresses, mail, null, sgClient);
                    bill.address_id = FindOrCreateAddressId(addresses, bill, mail, sgClient);
                }
                else
                {
                    CreateAndSaveAddresses(mail, bill, sgClient, processCode);
                }
            }

            billingAddressId = bill.address_id;
            mailingAddressId = mail.address_id;
        }

        private void LoadBundleItems(EntityOrder order, EntityPaidSubscription paidSubscription, int accountId, string processCode)
        {
            Guard.NotNull(order, nameof(order));
            Guard.NotNull(paidSubscription, nameof(paidSubscription));

            // 4 - get Bundle by name from KM SugGen db - set CreateOrder line_items
            var worker = new FrameworkSubGen.BusinessLogic.Bundle();
            foreach (var item in paidSubscription.BundleItems)
            {
                var bundle = worker.Select(item.BundleName, accountId);
                if (bundle != null)
                {
                    order.line_items.Add(new FrameworkSubGen.Object.CreateOrderLineItem(bundle.bundle_id, item.Price));
                    FileLog($"BundleId:{bundle.bundle_id}", processCode);
                }
                else
                {
                    FileLog($"Bundle is null:{item.BundleName}", processCode);
                }
            }
        }

        private int SaveOrder(
            EntityOrder newOrder,
            EntityPaidSubscription newPaidSubscription,
            int accountId,
            int subscriberId,
            int billingAddressId,
            int mailingAddressId,
            EnumsClient clientType,
            string processCode)
        {
            Guard.NotNull(newOrder, nameof(newOrder));
            Guard.NotNull(newPaidSubscription, nameof(newPaidSubscription));

            // 5 - call CreateOrder SubGen method
            newOrder.billing_address_id = billingAddressId;
            newOrder.is_gift = newPaidSubscription.IsGift;
            newOrder.mailing_address_id = mailingAddressId;
            newOrder.order_date = newPaidSubscription.OrderDate.ToString(DateFormat);
            var newPayment = new FrameworkSubGen.Entity.Payment
            {
                account_id = accountId,
                amount = newPaidSubscription.Payment.Amount,
                notes = newPaidSubscription.Payment.Note,
                transaction_id = newPaidSubscription.Payment.TransactionId
            };
            newPayment.type = ConvertPaymentType(newPaidSubscription.Payment.PaymentType, newPayment.type);
            newOrder.payment = newPayment;
            newOrder.subscriber_id = subscriberId;

            var orderWorker = new FrameworkSubGen.BusinessLogic.API.Order();
            var subGenOrderId = orderWorker.CreateOrder(clientType, newOrder);
            FileLog($"SubGenOrderId:{subGenOrderId}", processCode);

            // Save Paid info for sending to UAD during CDC
            newPayment.order_id = subGenOrderId;
            newPayment.subscriber_id = subscriberId;
            newPayment.date_created = DateTime.Now;
            if (newOrder.line_items?.Any() == true)
            {
                newPayment.bundle_id = newOrder.line_items.First().bundle_id;
            }

            var paymentWorker = new FrameworkSubGen.BusinessLogic.Payment();
            var payList = new List<FrameworkSubGen.Entity.Payment> { newPayment };
            paymentWorker.SaveBulkXml(payList);

            return subGenOrderId;
        }

        private void SaveCustomFields(
            Guid accessKey,
            Response<int> response,
            EntityPaidSubscription paidSubscription,
            EnumsClient sgClient,
            int accountId,
            int subscriberId)
        {
            Guard.NotNull(response, nameof(response));
            Guard.NotNull(paidSubscription, nameof(paidSubscription));

            // 6 - order placed - now save CustomFields.
            var apiWorker = new CustomFieldApiWorker();
            var worker = new CustomFieldWorker();
            var fieldUpdate = new EntityFieldUpdate();
            var fieldApiList = new List<EntityCustomField>();
            var fieldList = worker.Select(accountId);

            fieldUpdate.subscriber_id = subscriberId;
            FileLog($"Start save Demos - count:{paidSubscription.ProductDemographics.Count}", response.ProcessCode);
            // create object for saving to SG
            foreach (var demographic in paidSubscription.ProductDemographics.Where(p => !string.IsNullOrWhiteSpace(p.ProductCode)))
            {
                try
                {
                    var sgDemo = $"{demographic.ProductCode.ToUpper().Trim()} - {demographic.DemographicName.Trim()}";
                    var subscriberField = new EntitySubscriberField();

                    //x.DemographicName; - need to get the field_id
                    var customField = GetCustomFieldOrDefault(fieldList, fieldApiList, apiWorker, sgClient, sgDemo, demographic.DemographicName.Trim());
                    if (customField != null)
                    {
                        subscriberField.field_id = customField.field_id;
                    }

                    if (subscriberField.field_id > 0 && customField != null)
                    {
                        SetSubscriberFieldTextValue(response, demographic, fieldUpdate, subscriberField, customField);
                    }
                    else
                    {
                        //notify that the Demo passed in does not exist
                        response.Message += $"The demographic field {demographic.DemographicName} does not exist and has been excluded.";
                    }
                }
                catch (Exception ex)
                {
                    LogError(accessKey, ex, GetType().Name, "SavePaidSubscriber: 6 - order placed - now save CustomFields");
                }
            }

            //save
            //need to make sure this is creating/updating demos in SubGen - sent question to Joel
            apiWorker.SubscriberFieldUpdate(sgClient, fieldUpdate);
            SaveSubscriberDemographic(accessKey, fieldUpdate, accountId, fieldList, fieldApiList);
        }

        private void SetSubscriberFieldTextValue(
            Response<int> response,
            PaidSubscriptionProductDemographic demographic,
            EntityFieldUpdate fieldUpdate,
            EntitySubscriberField subscriberField,
            EntityCustomField customField)
        {
            Guard.NotNull(response, nameof(response));
            Guard.NotNull(demographic, nameof(demographic));
            Guard.NotNull(fieldUpdate, nameof(fieldUpdate));
            Guard.NotNull(subscriberField, nameof(subscriberField));
            Guard.NotNull(customField, nameof(customField));

            if (!string.IsNullOrWhiteSpace(demographic.OtherTextValue))
            {
                subscriberField.text_value = demographic.OtherTextValue;
            }
            else
            {
                foreach (var name in demographic.Values)
                {
                    //v.ToString() = values coming in - get optionId from value
                    var valueOption = customField.value_options.FirstOrDefault(x =>
                        x.value.Trim().Equals(name.Trim(), StringComparison.CurrentCultureIgnoreCase));
                    if (valueOption != null)
                    {
                        subscriberField.option_ids.Add(valueOption.option_id);
                    }
                    else
                    {
                        response.Message += $"The demographic field {demographic.DemographicName} value - {name} - does not exist and has been excluded.";
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(subscriberField.text_value) || subscriberField.option_ids.Count > 0)
            {
                fieldUpdate.fields.Add(subscriberField);
            }
            else
            {
                response.Message += $"The demographic field {demographic.DemographicName} had no matching configured values and has been excluded.";
            }
        }

        private static EntityCustomField GetCustomFieldOrDefault(IList<EntityCustomField> fieldList, string fullName, string name)
        {
            Guard.NotNull(fieldList, nameof(fieldList));

            return fieldList.FirstOrDefault(x => x.name.Trim().Equals(fullName, StringComparison.CurrentCultureIgnoreCase)) ??
                   fieldList.FirstOrDefault(x => x.name.Trim().Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        private static EntityCustomField GetCustomFieldOrDefault(
            IList<EntityCustomField> fieldList,
            List<EntityCustomField> fieldApiList,
            CustomFieldApiWorker apiWorker,
            EnumsClient clientType,
            string fullName,
            string name)
        {
            Guard.NotNull(fieldList, nameof(fieldList));
            Guard.NotNull(fieldApiList, nameof(fieldApiList));
            Guard.NotNull(apiWorker, nameof(apiWorker));

            //first try to get from SubGenDB if not there try via api
            var customField = GetCustomFieldOrDefault(fieldList, fullName, name);
            if (customField == null)
            {
                //call SG API
                if (fieldApiList.Count == 0)
                {
                    fieldApiList.AddRange(apiWorker.GetCustomFields(clientType));
                }

                customField = GetCustomFieldOrDefault(fieldApiList, fullName, name);
            }

            return customField;
        }

        private void SaveSubscriberDemographic(
            Guid accessKey,
            EntityFieldUpdate fieldUpdate,
            int accountId,
            IList<EntityCustomField> fieldList,
            IList<EntityCustomField> fieldApiList)
        {
            Guard.NotNull(fieldUpdate, nameof(fieldUpdate));
            Guard.NotNull(fieldList, nameof(fieldList));
            Guard.NotNull(fieldApiList, nameof(fieldApiList));

            var list = new List<FrameworkSubGen.Entity.SubscriberDemographic>();
            var detailList = new List<DemographicDetail>();
            foreach (var field in fieldUpdate.fields)
            {
                try
                {
                    var entity = new FrameworkSubGen.Entity.SubscriberDemographic
                    {
                        account_id = accountId,
                        DateCreated = DateTime.Now,
                        field_id = field.field_id,
                        IsProcessed = false,
                        subscriber_id = fieldUpdate.subscriber_id,
                        text_value = field.text_value
                    };
                    list.Add(entity);
                    var details = GetSubscriberDemographicDetails(field, fieldUpdate, fieldList, fieldApiList, accountId);
                    detailList.AddRange(details);
                }
                catch (Exception ex)
                {
                    LogError(accessKey, ex, GetType().Name);
                }
            }

            try
            {
                var sdWorker = new FrameworkSubGen.BusinessLogic.SubscriberDemographic();
                var sddWorker = new FrameworkSubGen.BusinessLogic.SubscriberDemographicDetail();
                if (list.Any())
                {
                    sdWorker.Save(list);
                }

                if (detailList.Any())
                {
                    sddWorker.Save(detailList);
                }
            }
            catch (Exception ex)
            {
                LogError(accessKey, ex, GetType().Name);
            }
        }

        private IEnumerable<DemographicDetail> GetSubscriberDemographicDetails(
            EntitySubscriberField subscriberField,
            EntityFieldUpdate fieldUpdate,
            IList<EntityCustomField> fieldList,
            IList<EntityCustomField> fieldApiList,
            int accountId)
        {
            Guard.NotNull(subscriberField, nameof(subscriberField));
            Guard.NotNull(fieldUpdate, nameof(fieldUpdate));
            Guard.NotNull(fieldList, nameof(fieldList));
            Guard.NotNull(fieldApiList, nameof(fieldApiList));

            foreach (var optionId in subscriberField.option_ids)
            {
                var customField = new EntityCustomField();
                if (fieldList.Any(x => x.field_id == subscriberField.field_id))
                {
                    customField = fieldList.First(x => x.field_id == subscriberField.field_id);
                }
                else if (fieldApiList.Any(x => x.field_id == subscriberField.field_id))
                {
                    customField = fieldApiList.First(x => x.field_id == subscriberField.field_id);
                }

                var detail = new DemographicDetail
                {
                    account_id = accountId,
                    DateCreated = DateTime.Now,
                    field_id = subscriberField.field_id,
                    IsProcessed = false,
                    option_id = optionId,
                    subscriber_id = fieldUpdate.subscriber_id,
                    value = customField.value_options.First(x => x.option_id == optionId).value
                };

                yield return detail;
            }
        }

        private static SubGenPayment ConvertPaymentType(UadPayment lookupType, SubGenPayment defaultType)
        {
            switch (lookupType)
            {
                case UadPayment.Cash:
                    return SubGenPayment.Cash;
                case UadPayment.Credit_Card:
                    return SubGenPayment.Credit;
                case UadPayment.PayPal:
                    return SubGenPayment.PayPal;
                case UadPayment.Check:
                    return SubGenPayment.Check;
                case UadPayment.Imported:
                    return SubGenPayment.Imported;
                case UadPayment.Other:
                    return SubGenPayment.Other;
            }

            return defaultType;
        }

        private EntityAddress GetMailingAddress(EntityPaidSubscription paidSubscription, int subscriberId, int accountId)
        {
            Guard.NotNull(paidSubscription, nameof(paidSubscription));

            return new EntityAddress
            {
                address = paidSubscription.MailingAddress,
                address_line_2 = paidSubscription.MailingAddress2,
                city = paidSubscription.MailingCity,
                state = paidSubscription.MailingState,
                subscriber_id = subscriberId,
                zip_code = paidSubscription.MailingZip,
                company = paidSubscription.MailingCompany,
                country = paidSubscription.MailingCountry,
                first_name = paidSubscription.MailingFirstName,
                last_name = paidSubscription.MailingLastName,
                account_id = accountId
            };
        }

        private EntityAddress GetBillingAddress(EntityPaidSubscription paidSubscription, int subscriberId, int accountId)
        {
            Guard.NotNull(paidSubscription, nameof(paidSubscription));

            return new EntityAddress
            {
                address = paidSubscription.BillingAddress,
                address_line_2 = paidSubscription.BillingAddress2,
                city = paidSubscription.BillingCity,
                state = paidSubscription.BillingState,
                subscriber_id = subscriberId,
                zip_code = paidSubscription.BillingZip,
                company = paidSubscription.BillingCompany,
                country = paidSubscription.BillingCountry,
                first_name = paidSubscription.BillingFirstName,
                last_name = paidSubscription.BillingLastName,
                account_id = accountId
            };
        }

        private static bool AreSame(EntityAddress first, EntityAddress second)
        {
            if (first == null || second == null)
            {
                return false;
            }

            return first == second ||
                   string.Equals(first.address, second.address, StringComparison.CurrentCultureIgnoreCase) &&
                   string.Equals(first.first_name, second.first_name, StringComparison.CurrentCultureIgnoreCase) &&
                   string.Equals(first.last_name, second.last_name, StringComparison.CurrentCultureIgnoreCase) &&
                   string.Equals(first.city, second.city, StringComparison.CurrentCultureIgnoreCase) &&
                   string.Equals(first.state, second.state, StringComparison.CurrentCultureIgnoreCase) &&
                   string.Equals(first.zip_code, second.zip_code, StringComparison.CurrentCultureIgnoreCase);
        }

        private void CreateAndSaveAddresses(
            EntityAddress mailingAddress,
            EntityAddress billingAddress,
            EnumsClient clientType,
            string processCode)
        {
            Guard.NotNull(mailingAddress, nameof(mailingAddress));
            Guard.NotNull(billingAddress, nameof(billingAddress));

            CreateAndSaveAddress(mailingAddress, clientType);
            FileLog($"Create new mailing address:{mailingAddress.address_id}", processCode);

            //check new mail against bill
            if (AreSame(mailingAddress, billingAddress))
            {
                billingAddress.address_id = mailingAddress.address_id;
                FileLog($"Use existing mailing address for billing:{billingAddress.address_id}", processCode);
            }
            else
            {
                CreateAndSaveAddress(billingAddress, clientType);
                FileLog($"Create new billing address:{billingAddress.address_id}", processCode);
            }
        }

        private int FindOrCreateAddressId(IList<EntityAddress> addresses, EntityAddress target, EntityAddress existing, EnumsClient clientType)
        {
            Guard.NotNull(addresses, nameof(addresses));
            Guard.NotNull(target, nameof(target));

            var entityOrDefault = addresses.FirstOrDefault(candidate => AreSame(candidate, target));
            if (entityOrDefault != null)
            {
                target = entityOrDefault;
            }
            else if (existing != null && AreSame(target, existing))
            {
                target.address_id = existing.address_id;
            }
            else
            {
                target.address_id = new AddressApiWorker().Create(clientType, target);
            }

            return target.address_id;
        }

        private static void CreateAndSaveAddress(EntityAddress address, EnumsClient clientType)
        {
            Guard.NotNull(address, nameof(address));

            address.address_id = new AddressApiWorker().Create(clientType, address);
            new AddressWorker().Save(address);
        }

        #endregion
    }
}
