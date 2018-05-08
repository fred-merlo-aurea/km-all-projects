using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace FrameworkUAD_Lookup.BusinessLogic
{
    public class Code
    {
        #region Select Methods
        public List<Entity.Code> Select()
        {
            List<Entity.Code> x = null;
            x = DataAccess.Code.Select().ToList();
            foreach (Entity.Code c in x)
            {
                if (c.HasChildren == true)
                    c.Children = SelectChildren(c.CodeId);
            }
            return x;
        }
        public List<Entity.Code> Select(int codeTypeId)
        {
            List<Entity.Code> x = null;
            x = DataAccess.Code.Select(codeTypeId).ToList();
            foreach (Entity.Code c in x)
            {
                if (c.HasChildren == true)
                    c.Children = SelectChildren(c.CodeId);
            }
            return x;
        }
        public List<Entity.Code> Select(Enums.CodeType codeType)
        {
            List<Entity.Code> x = null;
            x = DataAccess.Code.Select(codeType).ToList();
            foreach (Entity.Code c in x)
            {
                if (c.HasChildren == true)
                    c.Children = SelectChildren(c.CodeId);
            }
            return x;
        }
        public List<Entity.Code> SelectForDemographicAttribute(Enums.CodeType codeType, int dataCompareResultQueId, string ftpFolder)
        {
            List<Entity.Code> x = null;
            x = DataAccess.Code.SelectForDemographicAttribute(codeType, dataCompareResultQueId, ftpFolder).ToList();
            foreach (Entity.Code c in x)
            {
                if (c.HasChildren == true)
                    c.Children = SelectChildren(c.CodeId);
            }
            return x;
        }
        public List<Entity.Code> SelectChildren(int parentCodeID)
        {
            List<Entity.Code> x = null;
            x = DataAccess.Code.SelectChildren(parentCodeID).ToList();
            return x;
        }
        public Entity.Code SelectCodeId(int codeId)
        {
            Entity.Code x = null;
            x = DataAccess.Code.SelectCodeId(codeId);
            if (x.HasChildren == true)
                x.Children = SelectChildren(x.CodeId);
            return x;
        }
        public Entity.Code SelectCodeName(Enums.CodeType codeType, string codeName)
        {
            Entity.Code x = null;
            x = DataAccess.Code.SelectCodeName(codeType, codeName);
            if (x.HasChildren == true)
                x.Children = SelectChildren(x.CodeId);
            return x;
        }
        public Entity.Code SelectCodeValue(Enums.CodeType codeType, string codeValue)
        {
            Entity.Code x = null;
            x = DataAccess.Code.SelectCodeValue(codeType, codeValue);
            if (x.HasChildren == true)
                x.Children = SelectChildren(x.CodeId);
            return x;
        }
        public System.Data.DataTable dtGetCode(Enums.CodeType codeType)
        {
            string ct = codeType.ToString().Replace("_", " ");
            return DataAccess.Code.dtGetCode(ct);
        }

        public List<Entity.Code> SelectChildren(Enums.CodeType parentCodeType, string parentCode)
        {
            List<Entity.Code> x = null;
            x = DataAccess.Code.SelectChildren(parentCodeType, parentCode).ToList();
            return x;
        }
        #endregion

        #region Value Checks / saving
        public bool CodeExist(string codeName, int codeTypeID)
        {
            bool x = false;
            x = DataAccess.Code.CodeExist(codeName, codeTypeID);
            return x;
        }
        public bool CodeExist(string codeName, Enums.CodeType codeType)
        {
            bool x = false;
            x = DataAccess.Code.CodeExist(codeName, codeType);
            return x;
        }
        public bool CodeValueExist(string codeValue, int codeTypeID)
        {
            bool x = false;
            x = DataAccess.Code.CodeValueExist(codeValue, codeTypeID);
            return x;
        }
        public bool CodeValueExist(string codeValue, Enums.CodeType codeType)
        {
            bool x = false;
            x = DataAccess.Code.CodeValueExist(codeValue, codeType);
            return x;
        }
        public int Save(Entity.Code x)
        {
            if (x.DateCreated == null)
                x.DateCreated = DateTime.Now;

            using (TransactionScope scope = new TransactionScope())
            {
                x.CodeId = DataAccess.Code.Save(x);
                scope.Complete();
            }

            return x.CodeId;
        }
        #endregion

        #region Circ
        #region Action Type
        //public bool Exists(string actionTypeName)
        //{
        //    List<Entity.ActionType> all = Select().ToList();
        //    if (all.Exists(x => x.ActionTypeName.Equals(actionTypeName, StringComparison.CurrentCultureIgnoreCase)))
        //        return true;
        //    else
        //        return false;
        //}
        //public Entity.ActionType Select(Enums.ActionType actionTypeName)
        //{
        //    Entity.ActionType at = null;
        //    List<Entity.ActionType> all = Select().ToList();
        //    if (all.Exists(x => x.ActionTypeName.Equals(actionTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase)))
        //        at = all.SingleOrDefault(x => x.ActionTypeName.Equals(actionTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase));
        //    return at;
        //}
        //public Entity.ActionType Select(string actionTypeName)
        //{
        //    Entity.ActionType at = null;
        //    List<Entity.ActionType> all = Select().ToList();
        //    if (all.Exists(x => x.ActionTypeName.Equals(actionTypeName, StringComparison.CurrentCultureIgnoreCase)))
        //        at = all.SingleOrDefault(x => x.ActionTypeName.Equals(actionTypeName, StringComparison.CurrentCultureIgnoreCase));
        //    return at;
        //}
        //public List<Entity.ActionType> Select()
        //{
        //    List<Entity.ActionType> retList = null;
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        retList = DataAccess.ActionType.Select();
        //        scope.Complete();
        //    }

        //    return retList;
        //}

        //public int Save(Entity.ActionType x)
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        x.ActionTypeID = DataAccess.ActionType.Save(x);
        //        scope.Complete();
        //    }

        //    return x.ActionTypeID;
        //}
        #endregion
        #region AddressType
        //public List<Entity.AddressType> Select()
        //{
        //    List<Entity.AddressType> retList = null;
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        retList = DataAccess.AddressType.Select();
        //        scope.Complete();
        //    }

        //    return retList;
        //}
        #endregion
        #region CreditCardType
        //public List<Entity.CreditCardType> Select()
        //{
        //    List<Entity.CreditCardType> retList = null;
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        retList = DataAccess.CreditCardType.Select();
        //        scope.Complete();
        //    }

        //    return retList;
        //}

        //public int Save(Entity.CreditCardType x)
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        x.CreditCardTypeID = DataAccess.CreditCardType.Save(x);
        //        scope.Complete();
        //    }

        //    return x.CreditCardTypeID;
        //}
        #endregion
        #region PaymentType
        //public List<Entity.PaymentType> Select()
        //{
        //    List<Entity.PaymentType> retList = null;
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        retList = DataAccess.PaymentType.Select();
        //        scope.Complete();
        //    }

        //    return retList;
        //}

        //public int Save(Entity.PaymentType x)
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        x.PaymentTypeID = DataAccess.PaymentType.Save(x);
        //        scope.Complete();
        //    }

        //    return x.PaymentTypeID;
        //}
        #endregion
        #region QualificationSourceType
        //public List<Entity.QualificationSourceType> Select()
        //{
        //    List<Entity.QualificationSourceType> retList = null;
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        retList = DataAccess.QualificationSourceType.Select();
        //        scope.Complete();
        //    }

        //    return retList;
        //}

        //public int Save(Entity.QualificationSourceType x)
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        x.QSourceTypeID = DataAccess.QualificationSourceType.Save(x);
        //        scope.Complete();
        //    }

        //    return x.QSourceTypeID;
        //}
        #endregion
        #region SubscriberSourceType
        //public Entity.SubscriberSourceType Select(Enums.SubscriberSourceType subscriberSourceTypeName)
        //{
        //    Entity.SubscriberSourceType sst = null;
        //    List<Entity.SubscriberSourceType> all = Select().ToList();
        //    if (all.Exists(x => x.SubscriberSourceTypeName.Equals(subscriberSourceTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase)))
        //        sst = all.SingleOrDefault(x => x.SubscriberSourceTypeName.Equals(subscriberSourceTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase));
        //    return sst;
        //}
        //public Entity.SubscriberSourceType Select(string subscriberSourceTypeName)
        //{
        //    Entity.SubscriberSourceType sst = null;
        //    List<Entity.SubscriberSourceType> all = Select().ToList();
        //    if (all.Exists(x => x.SubscriberSourceTypeName.Equals(subscriberSourceTypeName, StringComparison.CurrentCultureIgnoreCase)))
        //        sst = all.SingleOrDefault(x => x.SubscriberSourceTypeName.Equals(subscriberSourceTypeName, StringComparison.CurrentCultureIgnoreCase));
        //    return sst;
        //}
        //public List<Entity.SubscriberSourceType> Select()
        //{
        //    List<Entity.SubscriberSourceType> retList = null;
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        retList = DataAccess.SubscriberSourceType.Select();
        //        scope.Complete();
        //    }

        //    return retList;
        //}

        //public int Save(Entity.SubscriberSourceType x)
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        x.SubscriberSourceTypeID = DataAccess.SubscriberSourceType.Save(x);
        //        scope.Complete();
        //    }

        //    return x.SubscriberSourceTypeID;
        //}
        #endregion
        #region UserLogType
        //public List<Entity.UserLogType> Select()
        //{
        //    List<Entity.UserLogType> x = null;

        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        x = DataAccess.UserLogType.Select().ToList();
        //        scope.Complete();
        //    }
        //    return x;
        //}
        //public Entity.UserLogType Select(Enums.UserLogType userLogTypeName)
        //{
        //    Entity.UserLogType ult = null;
        //    List<Entity.UserLogType> all = Select().ToList();
        //    if (all.Exists(x => x.UserLogTypeName.Equals(userLogTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase)))
        //        ult = all.SingleOrDefault(x => x.UserLogTypeName.Equals(userLogTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase));
        //    return ult;
        //}
        //public Entity.UserLogType Select(string userLogTypeName)
        //{
        //    Entity.UserLogType ult = null;
        //    List<Entity.UserLogType> all = Select().ToList();
        //    if (all.Exists(x => x.UserLogTypeName.Equals(userLogTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase)))
        //        ult = all.SingleOrDefault(x => x.UserLogTypeName.Equals(userLogTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase));
        //    return ult;
        //}
        //public Entity.UserLogType Select(int userLogTypeID)
        //{
        //    Entity.UserLogType ult = null;
        //    List<Entity.UserLogType> all = Select().ToList();
        //    if (all.Exists(x => x.UserLogTypeID == userLogTypeID))
        //        ult = all.SingleOrDefault(x => x.UserLogTypeID == userLogTypeID);
        //    return ult;
        //}

        //public int Save(Entity.UserLogType x)
        //{
        //    if (x.CreatedByUserID < 1)
        //        x.CreatedByUserID = -1;
        //    if (x.DateCreated == null)
        //        x.DateCreated = DateTime.Now;
        //    //ensure password is encrypted n salted
        //    //if password is null or empty generate a random password

        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        x.UserLogTypeID = DataAccess.UserLogType.Save(x);
        //        scope.Complete();
        //    }

        //    return x.UserLogTypeID;
        //}
        #endregion
        #endregion
        #region UAD
        #region ExportType

        #endregion
        #region RecurrenceType

        #endregion
        #endregion
        #region UAS
        #region ConfigurationType

        #endregion
        #region FieldMapppingType
        //public List<Entity.FieldMappingType> Select()
        //{
        //    List<Entity.FieldMappingType> x = null;
        //    x = DataAccess.FieldMappingType.Select().ToList();

        //    return x;
        //}
        //public Entity.FieldMappingType Select(Enums.FieldMappingTypeName fmtName)
        //{
        //    Entity.FieldMappingType x = null;
        //    x = DataAccess.FieldMappingType.Select(fmtName);

        //    return x;
        //}
        //public int IgnoredTypeID()
        //{
        //    return Select(Enums.FieldMappingTypeName.Ignored).FieldMappingTypeID;
        //}
        //public int DemographicTypeID()
        //{
        //    return Select(Enums.FieldMappingTypeName.Demographic).FieldMappingTypeID;
        //}
        //public int StandardTypeID()
        //{
        //    return Select(Enums.FieldMappingTypeName.Standard).FieldMappingTypeID;
        //}

        //public int Save(Entity.FieldMappingType x)
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        x.FieldMappingTypeID = DataAccess.FieldMappingType.Save(x);
        //        scope.Complete();
        //    }

        //    return x.FieldMappingTypeID;
        //}
        #endregion
        #region FileSnippetType
        //public int Save(Entity.FileSnippetType x)
        //{
        //    if (x.DateCreated == null)
        //        x.DateCreated = DateTime.Now;

        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        x.FileSnippetTypeID = DataAccess.FileSnippetType.Save(x);
        //        scope.Complete();
        //    }

        //    return x.FileSnippetTypeID;
        //}
        #endregion
        #region FileStatusType
        //public List<Entity.FileStatusType> Select()
        //{
        //    List<Entity.FileStatusType> retItem = null;
        //    retItem = DataAccess.FileStatusType.Select().ToList();

        //    return retItem;
        //}

        //public Entity.FileStatusType Select(int fileStatusTypeID)
        //{
        //    Entity.FileStatusType retItem = null;
        //    retItem = DataAccess.FileStatusType.Select(fileStatusTypeID);

        //    return retItem;
        //}

        //public Entity.FileStatusType Select(Enums.FileStatusTypeName fileStatusName)
        //{
        //    Entity.FileStatusType retItem = null;
        //    retItem = DataAccess.FileStatusType.Select(fileStatusName);

        //    return retItem;
        //}
        #endregion
        #region FileRecurranceType
        //public List<Entity.SourceFileType> Select()
        //{
        //    List<Entity.SourceFileType> x = null;
        //    x = DataAccess.SourceFileType.Select().ToList();

        //    return x;
        //}
        //public Entity.SourceFileType Select(int SourceFileTypeID)
        //{
        //    Entity.SourceFileType x = null;
        //    x = DataAccess.SourceFileType.Select(SourceFileTypeID);

        //    return x;
        //}
        #endregion
        #region TransformationType
        //public List<Entity.TransformationType> Select()
        //{
        //    List<Entity.TransformationType> x = null;
        //    x = DataAccess.TransformationType.Select().ToList();

        //    return x;
        //}

        //public Entity.TransformationType Select(int transformationTypeID)
        //{
        //    Entity.TransformationType x = null;
        //    x = DataAccess.TransformationType.Select(transformationTypeID);

        //    return x;
        //}

        //public int Save(Entity.TransformationType x)
        //{
        //    if (x.DateCreated == null)
        //        x.DateCreated = DateTime.Now;

        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        x.TransformationTypeID = DataAccess.TransformationType.Save(x);
        //        scope.Complete();
        //    }

        //    return x.TransformationTypeID;
        //}
        #endregion
        #region UserLogType
        //public List<Entity.UserLogType> Select()
        //{
        //    List<Entity.UserLogType> x = null;
        //    x = DataAccess.UserLogType.Select().ToList();

        //    return x;
        //}
        //public Entity.UserLogType Select(Enums.UserLogType userLogTypeName)
        //{
        //    Entity.UserLogType ult = null;
        //    List<Entity.UserLogType> all = Select().ToList();
        //    if (all.Exists(x => x.UserLogTypeName.Equals(userLogTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase)))
        //        ult = all.SingleOrDefault(x => x.UserLogTypeName.Equals(userLogTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase));
        //    return ult;
        //}
        //public Entity.UserLogType Select(string userLogTypeName)
        //{
        //    Entity.UserLogType ult = null;
        //    List<Entity.UserLogType> all = Select().ToList();
        //    if (all.Exists(x => x.UserLogTypeName.Equals(userLogTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase)))
        //        ult = all.SingleOrDefault(x => x.UserLogTypeName.Equals(userLogTypeName.ToString(), StringComparison.CurrentCultureIgnoreCase));
        //    return ult;
        //}
        //public Entity.UserLogType Select(int userLogTypeID)
        //{
        //    Entity.UserLogType ult = null;
        //    List<Entity.UserLogType> all = Select().ToList();
        //    if (all.Exists(x => x.UserLogTypeID == userLogTypeID))
        //        ult = all.SingleOrDefault(x => x.UserLogTypeID == userLogTypeID);
        //    return ult;
        //}

        //public int Save(Entity.UserLogType x)
        //{
        //    if (x.CreatedByUserID < 1)
        //        x.CreatedByUserID = -1;
        //    if (x.DateCreated == null)
        //        x.DateCreated = DateTime.Now;
        //    //ensure password is encrypted n salted
        //    //if password is null or empty generate a random password

        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        x.UserLogTypeID = DataAccess.UserLogType.Save(x);
        //        scope.Complete();
        //    }

        //    return x.UserLogTypeID;
        //}
        #endregion
        #region DatabaseDestinationType

        #endregion
        #region DatabaseFileType

        #endregion
        #region PostalServiceType

        #endregion
        #endregion


        #region Model
        public List<Model.Operator> GetOperators()
        {
            return DataAccess.Code.GetOperators().ToList();
        }
        #endregion
        #region SelectListItem dropdowns
        /// <summary>
        /// Text is DisplayName
        /// Value is CodeId
        /// </summary>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public List<SelectListItem> GetDropDownList(Enums.CodeType codeType)
        {
            List<Entity.Code> codes = DataAccess.Code.Select(codeType).OrderBy(x => x.DisplayOrder).ToList();
            List<SelectListItem> values = new List<SelectListItem>();
            foreach (var c in codes)
                values.Add(new SelectListItem() { Text = c.DisplayName, Value = c.CodeId.ToString() });

            return values;
        }
        public List<SelectListItem> GetDropDownList(Enums.CodeType parentCodeType, string parentCode)
        {
            List<Entity.Code> codes = DataAccess.Code.SelectChildren(parentCodeType, parentCode).ToList();
            List<SelectListItem> values = new List<SelectListItem>();
            foreach (var c in codes)
                values.Add(new SelectListItem() { Text = c.DisplayName, Value = c.CodeId.ToString() });

            return values;
        }
        #endregion
    }
}
