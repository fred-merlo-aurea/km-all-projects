using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using FrameworkUAD.BusinessLogic.Fakes;
using FrameworkUAD.Entity;
using KM.Common;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using BusinessEnums = FrameworkUAD.BusinessLogic.Enums;
using ShimProductSubscription = FrameworkUAD.BusinessLogic.Fakes.ShimProductSubscriptionsExtensionMapper;
using UserDataMastEntity = FrameworkUAD.Entity.UserDataMask;

namespace UAS.Web.Tests.Helpers.Common
{
    [ExcludeFromCodeCoverage]
    public class Fakes
    {
        private IDisposable _context;
        private const string IdUppercase = "ID";
        private const string IdUppercaseRegex = "ID$";
        private const string IdPascalCase = "Id";
        private const string IdPascalCaseRegex = "Id$";

        protected const string DisplayNameText = "Display Name Text";
        protected const string CustomFieldText = "Custom Field Text";
        protected const string StandardFieldText = "Standard Field Text";
        protected const string DescriptionKey = "_Description";
        protected const int PubId = 20;
        protected const int ResponseGroupId = 30;

        public void SetupFakes()
        {
            _context = ShimsContext.Create();
        }

        [TearDown]
        public void CleanUp() => _context?.Dispose();

        protected virtual string GetKey(IDictionary<string, string> dictionary, string property)
        {
            Guard.NotNull(dictionary, nameof(dictionary));

            return dictionary.FirstOrDefault(x => x.Value == property).Key;
        }

        protected virtual string GetKeyWithUppercase(IDictionary<string, string> dictionary, string property)
        {
            Guard.NotNull(dictionary, nameof(dictionary));

            var propertyName = Regex.Replace(property, IdPascalCaseRegex, IdUppercase);
            return dictionary.FirstOrDefault(x => x.Value == propertyName).Key;
        }

        protected virtual string CreateDataTypeResult(string property, string dataType)
        {
            return $"{property}|{dataType}";
        }

        protected void ShimForUserDataMaskProfileFields()
        {
            FrameworkUAD.BusinessLogic.Fakes.ShimUserDataMask.GetByUserIDClientConnectionsInt32 = (conn, id)
                => new List<UserDataMastEntity>
                {
                    new UserDataMastEntity { MaskField = Consts.FieldFirstName },
                    new UserDataMastEntity { MaskField = Consts.FieldLastName },
                    new UserDataMastEntity { MaskField = Consts.FieldEmail },
                    new UserDataMastEntity { MaskField = Consts.FieldCompany },
                    new UserDataMastEntity { MaskField = Consts.FieldTitle },
                    new UserDataMastEntity { MaskField = Consts.FieldAddress },
                    new UserDataMastEntity { MaskField = Consts.FieldAddress2 },
                    new UserDataMastEntity { MaskField = Consts.FieldAddress3 },
                    new UserDataMastEntity { MaskField = Consts.FieldCity },
                    new UserDataMastEntity { MaskField = Consts.FieldState },
                    new UserDataMastEntity { MaskField = Consts.FieldZip }
                };
        }

        protected void ShimResponseGroupSelect()
        {
            ShimResponseGroup.AllInstances.SelectInt32ClientConnections = (_, pubId, clientConnection)
                => new List<ResponseGroup>
                {
                    new ResponseGroup
                    {
                        DisplayName = DisplayNameText,
                        ResponseGroupID = ResponseGroupId
                    }
                };
        }

        protected void ShimProductsSubscriptions(BusinessEnums.FieldType fieldType)
        {
            ShimProductSubscription.AllInstances.SelectAllClientConnections = (_, clientConnection) 
                => new List<ProductSubscriptionsExtensionMapper>
                {
                    new ProductSubscriptionsExtensionMapper
                    {
                        PubID = PubId,
                        StandardField = StandardFieldText,
                        CustomField = CustomFieldText,
                        CustomFieldDataType = fieldType.ToString()
                    }
                };
        }
    }
}
