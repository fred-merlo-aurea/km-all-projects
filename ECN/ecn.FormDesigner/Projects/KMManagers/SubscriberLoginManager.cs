using System;
using System.Collections.Generic;
using KMDbManagers;
using KMEntities;
using KMModels;
using KMModels.PostModels;
using System.Transactions;

namespace KMManagers
{
    public class SubscriberLoginManager : ManagerBase
    {
        private SubscriberLoginDbManager SL
        {
            get
            {
                return DB.SubscriberLoginDbManager;
            }
        }

        private FormDbManager FM
        {
            get
            {
                return DB.FormDbManager;
            }
        }

        public TModel GetByID<TModel>(int id) where TModel : ModelBase, new()
        {
            TModel res = null;
            SubscriberLogin formSL = GetByID(id);
            if (formSL != null)
            {
                res = formSL.ConvertToModel<TModel>();
            }

            return res;
        }

        internal SubscriberLogin GetByID(int id)
        {
            return SL.GetByFormID(id);
        }        

        public void Save(KMPlatform.Entity.User User, int ChannelID, FormSubscriberLoginPostModel model)
        {
            Form form = FM.GetByID(ChannelID, model.FormID);
            form.UpdatedBy = User.UserName;
            form.LastUpdated = DateTime.Now;

            SubscriberLogin formSL = GetByID(model.FormID);
            bool needToAdd = false;
            if (formSL == null)
            {
                needToAdd = true;
                formSL = new SubscriberLogin();
            }

            FillData(formSL, model);

            using (TransactionScope t = new TransactionScope())
            {
                if (needToAdd)
                    SL.Add(formSL);
                SL.SaveChanges();
                FM.SaveChanges();

                t.Complete();
            }
        }

        private void FillData(SubscriberLogin login, FormSubscriberLoginPostModel model)
        {
            login.FormID = model.FormID;
            login.LoginRequired = model.LoginRequired;
            login.OtherIdentification = model.OtherIdentificationSelection ? model.OtherIdentification : "";
            login.PasswordRequired = model.PasswordRequired;
            login.AutoLoginAllowed = model.AutoLoginAllowed;
            login.LoginModalHTML = model.LoginModalHTML;
            login.LoginButtonText = model.LoginButtonText;
            login.SignUpButtonText = model.SignUpButtonText;
            login.ForgotPasswordButtonText = model.ForgotPasswordButtonText;
            login.NewSubscriberButtonText = model.NewSubscriberButtonText;
            login.ExistingSubscriberButtonText = model.ExistingSubscriberButtonText;
            login.ForgotPasswordMessageHTML = model.ForgotPasswordMessageHTML;
            login.ForgotPasswordNotificationHTML = model.ForgotPasswordNotificationHTML;
            login.ForgotPasswordFromName = model.ForgotPasswordFromName ?? "";
            login.ForgotPasswordSubject = model.ForgotPasswordSubject ?? "";
            login.EmailAddressQuerystringName = model.EmailAddressQuerystringName;
            login.OtherQuerystringName = model.OtherQuerystringName ?? "";
            login.PasswordQuerystringName = model.PasswordQuerystringName ?? "";
        }
    }
}
