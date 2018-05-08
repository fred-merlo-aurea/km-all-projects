using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FrameworkSubGen.BusinessLogic
{
    public class Payment
    {
        public bool SaveBulkXml(List<Entity.Payment> list)
        {
            foreach (Entity.Payment x in list)
                FormatData(x);

            bool done = false;
            int BatchSize = 500;
            int total = list.Count;
            int counter = 0;
            int processedCount = 0;
            int checkCount = 1;

            //batch this in 500 records
            StringBuilder sbXML = new StringBuilder();
            foreach (Entity.Payment x in list)
            {
                string msg = "Checking Payment: " + checkCount.ToString() + " of " + total.ToString();
                Core_AMS.Utilities.StringFunctions.WriteLineRepeater(msg, ConsoleColor.Green);

                checkCount++;

                string xmlObject = DataAccess.DataFunctions.CleanSerializedXML(XmlSerializer.SerializeToString<Entity.Payment>(x));
                sbXML.AppendLine(xmlObject);

                counter++;
                processedCount++;
                done = false;
                if (processedCount == total || counter == BatchSize)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            DataAccess.Payment.SaveBulkXml("<XML>" + sbXML.ToString() + "</XML>");
                            scope.Complete();
                            done = true;
                        }
                        catch (Exception ex)
                        {
                            scope.Dispose();
                            done = false;
                            API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                        }
                    }
                    sbXML = new StringBuilder();
                    counter = 0;
                }
            }
            return done;
        }
        public void FormatData(Entity.Payment x)
        {
            try
            {
                #region truncate strings
                if (x.notes != null && x.notes.Length > 15)
                    x.notes = x.notes.Substring(0, 15);
                if (x.transaction_id != null && x.transaction_id.Length > 15)
                    x.transaction_id = x.transaction_id.Substring(0, 15);
                #endregion
            }
            catch (Exception ex)
            {
                API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
        }
        public Entity.Payment Select(int subscriberId, DateTime dateCreated)
        {
            Entity.Payment retItem = null;
            retItem = DataAccess.Payment.Select(subscriberId, dateCreated);
            return retItem;
        }
        public Entity.Payment Select(Guid stRecordIdentifier)
        {
            Entity.Payment retItem = null;
            retItem = DataAccess.Payment.Select(stRecordIdentifier);
            return retItem;
        }

        public bool Update_SubscriptionId(int orderId, int subscriptionId)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    done = DataAccess.Payment.Update_SubscriptionId(orderId, subscriptionId);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    done = false;
                    API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
            }
            return done;
        }
        public bool Update_STRecordIdentifier(int orderId, Guid STRecordIdentifier)
        {
            bool done = false;
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    done = DataAccess.Payment.Update_STRecordIdentifier(orderId, STRecordIdentifier);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    done = false;
                    API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
                }
            }
            return done;
        }
        public bool Save(string processCode, KMPlatform.Object.ClientConnections client)
        {
            //here we get a processCode so pull all STRecs from SubscriberFinal - List<Guid>
            //next grab data from SubGenData.Payment by STRec - List<SubGenData.Payment>
            //foreach List<SubGenData.Payment> create UAD.SubscriptionPaid
            //Save to SubscriptionPaid
            bool success = false;
            List<FrameworkUAD.Entity.SubscriptionPaid> spList = new List<FrameworkUAD.Entity.SubscriptionPaid>();
            FrameworkUAD.BusinessLogic.SubscriptionPaid spWrk = new FrameworkUAD.BusinessLogic.SubscriptionPaid();
            FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            List<FrameworkUAD_Lookup.Entity.Code> paymentCodes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Payment);
            List<FrameworkUAD_Lookup.Entity.Code> deliveryCodes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Deliver);

            try
            {
                FrameworkUAD.BusinessLogic.SubscriberFinal sfWrk = new FrameworkUAD.BusinessLogic.SubscriberFinal();
                List<FrameworkUAD.Object.RecordIdentifier> sfList = sfWrk.SelectRecordIdentifiers(processCode, client);
                foreach (FrameworkUAD.Object.RecordIdentifier ri in sfList)
                {
                    //get our Payment object which matches STRecordIdentifier
                    if (ri.STRecordIdentifier != null)
                    {
                        Entity.Payment pay = Select(ri.STRecordIdentifier);
                        if (pay != null)
                        {
                            FrameworkUAD.Entity.SubscriptionPaid sp = new FrameworkUAD.Entity.SubscriptionPaid();
                            sp.Amount = (decimal) pay.amount;
                            sp.AmountPaid = sp.Amount;
                            sp.CreatedByUserID = 1;
                            sp.DateCreated = pay.date_created;
                            sp.PaidDate = pay.date_created;


                            int payTypeId = 0;
                            switch (pay.type)
                            {
                                case Entity.Enums.PaymentType.Cash:
                                    payTypeId = paymentCodes.SingleOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.PaymentType.Cash.ToString()).CodeId;
                                    break;
                                case Entity.Enums.PaymentType.Check:
                                    payTypeId = paymentCodes.SingleOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.PaymentType.Check.ToString()).CodeId;
                                    break;
                                case Entity.Enums.PaymentType.Credit:
                                    payTypeId = paymentCodes.SingleOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.PaymentType.Credit_Card.ToString().Replace("_", " ")).CodeId;
                                    break;
                                case Entity.Enums.PaymentType.Imported:
                                    payTypeId = paymentCodes.SingleOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.PaymentType.Imported.ToString()).CodeId;
                                    break;
                                case Entity.Enums.PaymentType.Other:
                                    payTypeId = paymentCodes.SingleOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.PaymentType.Other.ToString()).CodeId;
                                    break;
                                case Entity.Enums.PaymentType.PayPal:
                                    payTypeId = paymentCodes.SingleOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.PaymentType.PayPal.ToString()).CodeId;
                                    break;
                            }
                            sp.PaymentTypeID = payTypeId;

                            FrameworkUAD.BusinessLogic.ProductSubscription psWrk = new FrameworkUAD.BusinessLogic.ProductSubscription();
                            FrameworkUAD.Entity.ProductSubscription ps = psWrk.Select(ri.SFRecordIdentifier, client);
                            if (ps != null)
                                sp.PubSubscriptionID = ps.PubSubscriptionID;

                            Bundle bWrk = new Bundle();
                            Entity.Bundle bundle = bWrk.Select(pay.bundle_id);
                            if (bundle != null && bundle.bundle_id > 0)
                            {
                                sp.BalanceDue = (decimal) (pay.amount - bundle.price); //get Price from Bundle then calc Price - Amount to see if anything remain
                            }

                            int deliverId = 0;
                            switch (bundle.type)
                            {
                                case Entity.Enums.SubscriptionType.Both:
                                    deliverId = deliveryCodes.SingleOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.DeliverTypes.Both.ToString()).CodeId;
                                    break;
                                case Entity.Enums.SubscriptionType.Digital:
                                    deliverId = deliveryCodes.SingleOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.DeliverTypes.Digital.ToString()).CodeId;

                                    break;
                                case Entity.Enums.SubscriptionType.Print:
                                    deliverId = deliveryCodes.SingleOrDefault(x => x.CodeName == FrameworkUAD_Lookup.Enums.DeliverTypes.Print.ToString()).CodeId;

                                    break;
                            }
                            sp.DeliverID = deliverId;  //get from SubGenData.Subscription.type = Digital, Print, Both
                            Subscription scriptWrk = new Subscription();
                            Entity.Subscription script = scriptWrk.Select(pay.subscription_id);
                            if (script != null && script.subscription_id > 0)
                                sp.TotalIssues = script.issues; //get from SubGenData.Subscription.issues


                            //unknown values - can't get from SubGen as of 4/22/2016 JW
                            //sp.ExpireIssueDate;
                            //sp.Frequency;
                            //sp.GraceIssues;
                            //sp.PriceCodeID;
                            //sp.StartIssueDate;
                            //sp.Term;
                            //sp.WriteOffAmount;
                        }
                    }
                }
                success = spWrk.Save(spList, client);
            }
            catch (Exception ex)
            {
                success = false;
                string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                KMPlatform.BusinessLogic.ApplicationLog alWrk = new KMPlatform.BusinessLogic.ApplicationLog();
                alWrk.LogCriticalError(message, "SubscriptionPaid.Save", KMPlatform.BusinessLogic.Enums.Applications.ADMS_Engine);
            }
            return success;
        }
        public int Save(Guid stRecordIdentifier, KMPlatform.Object.ClientConnections client)
        {
            //grab data from SubGenData.Payment by STRec
            //Save to SubscriptionPaid
            int spId = 0;
            FrameworkUAD.BusinessLogic.SubscriptionPaid spWrk = new FrameworkUAD.BusinessLogic.SubscriptionPaid();
            FrameworkUAD.Entity.SubscriptionPaid sp = new FrameworkUAD.Entity.SubscriptionPaid();
            try
            {
                spId = spWrk.Save(sp, client);
            }
            catch (Exception ex)
            {
                API.Authentication.SaveApiLog(ex, this.GetType().ToString(), this.GetType().Name.ToString());
            }
            return spId;
        }
    }
}
