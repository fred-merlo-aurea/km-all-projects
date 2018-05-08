using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Encore.PayPal.Nvp;
using System.Web.Configuration;
using System.Text.RegularExpressions; 
using KMPS_JF_Objects.Objects;

namespace KMPS_JF.Class
{
    public class ccPayPal : ccProcessing
    {
        private NvpDoDirectPayment ppPay = new NvpDoDirectPayment();             
        
        public override bool ValidateCard(Publication pub)
        {
            try
            {
                if (WebConfigurationManager.AppSettings["PayEnvironment"].ToString().Equals("sandbox", StringComparison.OrdinalIgnoreCase))
                    ppPay.Environment = NvpEnvironment.Sandbox;
                else if (WebConfigurationManager.AppSettings["PayEnvironment"].ToString().Equals("live", StringComparison.OrdinalIgnoreCase))
                    ppPay.Environment = NvpEnvironment.Live;
                else if (WebConfigurationManager.AppSettings["PayEnvironment"].ToString().Equals("betasandbox", StringComparison.OrdinalIgnoreCase))
                    ppPay.Environment = NvpEnvironment.BetaSandbox;


                if (pub.PayflowSignature.Trim().Length > 0 && pub.PayflowPassword.Trim().Length > 0 && pub.PayflowAccount.Trim().Length > 0)
                {
                    ppPay.Credentials.Username = pub.PayflowAccount;
                    ppPay.Credentials.Password = pub.PayflowPassword;
                    ppPay.Credentials.Signature = pub.PayflowSignature;  
                }
                else
                {
                    ppPay.Credentials.Username = WebConfigurationManager.AppSettings["PayflowUserName"].ToString();
                    ppPay.Credentials.Password = WebConfigurationManager.AppSettings["PayflowPassword"].ToString();
                    ppPay.Credentials.Signature = WebConfigurationManager.AppSettings["PayflowSignature"].ToString();
                }

                ppPay.Add(NvpDoDirectPayment.Request._IPADDRESS, HttpContext.Current.Request.UserHostAddress);          
                ppPay.Add(NvpDoDirectPayment.Request._PAYMENTACTION, NvpPaymentActionCodeType.Sale);
                ppPay.Add(NvpDoDirectPayment.Request._AMT, String.Format("{0:0.00}", this.OrderAmount));
                ppPay.Add(NvpDoDirectPayment.Request.ITEMAMT, String.Format("{0:0.00}", this.OrderAmount));

                if (this.CreditCardType == CreditCardType.MasterCard)
                    ppPay.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.MasterCard);
                else if (this.CreditCardType == CreditCardType.Visa)
                    ppPay.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.Visa);
                else if (this.CreditCardType == CreditCardType.Amex)
                    ppPay.Add(NvpDoDirectPayment.Request._CREDITCARDTYPE, NvpCreditCardTypeType.Amex);

                string CardNo = Regex.Replace(this.CreditCardNumber, @"[ -/._#]", "");  
                ppPay.Add(NvpDoDirectPayment.Request._ACCT, CardNo);           
                ppPay.Add(NvpDoDirectPayment.Request._EXPDATE, this.CreditCardExpirationMonth + this.CreditCardExpirationFullYear);           
                ppPay.Add(NvpDoDirectPayment.Request.CVV2, this.SecurityCode);
                ppPay.Add(NvpDoDirectPayment.Request._FIRSTNAME, this.Firstname);
                ppPay.Add(NvpDoDirectPayment.Request._LASTNAME, this.Lastname); 

                if (this.Country.ToUpper() == "US" || this.Country.ToUpper() == "CA")       
                {
                    ppPay.Add(NvpDoDirectPayment.Request.STATE, this.State);   
                }
                else
                {
                    ppPay.Add(NvpDoDirectPayment.Request.STATE, this.City);
                }

                ppPay.Add(NvpDoDirectPayment.Request.SHIPTOSTREET, this.Address);                 
                ppPay.Add(NvpDoDirectPayment.Request.SHIPTOCITY, this.City);  
                ppPay.Add(NvpDoDirectPayment.Request.SHIPTOZIP, this.Zip);
                string countrCode = string.Empty;

                if (this.Country.ToUpper() == "UNITED STATES OF AMERICA" || this.Country.ToUpper() == "UNITED STATES")
                {
                    countrCode = "US";
                }
                else if (this.Country.ToUpper() == "CANADA")
                {
                    countrCode = "CA";
                }
                else
                {
                    countrCode = this.Country.ToUpper();  
                }

                ppPay.Add(NvpDoDirectPayment.Request.SHIPTOCOUNTRYCODE, countrCode);
                ppPay.Add(NvpDoDirectPayment.Request.SHIPTOPHONENUM, this.Phone);

                if (this.Country.ToUpper() == "US" || this.Country.ToUpper() == "CA")           
                {
                    ppPay.Add(NvpDoDirectPayment.Request.SHIPTOSTATE, this.State);
                }
                else
                {
                    ppPay.Add(NvpDoDirectPayment.Request.SHIPTOSTATE, this.City);
                }

                ppPay.Add(NvpDoDirectPayment.Request.SHIPTONAME, this.Name);     
                ppPay.Add(NvpDoDirectPayment.Request.PHONENUM, this.Phone);
                ppPay.Add(NvpDoDirectPayment.Request.STREET, this.Address);
                ppPay.Add(NvpDoDirectPayment.Request.STREET2, this.Address2);    
                ppPay.Add(NvpDoDirectPayment.Request.CITY, this.City);
                ppPay.Add(NvpDoDirectPayment.Request.ZIP, this.Zip);
                ppPay.Add(NvpDoDirectPayment.Request.COUNTRYCODE, countrCode);        
                                                                                
                ppPay.Add(NvpDoDirectPayment.Request.EMAIL, this.Email);

                string SubscriberID = "";    

                if (itemList.Count > 0)
                {
                    try
                    {
                        SubscriberID = ECNUtils.GetSubscriberID(itemList[0].GroupID, itemList[0].CustID, this.Email);             
                    }
                    catch
                    {
                        SubscriberID = "0";
                    }

                    ppPay.Add(NvpDoDirectPayment.Request.CUSTOM, SubscriberID);
                }

                for (int i = 0; i < itemList.Count; i++)
                {
                    ppPay.Add(NvpDoDirectPayment.Request.L_NAMEn + i.ToString(), itemList[i].ItemName);
                    ppPay.Add(NvpDoDirectPayment.Request.L_AMTn + i.ToString(), itemList[i].ItemAmount);
                    ppPay.Add(NvpDoDirectPayment.Request.L_QTYn + i.ToString(), itemList[i].ItemQty);
                }

                bool isSuccess =  ppPay.Post();  
     
                if(isSuccess)
                {
                    this.TransactionId =  ppPay.Get(NvpDoDirectPayment.Response.TRANSACTIONID.ToString());    
                    return true; 
                }

                return false;  
            }
            catch 
            {               
                return false;
            } 
        }
    }
}
