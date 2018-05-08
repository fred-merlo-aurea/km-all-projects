using System;
using System.Web.Configuration; 
using System.Collections.Specialized; 
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;
using AuthorizeNet.APICore;
using AuthorizeNet.Helpers;
using AuthorizeNet;
using System.Text;
using KMPS_JF_Objects.Objects;

namespace KMPS_JF.Class
{	
	/// <summary>
	/// This class provides Credit Card processing against
    /// the Authorize.Net Gateway.
	/// </summary>
	public class ccAuthorizeNet: ccProcessing
	{                     
		/// <summary>
		/// Validates the actual card against Authorize.Net Gateway using the API 
		/// interface.
		/// <seealso>Class ccAuthorizeNet</seealso>
		/// </summary>
		/// <param name=""></param>
		/// <returns>Boolean</returns>
		public override bool ValidateCard(Publication pub)      
		{
            try
            {
                if (pub.AuthorizeDotNetSignature.Trim().Length > 0 && pub.AuthorizeDotNetAccount.Trim().Length > 0)
                {
                    this.MerchantId = pub.AuthorizeDotNetAccount;
                    this.Signature = pub.AuthorizeDotNetSignature;  
                }
                else
                {
                    this.MerchantId = WebConfigurationManager.AppSettings["AuthorizeDotnetLogin"].ToString();
                    this.Signature = WebConfigurationManager.AppSettings["AuthorizeDotnetKey"].ToString();
                }
                
                string CardNo = Regex.Replace(this.CreditCardNumber, @"[ -/._#]", "");
                CardNo = CardNo.Trim().Replace(" ","");   
                var request = new AuthorizationRequest(CardNo, this.CreditCardExpiration, this.OrderAmount, this.Comment, true);    
                var gateway = new Gateway(this.MerchantId, this.Signature);                
                string orderItems = string.Empty;

                gateway.TestMode = Convert.ToBoolean(WebConfigurationManager.AppSettings["AuthorizeDotNetDemoMode"].ToString());
             
                request.AddCardCode(this.SecurityCode);
                request.Country = this.Country;
                request.CustomerIp = System.Web.HttpContext.Current.Request.UserHostAddress;      
                request.Phone = this.Phone;
                request.City = this.City;
                request.Company = this.Company;  
                request.Country = this.Country;
                request.Email = this.Email; 

                if (this.itemList.Count > 0)
                {
                    string SubscriberID = ECNUtils.GetSubscriberID(itemList[0].GroupID, itemList[0].CustID, this.Email);

                    request.AddCustomer(SubscriberID, this.Email, this.Firstname, this.Lastname, this.Address, this.City, this.State, this.Zip);
                    request.AddShipping(SubscriberID, this.Email, this.Firstname, this.Lastname, this.Address, this.City, this.State, this.Zip);
                    request.ShipToCity = this.City;
                    request.ShipToCountry = this.Country;                   
                }

                foreach (Item item in itemList)                   
                    orderItems += item.ItemName + " (" + item.ItemCode + ")" + ",";   

                request.AddLineItem(orderItems.TrimEnd(','), orderItems.TrimEnd(','), orderItems.TrimEnd(','), Convert.ToInt32(itemList[0].ItemQty), this.OrderAmount, false);
                request.AddMerchantValue("x_Description",orderItems.TrimEnd(','));                          

                var response = gateway.Send(request);
                this.ValidatedMessage = response.Message;
                this.TransactionId = response.TransactionID;       
                return response.Approved;
            }
            catch
            {              
                return false;  
            }
		} 		
	} 
}
