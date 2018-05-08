using System;
using System.Collections.Generic;
using FrameworkUAD.BusinessLogic;
using NUnit.Framework;
using Shouldly;
using static FrameworkUAD.BusinessLogic.Enums;

namespace FrameworkUAD.Tests.BusinessLogic
{
    [TestFixture]
    public partial class FilterMVCTests
    {
        private const string Column = "case when IsDate(E.column) = 1 then CAST(E.column AS DATETIME) else null end";
        
        private const string ProfilePermissionMail = "MAILPERMISSION";
        private const string ProfilePermissionFax = "FAXPERMISSION";
        private const string ProfilePermissionPhone = "PHONEPERMISSION";
        private const string ProfilePermissionOtherProducts = "OTHERPRODUCTSPERMISSION";
        private const string ProfilePermissionThirdParty = "THIRDPARTYPERMISSION";
        private const string ProfilePermissionEmailRenew = "EMAILRENEWPERMISSION";
        private const string ProfilePermissionText = "TEXTPERMISSION";

        private const string ProfileEmail = "EMAIL";
        private const string ProfilePhone = "PHONE";
        private const string ProfileFax = "FAX";

        private const string DateConditionDateRange = "DateRange";
        private const string DateConditionXDays = "XDays";
        private const string DateConditionYear = "Year";
        private const string DateConditionMonth = "Month";

        private const string ClickCriteria = "CLICK CRITERIA";
        private const string OpenCriteria = "OPEN CRITERIA";
        private const string VisitCriteria = "VISIT CRITERIA";
        
        private const string VisitActivity = "Visit Activity";
        private const string OpenActivity = "Open Activity";
        private const string ClickActivity = "Click Activity";
        private const string OpenEmailSentDate = "OPEN EMAIL SENT DATE";
        private const string ClickEmailSentDate = "CLICK EMAIL SENT DATE";
        
        private const string Country = "[COUNTRY]";
        private const string IgrpNo = "[IGRP_NO]";
        private const string Other = "[OTHER]";

        private const string Equal = "EQUAL";
        private const string Contains = "CONTAINS";
        private const string StartWith = "START WITH";
        private const string EndWith = "END WITH";
        private const string DoesNotContain = "DOES NOT CONTAIN";
        private const string Range = "RANGE";
        private const string IsEmpty = "IS EMPTY";
        private const string IsNotEmpty = "IS NOT EMPTY";

        private const string ExectedZipCodeRadiusMinus2 = "(s.Latitude >= 0.971014492753623 and s.Latitude <= 1.02898550724638 and s.Longitude >= 1.97100981774379 and s.Longitude <= 2.02898967027162 and (s.Latitude<=0.971014492753623 OR 1.02898550724638<=s.Latitude OR s.Longitude<=1.97100981774379 OR 2.02898967027162<=s.Longitude) and isnull(s.IsLatLonValid,0) = 1 );";
        private const string ExectedZipCodeRadius2 = "(s.Latitude >= 0.971014492753623 and s.Latitude <= 1.02898550724638 and s.Longitude >= 1.97100981774379 and s.Longitude <= 2.02898967027162 and (s.Latitude<=0.971014492753623 OR 1.02898550724638<=s.Latitude OR s.Longitude<=1.97100981774379 OR 2.02898967027162<=s.Longitude) and isnull(s.IsLatLonValid,0) = 1  and ( master.dbo.fn_CalcDistanceBetweenLocations(1, 2, s.Latitude, s.Longitude, 0) between  2 and 2));";

        private int today;

        [SetUp]
        public void Setup()
        {
            today = (int)(DateTime.Today - new DateTime(1900, 1, 1)).TotalDays;
        }
        
        private Object.FilterMVC GetActivityFilter(string name, string criteria, string condition, string values)
        {
            return new Object.FilterMVC
            {
                Fields = new List<Object.FilterDetails>
                {
                    new Object.FilterDetails
                    {
                        FilterType = Enums.FiltersType.Activity, 
                        Name = criteria, 
                        Values = "1"
                    },
                    new Object.FilterDetails
                    {
                        FilterType = Enums.FiltersType.Activity, 
                        Name = name, 
                        SearchCondition = condition, 
                        Values = values
                    }
                }
            };
        }

        private Object.FilterMVC GetQualificationDateFilter(string condition, string values)
        {
            return new Object.FilterMVC
            {
                Fields = new List<Object.FilterDetails>
                {
                    new Object.FilterDetails
                    {
                        FilterType = Enums.FiltersType.Activity, 
                        Name = "Open CRITERIA", 
                        Values = "0"
                    },
                    new Object.FilterDetails
                    {
                        FilterType = Enums.FiltersType.Adhoc, 
                        Name = "ADHOC", 
                        Group = "d|qdate", 
                        SearchCondition = condition, 
                        Values = values
                    }
                }
            };
        }

        private Object.FilterMVC GetProfileFilter(string name, ViewType viewType, string values)
        {
            return new Object.FilterMVC
            {
                ViewType = viewType,
                Fields = new List<Object.FilterDetails>
                {
                    new Object.FilterDetails
                    {
                        FilterType = FiltersType.Adhoc, 
                        Name = name, 
                        Values = values
                    }
                }
            };
        }

        private Object.FilterMVC GetProfileFilter(string name, string condition, string values)
        {
            return new Object.FilterMVC
            {
                Fields = new List<Object.FilterDetails>
                {
                    new Object.FilterDetails
                    {
                        FilterType = FiltersType.Adhoc, 
                        Name = name, 
                        SearchCondition = condition,
                        Values = values
                    }
                }
            };
        }

        private Object.FilterMVC GetDateFilter(string condition, string values, string group)
        {
            return new Object.FilterMVC
            {
                Fields = new List<Object.FilterDetails>
                {
                    new Object.FilterDetails
                    {
                        FilterType = FiltersType.Activity, 
                        Name = "Open CRITERIA", 
                        Values = "0"
                    },
                    new Object.FilterDetails
                    {
                        FilterType = FiltersType.Adhoc, 
                        Name = "ADHOC",
                        Group = group,
                        SearchCondition = condition, 
                        Values = values
                    }
                }
            };
        }

        private Object.FilterMVC GetAdhocFilter(string group, string condition, string values)
        {
            return new Object.FilterMVC
            {
                Fields = new List<Object.FilterDetails>
                {
                    new Object.FilterDetails
                    {
                        FilterType = FiltersType.Adhoc, 
                        Name = "ADHOC", 
                        SearchCondition = condition, 
                        Values = values,
                        Group = group
                    }
                }
            };
        }
    }
}
