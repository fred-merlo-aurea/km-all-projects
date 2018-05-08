using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using PayPal.Payments.Common;
using PayPal.Payments.Common.Utility;
using PayPal.Payments.DataObjects;
using PayPal.Payments.Transactions;
using System.Web.Configuration;
using System.Text.RegularExpressions;

namespace KMPS_JF_Objects.Objects
{
    public class ccPayPalPro : ccProcessing
    {
        public override bool ValidateCard(Publication pub)
        {
            try
            {
                PayflowConnectionData Connection = new PayflowConnectionData();

                String RequestID = PayflowUtility.RequestId;
                CultureInfo us = new CultureInfo("en-US");


                UserInfo User = new UserInfo(pub.PayflowAccount, pub.PayflowVendor, pub.PayflowPartner, pub.PayflowPassword);

                Invoice Inv = new Invoice();

                // Set Amount.  The amount cannot be changed once submitted.
                Currency Amt = new Currency(this.OrderAmount, "USD");
                Inv.Amt = Amt;

                BillTo Bill = new BillTo();
                Bill.BillToFirstName = this.Firstname;
                Bill.BillToLastName = this.Lastname;
                Bill.BillToStreet = this.Address;
                Bill.BillToCity = this.City;
                Bill.BillToState = this.State;
                Bill.BillToZip = this.Zip;
                Bill.BillToPhone = this.Phone;
                Bill.BillToEmail = this.Email;

                if ((this.Country.ToUpper() == "UNITED STATES OF AMERICA") || (this.Country.ToUpper() == "UNITED STATES"))
                    Bill.BillToCountry = "US";
                else if (this.Country.ToUpper() == "CANADA")
                    Bill.BillToCountry = "CA";
                else
                    Bill.BillToCountry = this.Country.ToUpper();

                Inv.BillTo = Bill;

                // Create Payment Device - Credit Card Data Object
                string CardNo = Regex.Replace(this.CreditCardNumber, @"[ -/._#]", "");
                CreditCard cc = new CreditCard(CardNo, this.CreditCardExpirationMonth + this.CreditCardExpirationFullYear);
                cc.Cvv2 = this.SecurityCode;

                // Create Card Tender Data Object
                CardTender Card = new CardTender(cc);

                // Create new Transaction
                SaleTransaction SaleTrans = new SaleTransaction(User, Connection, Inv, Card, RequestID);

                // Submit the Sales Transaction
                SaleTrans.SubmitTransaction();

                // If Transaction-Response returns zero means transaction was complete and successfull
                if (SaleTrans.Response.TransactionResponse.Result == 0)
                {
                    this.TransactionId = SaleTrans.Response.TransactionResponse.Pnref.ToString();
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
