using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using ecn.webservice.classes;
using ECN_Framework_Entities.Communicator;
using KMPlatform.Entity;
using BusinessCommunicator = ECN_Framework_BusinessLayer.Communicator;
using BusinessCommunicatorGroup = ECN_Framework_BusinessLayer.Communicator.Group;
using BusinessUser = KMPlatform.BusinessLogic.User;
using CommonObjects = ECN_Framework_Common.Objects;
using CommonStringFunctions = KM.Common.StringFunctions;

namespace ecn.webservice.CustomAPI
{
    public class FilterCreator
    {
        private const string CompareTypeOr = "OR";
        private const string ComparerTypeAnd = "AND";
        private const string ComparatorContains = "contains";
        private const string DatePartFull = "full";
        private const string FieldZip = "Zip";
        private const string FieldTypeString = "String";
        private const int SortOrderFirst = 1;
        private const int NotComparatorZero = 0;
        private const string Success = "Success";
        private const int IdNone = -1;
        private const char CommaChar = ',';

        private int _lastInsertedFilterId;
        private int _lastInsertedFilterGroupId;
        private Group _group;
        private User _user;
        private string _solicitationStartDate;
        private string _solicitationEndDate;
        private string _zipCodes;
        private string _filterName;
        private string _whereClause;

        public string CreateWeeklySolicitationFilter(
            string accesskey,
            int groupId,
            string solicitationStartDate,
            string solicitationEndDate,
            string zipCodes)
        {
            this._solicitationStartDate = solicitationStartDate;
            this._solicitationEndDate = solicitationEndDate;
            this._zipCodes = zipCodes;

            _user = BusinessUser.GetByAccessKey(accesskey, true);
            var zipList = new List<string>();

            if (_user?.UserID > 0)
            {
                string response;
                if (CreateWhereClause(zipList, out response))
                {
                    return response;
                }

                _group = BusinessCommunicatorGroup.GetByGroupID(groupId, _user);

                if (_group?.GroupID > 0)
                {
                    if (InsertFilter(groupId, out response))
                    {
                        return response;
                    }

                    if (InsertFilterGroup(out response))
                    {
                        return response;
                    }

                    return CreateFilterCondition(zipList);
                }
                return SendResponse.response(
                        "CreateWeeklySolicitationFilter",
                        SendResponse.ResponseCode.Fail,
                        0,
                        $"No Group set for Access Key: {accesskey}");
            }
            return SendResponse.response(
                    "CreateWeeklySolicitationFilter",
                    SendResponse.ResponseCode.Fail,
                    0,
                    $"Invalid Access Key: {accesskey}");
        }

        private string CreateFilterCondition(IEnumerable<string> zipList)
        {
            string filterConditionResult;
            if (_lastInsertedFilterGroupId > 0)
            {
                filterConditionResult = CreateFilterCondition(zipList, _group.CustomerID);
            }
            else
            {
                return SendResponse.response(
                    "CreateWeeklySolicitationFilter - FilterGroupID", 
                    SendResponse.ResponseCode.Fail,
                    0, 
                    "Failure creating Filter Condition");
            }

            if (filterConditionResult.Equals(Success))
            {
                return SendResponse.response(
                    "CreateWeeklySolicitationFilter", 
                    SendResponse.ResponseCode.Success,
                    _lastInsertedFilterId,
                    $"Filter Created: {_filterName}");
            }
            else
            {
                return SendResponse.response(
                    "CreateWeeklySolicitationFilter - CreateFilterCondition",
                    SendResponse.ResponseCode.Fail, 
                    0,
                    $"Filter could not be created: {filterConditionResult}");
            }
        }

        private bool InsertFilter(int groupId, out string response)
        {
            response = string.Empty;

            _lastInsertedFilterId = IdNone;
            try
            {
                var myFilter = new Filter
                {
                    CreatedDate = DateTime.Now,
                    CreatedUserID = _user.UserID,
                    CustomerID = _group.CustomerID,
                    FilterName = _filterName,
                    GroupID = groupId,
                    GroupCompareType = ComparerTypeAnd,
                    IsDeleted = false,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserID = _user.UserID,
                    WhereClause = _whereClause
                };

                try
                {
                    BusinessCommunicator.Filter.Validate(myFilter);
                }
                catch (CommonObjects.ECNException ex)
                {
                    var error = FormatEcnException(ex);
                    response = SendResponse.response(
                        "CreateWeeklySolicitationFilter - Filter Validate",
                        SendResponse.ResponseCode.Fail,
                        0,
                        $"Filter validation failed: {error}");
                    return true;
                }

                _lastInsertedFilterId = BusinessCommunicator.Filter.Save(myFilter, _user);
            }
            catch (Exception exception)
            {
                var error = CommonStringFunctions.FormatException(exception);
                response = SendResponse.response(
                    "CreateWeeklySolicitationFilter - Filter insert",
                    SendResponse.ResponseCode.Fail, 
                    0,
                    $"Failure creating filter: {error}");
                return true;
            }

            return false;
        }

        private bool CreateWhereClause(ICollection<string> zipList, out string response)
        {
            response = string.Empty;

            var startDate = DateTime.Now;
            DateTime.TryParse(_solicitationStartDate, out startDate);
            var endDate = DateTime.Now.AddDays(7);
            DateTime.TryParse(_solicitationEndDate, out endDate);

            _filterName = $"{startDate:MM-dd-yyyy} - {endDate:MM-dd-yyyy} Mailing Schedule";

            _whereClause = " Isnumeric(substring(zip, 1,5)) = 1 and (Convert(int,substring(zip, 1,5)) in (";
            var dataSet = new DataSet();
            using (var xmlReader = XmlReader.Create(new StringReader(_zipCodes)))
            {
                dataSet.ReadXml(xmlReader);
            }

            var table = dataSet.Tables[0];

            foreach (DataRow row in table.Rows)
            {
                _whereClause += row[0] + ",";
                if (!zipList.Contains(row[0].ToString()))
                {
                    zipList.Add(row[0].ToString());
                }
            }

            try
            {
                _whereClause = $"{_whereClause.TrimEnd(CommaChar)}))";
            }
            catch (Exception ex)
            {
                var error = CommonStringFunctions.FormatException(ex);
                {
                    response = SendResponse.response(
                        "CreateWeeklySolicitationFilter - Where Clause creation",
                        SendResponse.ResponseCode.Fail, 
                        0, 
                        error);
                    return true;
                }
            }

            return false;
        }

        private bool InsertFilterGroup(out string response)
        {
            response = string.Empty;

            _lastInsertedFilterGroupId = -1;
            if (_lastInsertedFilterId > 0)
            {
                try
                {
                    var myFilterGroup = InitFilterGroup();

                    try
                    {
                        BusinessCommunicator.FilterGroup.Validate(myFilterGroup);
                    }
                    catch (CommonObjects.ECNException ex)
                    {
                        var error = FormatEcnException(ex);
                        response = SendResponse.response(
                            "CreateWeeklySolicitationFilter - FilterGroup Validate",
                            SendResponse.ResponseCode.Fail,
                            0,
                            $"FilterGroup validation failed: {error}");
                        return true;
                    }

                    _lastInsertedFilterGroupId = BusinessCommunicator.FilterGroup.Save(myFilterGroup, _user);
                }
                catch (Exception ex)
                {
                    var error = CommonStringFunctions.FormatException(ex);
                    response = SendResponse.response(
                        "CreateWeeklySolicitationFilter - FilterGroup insert",
                        SendResponse.ResponseCode.Fail,
                        0,
                        $"Failure creating filter group: {error}");
                    return true;
                }
            }
            else
            {
                response = SendResponse.response(
                    "CreateWeeklySolicitationFilter - FilterID",
                    SendResponse.ResponseCode.Fail,
                    0,
                    "Failure creating filter");
                return true;
            }

            return false;
        }

        private FilterGroup InitFilterGroup()
        {
            var myFilterGroup = new FilterGroup
            {
                CreatedDate = DateTime.Now,
                CreatedUserID = _user.UserID,
                CustomerID = _group.CustomerID,
                FilterID = _lastInsertedFilterId,
                IsDeleted = false,
                Name = _filterName,
                SortOrder = 1,
                UpdatedDate = DateTime.Now,
                UpdatedUserID = _user.UserID,
                ConditionCompareType = CompareTypeOr
            };
            return myFilterGroup;
        }

        private static string FormatEcnException(CommonObjects.ECNException ex)
        {
            var builder = new StringBuilder();
            foreach (var error in ex.ErrorList)
            {
                builder.AppendLine($"Error: {error.ErrorMessage}");
            }

            builder.AppendLine(CommonStringFunctions.FormatException(ex));

            return builder.ToString();
        }

        private string CreateFilterCondition(IEnumerable<string> zipCodeList, int customerId)
        {
            var result = string.Empty;
            foreach (var zip in zipCodeList)
            {
                try
                {
                    var filterCondition = new FilterCondition
                    {
                        Comparator = ComparatorContains,
                        CompareValue = zip,
                        CreatedDate = DateTime.Now,
                        CreatedUserID = _user.UserID,
                        CustomerID = customerId,
                        DatePart = DatePartFull,
                        Field = FieldZip,
                        FieldType = FieldTypeString,
                        FilterGroupID = _lastInsertedFilterGroupId,
                        IsDeleted = false,
                        NotComparator = NotComparatorZero,
                        SortOrder = SortOrderFirst,
                        UpdatedDate = DateTime.Now,
                        UpdatedUserID = _user.UserID
                    };

                    try
                    {
                        BusinessCommunicator.FilterCondition.Validate(filterCondition);
                    }
                    catch (CommonObjects.ECNException ex)
                    {
                        var error = $"FilterCondition validation failed: {FormatEcnException(ex)}";
                        return error;
                    }

                    var filterConditionId = BusinessCommunicator.FilterCondition.Save(filterCondition, _user);
                    result = filterConditionId > 0 ? "Success" : "Failure";
                }
                catch (Exception ex)
                {
                    var error = CommonStringFunctions.FormatException(ex);
                    return error;
                }
            }
            return result;
        }
    }
}
