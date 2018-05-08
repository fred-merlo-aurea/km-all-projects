using System;
using System.Collections.Generic;
using System.Transactions;
using KMDbManagers;
using KMEntities;
using KMEnums;
using KMModels;
using KMModels.PostModels;

namespace KMManagers
{
    public class FormResultManager : ManagerBase
    {
        private FormResultDbManager FRM
        {
            get
            {
                return DB.FormResultDbManager;
            }
        }
        private FormManager fm = new FormManager();

        public TModel GetThirdpartyOutputResultByFormID<TModel>(int id) where TModel : ModelBase, new()
        {
            return GetFormResultByFormIDAndType<TModel>(id, FormResultType.ThirdpartyOutput);
        }
        
        public TModel GetFormResultByFormIDAndType<TModel>(int id, FormResultType type) where TModel : ModelBase, new()
        {
            TModel res = null;
            FormResult fr = FRM.GetFormResultByFormIDAndType(id, type);
            if (fr != null)
            {
                res = fr.ConvertToModel<TModel>();
            }

            return res;
        }

        public void DeleteByID(int id)
        {
            FRM.DeleteByID(id);
            FRM.SaveChanges();
        }

        public void Update(KMPlatform.Entity.User User, int ChannelID, FormOutputPostModel model)
        {
            Form form = fm.GetByID(ChannelID, model.Id);
            form.UpdatedBy = User.UserName;
            form.LastUpdated = DateTime.Now;
            FormResult res = FRM.GetByID(model.ResultId);
            if (res == null)
                res = FRM.GetFormResultByFormIDAndType(model.Id, FormResultType.ThirdpartyOutput);
            if (res == null)
            {
                res = new FormResult();
                res.Form_Seq_ID = model.Id;
                res.ResultType = (int)FormResultType.ThirdpartyOutput;
                FRM.Add(res);
            }
            res.URL = model.ExternalUrl;
            using (TransactionScope t = new TransactionScope())
            {
                FRM.SaveChanges();
                fm.SaveChanges();
                DB.ThirdPartyQueryValueDbManager.RewriteAll(res.FormResult_Seq_ID, GetDbValues(model.ThirdPartyQueryValue));
                DB.ThirdPartyQueryValueDbManager.SaveChanges();
                t.Complete();
            }
        }

        private IEnumerable<ThirdPartyQueryValue> GetDbValues(IEnumerable<ThirdPartyQueryValueModel> values)
        {
            if (values != null)
            {
                foreach (var v in values)
                {
                    ThirdPartyQueryValue db = new ThirdPartyQueryValue();
                    db.Control_ID = v.Value;
                    db.Name = v.Name;

                    yield return db;
                }
            }
        }
    }
}