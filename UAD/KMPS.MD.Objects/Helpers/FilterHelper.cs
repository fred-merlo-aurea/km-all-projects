using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FrameworkUAD.BusinessLogic;
using KM.Common;
using KM.Common.Extensions;
using KMPlatform.Object;
using UadViewType = FrameworkUAD.BusinessLogic.Enums.ViewType;

namespace KMPS.MD.Objects.Helpers
{
    internal static class FilterHelper
    {
        private const string BlastSendTimeColumnName = "bla.SendTime";
        private const string ProfilePermissionMail = "MailPermission";
        private const string ProfilePermissionFax = "FaxPermission";
        private const string ProfilePermissionPhone = "PhonePermission";
        private const string ProfilePermissionOtherProducts = "OtherProductsPermission";
        private const string ProfilePermissionThirdParty = "ThirdPartyPermission";
        private const string ProfilePermissionEmailRenew = "EmailRenewPermission";
        private const string ProfilePermissionText = "TextPermission";
        private const string SearchAll = "Search All";
        private const string SearchSelectedProducts = "Search Selected Products";
        private const char CommaChar = ',';
        private const char VBarChar = '|';
        private const char ClickChar = 'c';
        private const string VBarString = "|";
        private const string NameProduct = "PRODUCT";
        private const string NameBrand = "BRAND";
        private const string NameDataCompare = "DATACOMPARE";
        private const string NameState = "STATE";
        private const string NameCountry = "COUNTRY";
        private const string NameEmail = "EMAIL";
        private const string NamePhone = "PHONE";
        private const string NameFax = "FAX";
        private const string NameGeoLocated = "GEOLOCATED";
        private const string NameMedia = "MEDIA";
        private const string NameQFrom = "QFROM";
        private const string NameQTo = "QTO";
        private const string NameEmailStatus = "EMAIL STATUS";
        private const string NameAdhoc = "ADHOC";
        private const string NameZipCodeRadius = "ZIPCODE-RADIUS";
        private const string NameLastName = "LAST NAME";
        private const string NameFirstName = "FIRST NAME";
        private const string NameCompany = "COMPANY";
        private const string NamePhoneNo = "PHONENO";
        private const string NameEmailId = "EMAILID";
        private const string NameMatch = "Match";
        private const string AdhocFlagM = "m";
        private const string AdhocFlagD = "d";
        private const string AdhocFlagI = "i";
        private const string AdhocFlagF = "f";
        private const string AdhocFlagE = "e";
        private const string AdhocFlagB = "b";
        private const string ConditionEqual = "EQUAL";
        private const string ConditionContains = "CONTAINS";
        private const string ConditionDoesNotContain = "DOES NOT CONTAIN";
        private const string ConditionStartWith = "START WITH";
        private const string ConditionEndWith = "END WITH";
        private const string ConditionIsEmpty = "IS EMPTY";
        private const string ConditionIsNotEmpty = "IS NOT EMPTY";
        private const string ConditionGreater = "GREATER";
        private const string ConditionLesser = "LESSER";
        private const string ConditionRange = "RANGE";
        private const string NameQDate = "qdate";
        private const string NameQualificationDate = "qualificationdate";
        private const string NameTransactionDate = "[transactiondate]";
        private const string NamePubTransactionDate = "[pubtransactiondate]";
        private const string NameStatusUpdatedDate = "[statusupdateddate]";
        private const string NameDateCreated = "datecreated";
        private const string NameDateUpdated = "dateupdated";
        private const string NameProductCount = "[PRODUCT COUNT]";
        private const string NameScore = "[SCORE]";
        private const string GroupNameCountry = "[COUNTRY]";
        private const string GroupNameGrpNo = "[IGRP_NO]";
        private const string NameOpenCriteria = "OPEN CRITERIA";
        private const string NameOpenActivity = "OPEN ACTIVITY";
        private const string NameOpenBlastId = "OPEN BLASTID";
        private const string NameOpenCampaign = "OPEN CAMPAIGNS";
        private const string NameOpenEmailSubject = "OPEN EMAIL SUBJECT";
        private const string NameOpenEmailSentDate = "OPEN EMAIL SENT DATE";
        private const string NameClickCriteria = "CLICK CRITERIA";
        private const string NameLink = "LINK";
        private const string NameClickActivity = "CLICK ACTIVITY";
        private const string NameClickBlastId = "CLICK BLASTID";
        private const string NameClickCampaign = "CLICK CAMPAIGNS";
        private const string NameClickEmailSubject = "CLICK EMAIL SUBJECT";
        private const string NameClickEmailSentDate = "CLICK EMAIL SENT DATE";
        private const string NameDomainTracking = "DOMAIN TRACKING";
        private const string NameUrl = "URL";
        private const string NameVisitCriteria = "VISIT CRITERIA";
        private const string NameVisitActivity = "VISIT ACTIVITY";
        private const string SubscriberClickActivity = "SubscriberClickActivity";
        private const string ClickActivityId = "ClickActivityID";
        private const string NameCategoryType = "CATEGORY TYPE";
        private const string NameCategoryCode = "CATEGORY CODE";
        private const string NameXAct = "XACT";
        private const string NameTransactionCode = "TRANSACTION CODE";
        private const string NameQSourceType = "QSOURCE TYPE";
        private const string NameQSourceCode = "QSOURCE CODE";
        private const string NameQualificationYear = "QUALIFICATION YEAR";
        private const string NameWaveMailing = "WAVE MAILING";

        public static void UpdateProfileFields(FilterArgs args, ClientConnections clientConnection)
        {
            Guard.NotNull(args, nameof(args));

            var profileFields = args.Filter.Fields
                .Where(f => f.FilterType != Enums.FiltersType.Dimension && f.FilterType != Enums.FiltersType.Activity)
                .ToList();

            foreach (var field in profileFields)
            {
                if (!args.Where.IsNullOrWhiteSpace())
                {
                    args.Where += " and ";
                }

                if (field.FilterType == Enums.FiltersType.Circulation)
                {
                    UpdateProfileFieldForCirculation(args, profileFields, field, clientConnection);
                }
                else
                {
                    UpdateProfileFieldForNonCirculation(args, field);
                }
            }
        }

        public static void UpdateDimensionFields(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));

            // Loops through all of the Dimensions'
            // the PubIds go into the sub_query list as it limits the number of rows we need to get
            // all the other business/function/industry/etc fields.

            args.AddedMasterId = false;
            args.DimQuery = string.Empty;
            string dimQueryStart;

            if (IsProductViewType(args.Filter.ViewType))
            {
                dimQueryStart = "select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) join pubsubscriptions ps1  with (nolock) on sfilter.subscriptionID = ps1.subscriptionID join PubSubscriptionDetail psd  with (nolock) on ps1.pubsubscriptionID = psd.pubsubscriptionID  where ";
            }
            else if (args.Filter.ViewType == Enums.ViewType.RecencyView)
            {
                dimQueryStart = args.Filter.BrandID > 0
                    ? " select distinct vrbc.subscriptionid from vw_RecentBrandConsensus vrbc with (nolock) where "
                    : "select distinct vrc.SubscriptionID from vw_RecentConsensus vrc with (nolock) where ";
            }
            else
            {
                dimQueryStart = args.Filter.BrandID > 0
                    ? " select distinct vbc.subscriptionid from vw_BrandConsensus vbc with (nolock) where "
                    : "select distinct sd.SubscriptionID from SubscriptionDetails sd  with (nolock) where ";
            }

            UpdateDimensionQuery(args, dimQueryStart);
        }

        public static void UpdateActivityFields(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));

            UpdateActivityByFieldNames(args);
            UpdateActivityByPubIds(args);

            UpdateActivityForOpen(args);
            UpdateActivityForClick(args);
            UpdateActivityForVisit(args);
        }

        public static void UpdateTempTables(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));

            // If we have added master_ids we need to select based on the cgrp or igrp entities that the bubble_up_query selects for us
            // Otherwise we can just add the sub query and it will select things ignoring their survey responses
            if (args.AddedMasterId)
            {
                args.CreateTempTableQuery += " Create table #tempDimSub (subscriptionid int);";
                args.CreateTempTableQuery += " CREATE UNIQUE CLUSTERED INDEX tempDimSub_1 on #tempDimSub  (subscriptionid); ";
                args.CreateTempTableQuery += " Insert into #tempDimSub " + args.DimQuery + ";";
                args.DropTempTableQuery += " drop table #tempDimSub; ";
            }

            UpdateTempTablesOpen(args);
            UpdateTempTablesClick(args);
            UpdateTempTablesVisit(args);

            if (args.AdditionalFilters.Length > 0)
            {
                args.Query += args.AdditionalFilters;
            }

            if (args.JoinBlastForOpen)
            {
                args.DropTempTableQuery += "drop table #tempOblast; ";
            }

            if (args.JoinBlastForClick)
            {
                args.DropTempTableQuery += "drop table #tempCblast; ";
            }
        }

        public static void UpdateOuterQuery(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));
            if (args.AddedMasterId)
            {
                args.Query = $"select {args.SelectList} from #tempDimSub x4  with (nolock) join subscriptions s with (nolock) on x4.SubscriptionID = s.SubscriptionID join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID {args.Query}";
            }
            else
            {
                args.Query = $"select {args.SelectList} from subscriptions s with (nolock) join pubsubscriptions ps  with (nolock)  on s.subscriptionID = ps.subscriptionID {args.Query}";
            }
        }

        private static void UpdateActivityByFieldNames(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));

            foreach (var field in args.Filter.Fields.Where(f => f.FilterType == Enums.FiltersType.Activity))
            {
                UpdateActivityByFieldNameOpen(args, field);
                UpdateActivityByFieldNameClick(args, field);
                UpdateActivityByFieldNameVisit(args, field);
            }
        }

        private static void UpdateActivityByFieldNameOpen(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));

            if (field.Name.EqualsIgnoreCase(NameOpenCriteria))
            {
                args.OpenSearchType = field.SearchCondition;
                args.OpenCount = Convert.ToInt32(field.Values);
            }
            else if (field.Name.EqualsIgnoreCase(NameOpenActivity))
            {
                FilterMVC.AppendDateNumberCondition(args.OpenCondition, field.SearchCondition, field.Values);
            }
            else if (field.Name.EqualsIgnoreCase(NameOpenBlastId))
            {
                args.JoinBlastForOpen = true;
                args.OpenBlastCondition.AppendFormat(args.OpenBlastCondition.Length > 0
                        ? " and bla.BlastID in ({0})"
                        : " where bla.BlastID in ({0})",
                    field.Values);
            }
            else if (field.Name.EqualsIgnoreCase(NameOpenCampaign))
            {
                args.JoinBlastForOpen = true;
                args.OpenBlastCondition.AppendFormat(args.OpenBlastCondition.Length > 0
                        ? " and bla.ECNCampaignID in ({0})"
                        : " where bla.ECNCampaignID in ({0})",
                    field.Values);
            }
            else if (field.Name.EqualsIgnoreCase(NameOpenEmailSubject))
            {
                args.JoinBlastForOpen = true;
                args.OpenBlastCondition.AppendFormat(args.OpenBlastCondition.Length > 0
                        ? " and bla.Emailsubject like '%{0}%'"
                        : " Where bla.Emailsubject like '%{0}%'",
                    Utilities.ReplaceSingleQuotes(field.Values.Trim()).Replace("_", "[_]"));
            }
            else if (field.Name.EqualsIgnoreCase(NameOpenEmailSentDate))
            {
                args.JoinBlastForOpen = true;
                FilterMVC.AppendDateCondition(args.OpenBlastCondition, BlastSendTimeColumnName, field.SearchCondition, field.Values);
            }
        }

        private static void UpdateActivityByFieldNameClick(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));

            if (field.Name.EqualsIgnoreCase(NameClickCriteria))
            {
                args.ClickSearchType = field.SearchCondition;
                args.ClickCount = Convert.ToInt32(field.Values);
            }
            else if (field.Name.EqualsIgnoreCase(NameLink))
            {
                args.Link = field.Values;
            }
            else if (field.Name.EqualsIgnoreCase(NameClickActivity))
            {
                FilterMVC.AppendDateNumberCondition(args.ClickCondition, field.SearchCondition, field.Values);
            }
            else if (field.Name.EqualsIgnoreCase(NameClickBlastId))
            {
                args.JoinBlastForClick = true;
                args.ClickBlastCondition.AppendFormat(args.ClickBlastCondition.Length > 0
                        ? " and bla.BlastID in ({0})"
                        : " where bla.BlastID in ({0})",
                    field.Values);
            }
            else if (field.Name.EqualsIgnoreCase(NameClickCampaign))
            {
                args.JoinBlastForClick = true;
                args.ClickBlastCondition.AppendFormat(args.ClickBlastCondition.Length > 0
                        ? " and bla.ECNCampaignID in ({0})"
                        : " where bla.ECNCampaignID in ({0})",
                    field.Values);
            }
            else if (field.Name.EqualsIgnoreCase(NameClickEmailSubject))
            {
                args.JoinBlastForClick = true;
                args.ClickBlastCondition.AppendFormat(args.ClickBlastCondition.Length > 0
                        ? " and bla.Emailsubject like '%{0}%'"
                        : " Where bla.Emailsubject like '%{0}%'",
                    Utilities.ReplaceSingleQuotes(field.Values.Trim()).Replace("_", "[_]"));
            }
            else if (field.Name.EqualsIgnoreCase(NameClickEmailSentDate))
            {
                args.JoinBlastForClick = true;
                FilterMVC.AppendDateCondition(
                    args.ClickBlastCondition,
                    BlastSendTimeColumnName,
                    field.SearchCondition,
                    field.Values);
            }
            else if (field.Name.EqualsIgnoreCase(NameDomainTracking))
            {
                args.DomainTrackingId = field.Values;
            }
            else if (field.Name.EqualsIgnoreCase(NameUrl))
            {
                args.Url = field.Values;
            }
        }

        private static void UpdateActivityByFieldNameVisit(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));

            if (field.Name.EqualsIgnoreCase(NameVisitCriteria))
            {
                int count;
                int.TryParse(field.Values, out count);
                args.VisitCount = count;
            }
            else if (field.Name.EqualsIgnoreCase(NameVisitActivity))
            {
                FilterMVC.AppendDateNumberCondition(args.VisitCondition, field.SearchCondition, field.Values);
            }
        }

        private static void UpdateActivityByPubIds(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));
            if (args.PubIds.Length > 0)
            {
                if (SearchSelectedProducts.EqualsIgnoreCase(args.OpenSearchType) || IsProductViewType(args.Filter.ViewType))
                {
                    args.OpenCondition.AppendFormat(args.OpenCondition.Length > 0
                            ? " and pso.pubID in ({0})"
                            : " where pso.pubID in ({0})",
                        args.PubIds);
                }

                if (SearchSelectedProducts.EqualsIgnoreCase(args.ClickSearchType) || IsProductViewType(args.Filter.ViewType))
                {
                    args.ClickCondition.AppendFormat(args.ClickCondition.Length > 0
                            ? " and psc.pubID in ({0})"
                            : " where psc.pubID in ({0})",
                        args.PubIds);
                }
            }
        }

        private static void UpdateActivityForOpen(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));
            if (args.OpenCount < 0)
            {
                return;
            }

            if (SearchAll.EqualsIgnoreCase(args.OpenSearchType) && !IsProductViewType(args.Filter.ViewType))
            {
                if (args.Filter.BrandID > 0)
                {
                    //brand
                    args.OpenQuery += "select x.subscriptionID from (";
                    args.OpenQuery += " select so.SubscriptionID, openactivityID from SubscriberOpenActivity so with (NOLOCK) " +
                                      " join PubSubscriptions pso  with (NOLOCK) on so.PubSubscriptionID = pso.PubSubscriptionID ";

                    if (args.JoinBlastForOpen)
                    {
                        args.OpenQuery += " join #tempOblast tob  with (NOLOCK) on so.blastID = tob.blastID ";
                    }

                    args.OpenQuery += args.OpenCondition;
                    args.OpenQuery += (args.OpenCondition.Length > 0 ? " and " : " where ");
                    args.OpenQuery += " pubID in (select PubID from BrandDetails bd  with (nolock) join Brand b with (nolock) on bd.BrandID = b.BrandID where bd.BrandID in (" + args.Filter.BrandID + ") and  b.Isdeleted = 0) ";

                    // ADD UNION
                    args.OpenQuery += " union select so1.SubscriptionID, so1.openactivityID from SubscriberOpenActivity so1 with (NOLOCK) ";

                    if (args.JoinBlastForOpen)
                    {
                        args.OpenQuery += " join #tempOblast tob  with (NOLOCK) on so1.blastID = tob.blastID ";
                    }

                    args.OpenQuery += args.OpenCondition;
                    args.OpenQuery += (args.OpenCondition.Length > 0 ? " and " : " where ");
                    args.OpenQuery += " so1.pubsubscriptionid IS NULL ";

                    args.OpenQuery += " ) x GROUP  BY x.subscriptionid ";
                    args.OpenQuery += (args.OpenCount > 0 ? " HAVING Count(x.openactivityid) >= " + args.OpenCount : " ");
                }
                else
                {
                    //consensus
                    args.OpenQuery += " select so.SubscriptionID from SubscriberOpenActivity so  with (NOLOCK) ";

                    if (args.JoinBlastForOpen)
                    {
                        args.OpenQuery += " join #tempOblast tob  with (NOLOCK) on so.blastID = tob.blastID ";
                    }

                    args.OpenQuery += args.OpenCondition;
                    args.OpenQuery += " group by so.SubscriptionID";
                    args.OpenQuery += args.OpenCount > 0
                        ? $" HAVING Count(so.openactivityid) >= {args.OpenCount}"
                        : " ";
                }
            }
            else
            {
                args.OpenQuery = CreateQueryWithBrand(
                    args.Filter,
                    args.OpenCondition,
                    args.OpenCount,
                    args.JoinBlastForOpen,
                    'o',
                    "SubscriberOpenActivity",
                    "openactivityid");
            }
        }

        private static void UpdateActivityForClick(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));

            UpdateActivityForClickLink(args);

            if (args.ClickCount >= 0)
            {
                if (SearchAll.EqualsIgnoreCase(args.ClickSearchType) && !IsProductViewType(args.Filter.ViewType))
                {
                    if (args.Filter.BrandID > 0)
                    {
                        args.ClickQuery = "select x.subscriptionID from (";
                        args.ClickQuery += " select sc.SubscriptionID, sc.ClickActivityID  from  SubscriberClickActivity sc  with (NOLOCK) " +
                                     " join PubSubscriptions psc  with (NOLOCK) on sc.PubSubscriptionID = psc.PubSubscriptionID ";

                        if (args.JoinBlastForClick)
                        {
                            args.ClickQuery += " join #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID ";
                        }

                        args.ClickQuery += args.ClickCondition;
                        args.ClickQuery += args.ClickCondition.Length > 0
                            ? " and "
                            : " where ";
                        args.ClickQuery += $" pubID in (select PubID from BrandDetails bd  with (nolock) join Brand b with (nolock) on bd.BrandID = b.BrandID where bd.BrandID in ({args.Filter.BrandID}) and  b.Isdeleted = 0)";

                        // ADD UNION
                        args.ClickQuery += "union select sc1.SubscriptionID, sc1.ClickActivityID  from  SubscriberClickActivity sc1  with (NOLOCK) ";

                        if (args.JoinBlastForClick)
                        {
                            args.ClickQuery += " join #tempCblast tcb with (NOLOCK) on sc1.blastID = tcb.blastID ";
                        }

                        args.ClickQuery += args.ClickCondition;
                        args.ClickQuery += (args.ClickCondition.Length > 0 ? " and " : " where ");
                        args.ClickQuery += " sc1.pubsubscriptionid IS NULL ";
                        args.ClickQuery += " ) x GROUP BY x.subscriptionid ";
                        args.ClickQuery += args.ClickCount > 0 ? $" HAVING Count(x.ClickActivityID) >= {args.ClickCount}" : " ";
                    }
                    else
                    {
                        args.ClickQuery = " select sc.SubscriptionID from SubscriberClickActivity sc  with (NOLOCK) ";

                        if (args.JoinBlastForClick)
                        {
                            args.ClickQuery += " join #tempCblast tcb with (NOLOCK) on sc.blastID = tcb.blastID ";
                        }

                        args.ClickQuery += args.ClickCondition;
                        args.ClickQuery += " group by sc.SubscriptionID";
                        args.ClickQuery += args.ClickCount > 0 ? $" HAVING Count(sc.ClickActivityID) >= {args.ClickCount}" : " ";
                    }
                }
                else
                {
                    args.ClickQuery = CreateQueryWithBrand(
                        args.Filter,
                        args.ClickCondition,
                        args.ClickCount,
                        args.JoinBlastForClick,
                        ClickChar,
                        SubscriberClickActivity,
                        ClickActivityId);
                }
            }
        }

        private static void UpdateActivityForClickLink(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));
            if (args.Link.Length > 0)
            {
                if (args.ClickCount < 0)
                {
                    args.ClickCount = 1;
                }

                var linkQuery = string.Empty;
                var links = args.Link.Split(CommaChar);

                linkQuery = links.Select(link => Utilities.ReplaceSingleQuotes(link.Trim()).Replace("_", "[_]"))
                    .Aggregate(linkQuery, (current, escaped) => current + (current.Length > 0
                                                                    ? $" or (Link like '%{escaped}%' or  LinkAlias like '%{escaped}%')"
                                                                    : $" (Link like'%{escaped}%' or LinkAlias like '%{escaped}%')"));

                args.ClickCondition.AppendFormat(args.ClickCondition.Length > 0
                        ? " and ({0})"
                        : " Where ({0})",
                    linkQuery);
            }
        }

        private static void UpdateActivityForVisit(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));
            if (args.VisitCount < 0)
            {
                return;
            }

            args.VisitQuery = " select sv.SubscriptionID from  SubscriberVisitActivity sv  with (NOLOCK) ";

            if (args.DomainTrackingId.Length > 0)
            {
                args.VisitCondition.Append(args.VisitCondition.Length > 0
                    ? $" and sv.DomainTrackingID = {args.DomainTrackingId}"
                    : $" where sv.DomainTrackingID = {args.DomainTrackingId}");
            }

            if (args.Url.Length > 0)
            {
                var strUrls = args.Url.Split(CommaChar);
                var urlQuery = string.Empty;

                foreach (var url in strUrls)
                {
                    var escaped = Utilities.ReplaceSingleQuotes(url.Trim()).Replace("_", "[_]");
                    urlQuery += urlQuery.Length > 0
                        ? $" or sv.url like '%{escaped}%'"
                        : $" sv.url like '%{escaped}%'";
                }

                args.VisitCondition.Append(args.VisitCondition.Length > 0
                    ? $" and ({urlQuery})"
                    : $" Where ({urlQuery})");
            }

            if (args.VisitCondition.Length > 0)
            {
                args.VisitQuery += args.VisitCondition;
            }

            args.VisitQuery += " group by sv.SubscriptionID";
            args.VisitQuery += args.VisitCount > 0
                ? $" having COUNT(sv.VisitActivityID) >= {args.VisitCount}"
                : " ";
        }

        private static void UpdateDimensionQuery(FilterArgs args, string dimQueryStart)
        {
            Guard.NotNull(args, nameof(args));
            foreach (var field in args.Filter.Fields.Where(f => f.FilterType == Enums.FiltersType.Dimension))
            {
                if (!args.DimQuery.IsNullOrWhiteSpace())
                {
                    args.DimQuery += " intersect ";
                }

                args.DimQuery += dimQueryStart;

                if (args.AddedMasterId)
                {
                    if (IsProductViewType(args.Filter.ViewType))
                    {
                        args.DimQuery += $" ps1.pubID = {args.Filter.PubID} and psd.codesheetID in ( {field.Values} ) ";
                    }
                    else if (args.Filter.ViewType == Enums.ViewType.RecencyView)
                    {
                        args.DimQuery += args.Filter.BrandID > 0
                            ? $" vrbc.brandid = {args.Filter.BrandID} and vrbc.masterid  in ( {field.Values} ) "
                            : $" vrc.masterid  in ( {field.Values} ) ";
                    }
                    else
                    {
                        args.DimQuery += args.Filter.BrandID > 0
                            ? $" vbc.brandid= {args.Filter.BrandID} and vbc.masterid  in ( {field.Values} ) "
                            : $" sd.masterid  in ( {field.Values} ) ";
                    }
                }
                else
                {
                    if (IsProductViewType(args.Filter.ViewType))
                    {
                        args.DimQuery += $" psd.codesheetID in ({field.Values} ) ";
                    }
                    else if (args.Filter.ViewType == Enums.ViewType.RecencyView)
                    {
                        args.DimQuery += args.Filter.BrandID > 0
                            ? $" vrbc.brandid= {args.Filter.BrandID} and vrbc.masterid in ({field.Values} ) "
                            : $" vrc.masterid in ({field.Values} ) ";
                    }
                    else
                    {
                        args.DimQuery += args.Filter.BrandID > 0
                            ? $" vbc.brandid= {args.Filter.BrandID} and vbc.masterid in ({field.Values} ) "
                            : $" sd.masterid in ({field.Values} ) ";
                    }
                }
                args.AddedMasterId = true;
            }
        }

        private static void UpdateProfileFieldForCirculation(
            FilterArgs args,
            IEnumerable<Field> profileFields,
            Field field,
            ClientConnections clientConnection)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(profileFields, nameof(profileFields));
            Guard.NotNull(field, nameof(field));

            if (field.Name.EqualsIgnoreCase(NameCategoryType))
            {
                args.Where += $"ps.PubCategoryID in (select CategoryCodeID from UAD_Lookup..CategoryCode with (nolock) where CategoryCodeTypeID in ( {field.Values} ) )";
            }
            else if (field.Name.EqualsIgnoreCase(NameCategoryCode))
            {
                args.Where += $"ps.PubCategoryID in ({field.Values}) ";
            }
            else if (field.Name.EqualsIgnoreCase(NameXAct))
            {
                args.Where += $"ps.PubTransactionID in (select TransactionCodeID from UAD_Lookup..TransactionCode with (nolock) where TransactionCodeTypeID in ( {field.Values} ) )";
            }
            else if (field.Name.EqualsIgnoreCase(NameTransactionCode))
            {
                args.Where += $"ps.PubTransactionID in (  {field.Values}) ";
            }
            else if (field.Name.EqualsIgnoreCase(NameQSourceType))
            {
                args.Where += $"ps.PubQSourceID in (select CodeID from UAD_Lookup..Code with (nolock) where ParentCodeId in ( {field.Values} ) )";
            }
            else if (field.Name.EqualsIgnoreCase(NameQSourceCode))
            {
                args.Where += $"ps.PubQSourceID in ({field.Values}) ";
            }
            else if (field.Name.EqualsIgnoreCase(NameMedia))
            {
                args.Where += $"ps.Demo7 in ('{field.Values.Replace(",", "','")}') ";
            }
            else if (field.Name.EqualsIgnoreCase(NameQualificationYear))
            {
                UpdateProfileQualificationYear(args, profileFields, field, clientConnection);
            }
            else if (field.Name.EqualsIgnoreCase(NameQFrom))
            {
                args.Where += $" ps.QualificationDate >= '{field.Values}' ";
            }
            else if (field.Name.EqualsIgnoreCase(NameQTo))
            {
                args.Where += $" ps.QualificationDate <= '{field.Values} 23:59:59' ";
            }
            else if (field.Name.EqualsIgnoreCase(NameWaveMailing))
            {
                args.Where += $" ISNULL(ps.IsInActiveWaveMailing,0) = {field.Values}";
            }
        }

        private static void UpdateProfileQualificationYear(
            FilterArgs args,
            IEnumerable<Field> profileFields,
            Field field,
            ClientConnections clientConnection)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(profileFields, nameof(profileFields));
            Guard.NotNull(field, nameof(field));
            var strYear = field.Values.Split(CommaChar);
            args.Where += "(";

            var pubId = Convert.ToInt32(profileFields.First(x => NameProduct.EqualsIgnoreCase(x.Name)).Values);
            DateTime startDate;
            DateTime endDate;

            var pubs = Pubs.GetByID(clientConnection, pubId);

            if (!string.IsNullOrWhiteSpace(pubs.YearStartDate) && !string.IsNullOrWhiteSpace(pubs.YearEndDate))
            {
                int currentYear;
                if (DateTime.Now > Convert.ToDateTime($"{pubs.YearStartDate}/{DateTime.Now.Year}", CultureInfo.InvariantCulture))
                {
                    currentYear = DateTime.Now.Year;
                }
                else
                {
                    currentYear = DateTime.Now.Year - 1;
                }

                startDate = Convert.ToDateTime($"{pubs.YearStartDate}/{currentYear}", CultureInfo.InvariantCulture);
                endDate = Convert.ToDateTime($"{pubs.YearEndDate}/{(currentYear + 1)}", CultureInfo.InvariantCulture);
            }
            else
            {
                startDate = DateTime.Now;
                endDate = startDate.AddYears(-1).AddDays(1);
            }

            for (var i = 0; i < strYear.Length; i++)
            {
                int year;
                if (!int.TryParse(strYear[i], out year))
                {
                    throw new InvalidOperationException($"Cannot parse '{strYear[i]}' to year.");
                }

                year--;
                args.Where += (i > 0 ? " OR " : string.Empty) + " ps.QualificationDate between convert(varchar(20), DATEADD(year, -" +
                              year + ", '" + startDate.ToShortDateString() +
                              "'),111)  and  convert(varchar(20), DATEADD(year, -" + year + ", '" +
                              endDate.ToShortDateString() + "'),111) + ' 23:59:59'";
            }

            args.Where += ")";
        }

        private static void UpdateProfileFieldForNonCirculation(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            if (field.Name.EqualsIgnoreCase(NameBrand))
            {
                UpdateProfileBrand(args, field);
            }
            else if (field.Name.EqualsIgnoreCase(NameProduct))
            {
                UpdateProfileProduct(args, field);
            }
            else if (field.Name.EqualsIgnoreCase(NameDataCompare))
            {
                UpdateProfileDataCompare(args, field);
            }
            else if (field.Name.EqualsIgnoreCase(NameState))
            {
                UpdateProfileState(args, field);
            }
            else if (field.Name.EqualsIgnoreCase(NameCountry))
            {
                UpdateProfileCountry(args, field);
            }
            else if (field.Name.EqualsIgnoreCase(NameEmail))
            {
                UpdateProfileEmail(args, field);
            }
            else if (field.Name.EqualsIgnoreCase(NamePhone))
            {
                UpdateProfilePhone(args, field);
            }
            else if (field.Name.EqualsIgnoreCase(NameFax))
            {
                UpdateProfileFax(args, field);
            }
            else if (field.Name.EqualsIgnoreCase(NameGeoLocated))
            {
                UpdateProfileGeoLocated(args, field);
            }
            else if (field.Name.EqualsIgnoreCase(ProfilePermissionMail))
            {
                args.Where += FilterMVC.CreatePermissionCondition(ProfilePermissionMail, field.Values, (UadViewType)args.Filter.ViewType);
            }
            else if (field.Name.EqualsIgnoreCase(ProfilePermissionFax))
            {
                args.Where += FilterMVC.CreatePermissionCondition(ProfilePermissionFax, field.Values, (UadViewType)args.Filter.ViewType);
            }
            else if (field.Name.EqualsIgnoreCase(ProfilePermissionPhone))
            {
                args.Where += FilterMVC.CreatePermissionCondition(ProfilePermissionPhone, field.Values, (UadViewType)args.Filter.ViewType);
            }
            else if (field.Name.EqualsIgnoreCase(ProfilePermissionOtherProducts))
            {
                args.Where += FilterMVC.CreatePermissionCondition(ProfilePermissionOtherProducts, field.Values, (UadViewType)args.Filter.ViewType);
            }
            else if (field.Name.EqualsIgnoreCase(ProfilePermissionThirdParty))
            {
                args.Where += FilterMVC.CreatePermissionCondition(ProfilePermissionThirdParty, field.Values, (UadViewType)args.Filter.ViewType);
            }
            else if (field.Name.EqualsIgnoreCase(ProfilePermissionEmailRenew))
            {
                args.Where += FilterMVC.CreatePermissionCondition(ProfilePermissionEmailRenew, field.Values, (UadViewType)args.Filter.ViewType);
            }
            else if (field.Name.EqualsIgnoreCase(ProfilePermissionText))
            {
                args.Where += FilterMVC.CreatePermissionCondition(ProfilePermissionText, field.Values, (UadViewType)args.Filter.ViewType);
            }
            else if (field.Name.EqualsIgnoreCase(NameMedia))
            {
                args.Where += "ps.Demo7 in (  '" + field.Values.Replace(",", "','") + "') ";
            }
            else if (field.Name.EqualsIgnoreCase(NameQFrom))
            {
                args.Where += " ps.QualificationDate >= '" + field.Values + "' ";
            }
            else if (field.Name.EqualsIgnoreCase(NameQTo))
            {
                args.Where += " ps.QualificationDate <= '" + field.Values + " 23:59:59' ";
            }
            else if (field.Name.EqualsIgnoreCase(NameEmailStatus))
            {
                args.Where += "ps.EmailStatusID in (" + field.Values + ") ";
            }
            else if (field.Name.EqualsIgnoreCase(NameAdhoc))
            {
                UpdateProfileAdhoc(args, field);
            }
            else if (field.Name.EqualsIgnoreCase(NameZipCodeRadius))
            {
                args.Where += FilterMVC.CreateZipCodeRadiusCondition(field.SearchCondition);
            }
            else if (field.Name.EqualsIgnoreCase(NameLastName))
            {
                args.Where += "PATINDEX('" + Utilities.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', s.lname ) > 0 ";
            }
            else if (field.Name.EqualsIgnoreCase(NameFirstName))
            {
                args.Where += "PATINDEX('" + Utilities.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', s.fname ) > 0 ";
            }
            else if (field.Name.EqualsIgnoreCase(NameCompany))
            {
                args.Where += "PATINDEX('" + Utilities.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', s.company ) > 0 ";
            }
            else if (field.Name.EqualsIgnoreCase(NamePhoneNo))
            {
                args.Where += "PATINDEX('" + Utilities.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', s.phone ) > 0 ";
            }
            else if (field.Name.EqualsIgnoreCase(NameEmailId))
            {
                args.Where += "PATINDEX('" + Utilities.ReplaceSingleQuotes(field.Values).Replace("_", "[_]") + "%', ps.email ) > 0 ";
            }
        }

        private static void UpdateProfileBrand(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            if (args.Filter.ViewType != Enums.ViewType.ProductView && args.Filter.ViewType != Enums.ViewType.CrossProductView)
            {
                args.Query += " join branddetails bd  with (nolock)  on bd.pubID = ps.pubID  join brand b with (nolock) on b.brandID = bd.brandID ";
                args.Where += "b.IsDeleted = 0 and bd.BrandID = " + field.Values;
            }
        }

        private static void UpdateProfileProduct(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            args.PubIds = field.Values;
            args.Where += $"ps.pubid in ({field.Values} ) ";
        }

        private static void UpdateProfileDataCompare(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            var strDataCompare = field.Values.Split(VBarChar);

            if (strDataCompare.Length < 2)
            {
                throw new ArgumentException($"Invalid field value '{field.Values}'");
            }

            int compareId;
            if (!int.TryParse(strDataCompare[1], out compareId))
            {
                throw new InvalidOperationException($"Cannot parse '{strDataCompare[1]}' to int.");
            }
            var dataCompareType = Code.GetDataCompareType()
                .Find(x => x.CodeID == compareId)
                .CodeName;

            if (NameMatch.EqualsIgnoreCase(dataCompareType))
            {
                args.Query += " join DataCompareProfile d with(nolock) on s.IGRP_NO = d.IGrp_No ";
                args.Where += "d.ProcessCode = '" + strDataCompare[0] + "'";
            }
            else
            {
                args.Query += " left outer join DataCompareProfile d with(nolock) on s.IGRP_NO = d.IGrp_No and  d.ProcessCode = '" +
                              strDataCompare[0] + "'";
                args.Where += "d.SubscriberFinalId is null ";
            }
        }

        private static void UpdateProfileState(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));

            var state = field.Values.Replace(",", "','");
            if (args.Filter.ViewType != Enums.ViewType.ProductView && args.Filter.ViewType != Enums.ViewType.CrossProductView)
            {
                args.Where += $"s.State in ('{state}') ";
            }
            else
            {
                args.Where += $"ps.RegionCode in ('{state}') ";
            }
        }

        private static void UpdateProfileCountry(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            var strCountryId = field.Values.Split(CommaChar);
            args.Where += "(";

            if (args.Filter.ViewType != Enums.ViewType.ProductView && args.Filter.ViewType != Enums.ViewType.CrossProductView)
            {
                for (var i = 0; i < strCountryId.Length; i++)
                {
                    if (strCountryId[i] == "1")
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}s.CountryID = 1";
                    }
                    else if (strCountryId[i] == "3")
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}((s.CountryID = 1) or (s.CountryID = 2)) ";
                    }
                    else if (strCountryId[i] == "4")
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}not ((s.CountryID = 1) or (s.CountryID = 2) or ISNULL(s.CountryID,0) = 0)";
                    }
                    else
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}s.CountryID in ( {strCountryId[i]} ) ";
                    }
                }
            }
            else
            {
                for (var i = 0; i < strCountryId.Length; i++)
                {
                    if (strCountryId[i] == "1")
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}ps.CountryID = 1";
                    }
                    else if (strCountryId[i] == "3")
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}((ps.CountryID = 1) or (ps.CountryID = 2)) ";
                    }
                    else if (strCountryId[i] == "4")
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}not ((ps.CountryID = 1) or (ps.CountryID = 2) or ISNULL(ps.CountryID,0) = 0)";
                    }
                    else
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}ps.CountryID in ( {strCountryId[i]} ) ";
                    }
                }
            }

            args.Where += ")";
        }

        private static void UpdateProfileEmail(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            var emails = field.Values.Split(CommaChar);
            args.Where += "(";

            if (args.Filter.ViewType == Enums.ViewType.ProductView || args.Filter.ViewType == Enums.ViewType.CrossProductView)
            {
                for (var i = 0; i < emails.Length; i++)
                {
                    if (emails[i] == "1")
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}isnull(ps.Email, '') != '' ";
                    }
                    else
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}isnull(ps.Email, '') = '' ";
                    }
                }
            }
            else
            {
                for (var i = 0; i < emails.Length; i++)
                {
                    args.Where += $"{(i > 0 ? " OR " : string.Empty)}s.emailexists = {emails[i]}";
                }
            }

            args.Where += ")";
        }

        private static void UpdateProfilePhone(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            var phones = field.Values.Split(CommaChar);
            args.Where += "(";

            if (args.Filter.ViewType == Enums.ViewType.ProductView || args.Filter.ViewType == Enums.ViewType.CrossProductView)
            {
                for (var i = 0; i < phones.Length; i++)
                {
                    if (phones[i] == "1")
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}isnull(ps.Phone, '') != '' ";
                    }
                    else
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}isnull(ps.Phone, '') = '' ";
                    }
                }
            }
            else
            {
                for (var i = 0; i < phones.Length; i++)
                {
                    args.Where += $"{(i > 0 ? " OR " : string.Empty)}s.phoneexists = {phones[i]}";
                }
            }

            args.Where += ")";
        }

        private static void UpdateProfileFax(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            var faxes = field.Values.Split(CommaChar);
            args.Where += "(";

            if (args.Filter.ViewType == Enums.ViewType.ProductView || args.Filter.ViewType == Enums.ViewType.CrossProductView)
            {
                for (var i = 0; i < faxes.Length; i++)
                {
                    if (faxes[i] == "1")
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}isnull(ps.fax, '') != '' ";
                    }
                    else
                    {
                        args.Where += $"{(i > 0 ? " OR " : string.Empty)}isnull(ps.fax, '') = '' ";
                    }
                }
            }
            else
            {
                for (var i = 0; i < faxes.Length; i++)
                {
                    args.Where += $"{(i > 0 ? " OR " : string.Empty)}s.faxexists = {faxes[i]}";
                }
            }

            args.Where += ")";
        }

        private static void UpdateProfileGeoLocated(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            var isLatLonValidValues = field.Values.Split(CommaChar);

            args.Where += "(";

            for (int i = 0; i < isLatLonValidValues.Length; i++)
            {
                args.Where += $"{(i > 0 ? " OR " : string.Empty)}s.IsLatLonValid = {isLatLonValidValues[i]}";
            }

            args.Where += ")";
        }

        private static void UpdateProfileAdhoc(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            if (field.Group.Contains(VBarString))
            {
                var groupArray = field.Group.Split(VBarChar);
                if (groupArray[0] == AdhocFlagM)
                {
                    UpdateProfileAdhocMaster(args, field, groupArray[1]);
                }
                else if (groupArray[0] == AdhocFlagD)
                {
                    UpdateProfileAdhocDate(args, field, groupArray[1]);
                }
                else if (groupArray[0] == AdhocFlagI || groupArray[0] == AdhocFlagF)
                {
                    UpdateProfileAdhocCountOrScore(args, field, groupArray[1]);
                }
                else if (groupArray[0] == AdhocFlagE)
                {
                    UpdateProfileAdhocPubId(args, field);

                    var columnName = string.Concat("E.", groupArray[1]);
                    var typeFlag = groupArray[2];

                    if (typeFlag.EqualsAnyIgnoreCase(AdhocFlagI, AdhocFlagF))
                    {
                        UpdateProfileAdhocCompareNumber(args, field, columnName, groupArray[2]);
                    }
                    else if (typeFlag.EqualsIgnoreCase(AdhocFlagB))
                    {
                        args.Where += $"CAST({columnName} AS BIT) = {field.Values})";
                    }
                    else if (typeFlag.EqualsIgnoreCase(AdhocFlagD))
                    {
                        columnName = string.Format("case when IsDate({0}) = 1 then CAST({0} AS DATETIME) else null end", columnName);
                        var builder = new StringBuilder();
                        FilterMVC.AppendDateCondition(builder, columnName, field.SearchCondition, field.Values);
                        args.Where += builder.ToString();
                    }
                    else
                    {
                        UpdateProfileAdhocSearchString(args, field, columnName);
                    }
                }
            }
            else
            {
                UpdateProfileAdhocSingleGroup(args, field);
            }
        }

        private static void UpdateProfileAdhocMaster(FilterArgs args, Field field, string masterGroupId)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            string queryIsEmpty;
            string queryNotNull;
            string queryIsNotEmpty;

            if (IsProductViewType(args.Filter.ViewType))
            {
                UpdateProfileAdhocMasterForProductView(args, masterGroupId, out queryIsEmpty, out queryNotNull, out queryIsNotEmpty);
            }
            else
            {
                UpdateProfileAdhocMasterForNonProductView(args, masterGroupId, out queryIsEmpty, out queryNotNull, out queryIsNotEmpty);
            }

            UpdateProfileAdhocMasterCondition(args, field, queryIsEmpty, queryNotNull, queryIsNotEmpty);

            var adhocValues = field.Values.Split(CommaChar);

            for (var i = 0; i < adhocValues.Length; i++)
            {
                var itemValue = Utilities.ReplaceSingleQuotes(adhocValues[i].Trim());
                var itemValueUnderScore = itemValue.Replace("_", "[_]");

                if (field.SearchCondition.EqualsIgnoreCase(ConditionEqual))
                {
                    args.Where += $"{(i > 0 ? " OR " : string.Empty)} (ms.MasterDesc = '{itemValue}' or ms.Mastervalue = '{itemValue}') ";
                }
                else if (field.SearchCondition.EqualsAnyIgnoreCase(ConditionContains, ConditionDoesNotContain))
                {
                    args.Where += $"{(i > 0 ? " OR " : string.Empty)} (ms.MasterDesc like '%{itemValueUnderScore}%'  or  ms.Mastervalue like '%{itemValueUnderScore}%') ";
                }
                else if (field.SearchCondition.EqualsIgnoreCase(ConditionStartWith))
                {
                    args.Where += $"{(i > 0 ? " OR " : string.Empty)}(ms.MasterDesc like '{itemValueUnderScore}%' or ms.Mastervalue like '{itemValueUnderScore}%') ";
                }
                else if (field.SearchCondition.EqualsIgnoreCase(ConditionEndWith))
                {
                    args.Where += $"{(i > 0 ? " OR " : string.Empty)} (ms.MasterDesc like '%{itemValueUnderScore}' or  ms.Mastervalue like '%{itemValueUnderScore}') ";
                }
            }

            if (args.Filter.BrandID > 0 && !field.SearchCondition.EqualsAnyIgnoreCase(ConditionIsEmpty, ConditionIsNotEmpty))
            {
                args.Where += "))";
            }

            args.Where += ")";
        }

        private static void UpdateProfileAdhocMasterForProductView(
            FilterArgs args,
            string masterGroupId,
            out string queryIsEmpty,
            out string queryNotNull,
            out string queryIsNotEmpty)
        {
            Guard.NotNull(args, nameof(args));
            if (args.Filter.BrandID == 0)
            {
                queryIsEmpty =
                    " (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock) " +
                    " left outer join (select distinct vrc.subscriptionID from vw_RecentConsensus vrc " +
                    " where vrc.MasterGroupID=" + masterGroupId + ") inn1 on sfilter.SubscriptionID = inn1.SubscriptionID " +
                    " where inn1.subscriptionID is null ";
                queryNotNull =
                    " (select distinct vrc.subscriptionID from vw_RecentConsensus vrc " +
                    " join Mastercodesheet ms with (nolock)  on vrc.MasterID = ms.MasterID " +
                    " where vrc.MasterGroupID=" + masterGroupId + " and ";
                queryIsNotEmpty =
                    " (select distinct vrc.subscriptionID from vw_RecentConsensus vrc " +
                    " where vrc.MasterGroupID=" + masterGroupId;
            }
            else
            {
                queryIsEmpty =
                    " (select distinct sfilter.subscriptionid FROM subscriptions sfilter WITH (nolock) " +
                    " join pubsubscriptions ps1 WITH (nolock) ON sfilter.subscriptionID = ps1.subscriptionID " +
                    " join BrandDetails bd with (nolock) on bd.PubID = ps1.PubID and bd.BrandID = " + args.Filter.BrandID + " " +
                    " left outer join ( " +
                    " select DISTINCT sd.subscriptionid  " +
                    " FROM   subscriptiondetails sd WITH (nolock)  " +
                    " 	join vw_brandconsensus v WITH (nolock) ON v.subscriptionid = sd.subscriptionid  " +
                    " 	join mastercodesheet ms WITH (nolock) ON v.masterid = ms.masterid  " +
                    " join branddetails bd5 WITH ( nolock) ON bd5.brandid = v.brandid  " +
                    " WHERE  bd5.brandid = " + args.Filter.BrandID + " AND ms.mastergroupid = " + masterGroupId +
                    ") inn3 on sfilter.SubscriptionID = inn3.SubscriptionID	 " +
                    " WHERE inn3.SubscriptionID is null ";
                queryNotNull =
                    " (select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) " +
                    " join vw_BrandConsensus v  with (nolock) on v.SubscriptionID = sfilter.SubscriptionID " +
                    " join Mastercodesheet ms with (nolock)  on v.MasterID = ms.MasterID " +
                    " join BrandDetails bd  with (nolock) on bd.BrandID = v.BrandID " +
                    " where  (bd.BrandID = " + args.Filter.BrandID + " and ms.MasterGroupID=" + masterGroupId + " and (";
                queryIsNotEmpty =
                    " (select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) " +
                    " join vw_BrandConsensus v  with (nolock) on v.SubscriptionID = sfilter.SubscriptionID " +
                    " join Mastercodesheet ms with (nolock)  on v.MasterID = ms.MasterID " +
                    " join BrandDetails bd  with (nolock) on bd.BrandID = v.BrandID " +
                    " where  (bd.BrandID = " + args.Filter.BrandID + " and ms.MasterGroupID=" + masterGroupId + ")";
            }
        }

        private static void UpdateProfileAdhocMasterForNonProductView(
            FilterArgs args,
            string masterGroupId,
            out string queryIsEmpty,
            out string queryNotNull,
            out string queryIsNotEmpty)
        {
            Guard.NotNull(args, nameof(args));
            if (args.Filter.BrandID == 0)
            {
                queryIsEmpty =
                    " (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock) " +
                    " left outer join (select distinct sd.subscriptionID from subscriptiondetails sd WITH (nolock) " +
                    " join Mastercodesheet ms  with (nolock) on sd.MasterID = ms.MasterID " +
                    " where ms.MasterGroupID=" + masterGroupId + ") inn1 on sfilter.SubscriptionID = inn1.SubscriptionID " +
                    " where inn1.subscriptionID is null ";
                queryNotNull =
                    " (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock) " +
                    " join Subscriptiondetails sd  with (nolock) on sd.SubscriptionID = sfilter.SubscriptionID " +
                    " join Mastercodesheet ms  with (nolock) on sd.MasterID = ms.MasterID and ms.MasterGroupID=" + masterGroupId +
                    " where ";
                queryIsNotEmpty =
                    " (select distinct sfilter.SubscriptionID from subscriptions sfilter with (nolock) " +
                    " join Subscriptiondetails sd  with (nolock) on sd.SubscriptionID = sfilter.SubscriptionID " +
                    " join Mastercodesheet ms  with (nolock) on sd.MasterID = ms.MasterID and ms.MasterGroupID=" + masterGroupId;
            }
            else
            {
                queryIsEmpty =
                    " (select distinct sfilter.subscriptionid FROM subscriptions sfilter WITH (nolock) " +
                    " join pubsubscriptions ps1 WITH (nolock) ON sfilter.subscriptionID = ps1.subscriptionID " +
                    " join BrandDetails bd with (nolock) on bd.PubID = ps1.PubID and bd.BrandID = " + args.Filter.BrandID + " " +
                    " left outer join ( " +
                    " select DISTINCT sd.subscriptionid  " +
                    " FROM   subscriptiondetails sd WITH (nolock)  " +
                    " 	   join vw_brandconsensus v WITH (nolock) ON v.subscriptionid = sd.subscriptionid  " +
                    " 	   join mastercodesheet ms WITH (nolock) ON v.masterid = ms.masterid  " +
                    " 	   join branddetails bd5 WITH ( nolock) ON bd5.brandid = v.brandid  " +
                    " WHERE  bd5.brandid = " + args.Filter.BrandID + " AND ms.mastergroupid = " + masterGroupId +
                    ") inn3 on sfilter.SubscriptionID = inn3.SubscriptionID	 " +
                    " WHERE inn3.SubscriptionID is null ";
                queryNotNull =
                    " (select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) " +
                    " join vw_BrandConsensus v  with (nolock) on v.SubscriptionID = sfilter.SubscriptionID " +
                    " join Mastercodesheet ms with (nolock)  on v.MasterID = ms.MasterID " +
                    " join BrandDetails bd  with (nolock) on bd.BrandID = v.BrandID " +
                    " where  (bd.BrandID = " + args.Filter.BrandID + " and ms.MasterGroupID=" + masterGroupId + " and (";
                queryIsNotEmpty =
                    " (select distinct sfilter.SubscriptionID from subscriptions sfilter  with (nolock) " +
                    " join vw_BrandConsensus v  with (nolock) on v.SubscriptionID = sfilter.SubscriptionID " +
                    " join Mastercodesheet ms with (nolock)  on v.MasterID = ms.MasterID " +
                    " join BrandDetails bd  with (nolock) on bd.BrandID = v.BrandID " +
                    " where  (bd.BrandID = " + args.Filter.BrandID + " and ms.MasterGroupID=" + masterGroupId + ")";
            }
        }

        private static void UpdateProfileAdhocMasterCondition(
            FilterArgs args,
            Field field,
            string queryIsEmpty,
            string queryNotNull,
            string queryIsNotEmpty)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            if (ConditionEqual.EqualsIgnoreCase(field.SearchCondition))
            {
                args.Where += $" s.SubscriptionID in {queryNotNull}";
            }
            else if (ConditionContains.EqualsIgnoreCase(field.SearchCondition))
            {
                args.Where += $" s.SubscriptionID in {queryNotNull}";
            }
            else if (ConditionDoesNotContain.EqualsIgnoreCase(field.SearchCondition))
            {
                args.Where += $" s.SubscriptionID not in {queryNotNull}";
            }
            else if (ConditionStartWith.EqualsIgnoreCase(field.SearchCondition))
            {
                args.Where += $" s.SubscriptionID in {queryNotNull}";
            }
            else if (ConditionEndWith.EqualsIgnoreCase(field.SearchCondition))
            {
                args.Where += $" s.SubscriptionID in {queryNotNull}";
            }
            else if (ConditionIsEmpty.EqualsIgnoreCase(field.SearchCondition))
            {
                args.Where += $" s.SubscriptionID in {queryIsEmpty}";
            }
            else if (ConditionIsNotEmpty.EqualsIgnoreCase(field.SearchCondition))
            {
                args.Where += $" s.SubscriptionID in {queryIsNotEmpty}";
            }
        }

        private static void UpdateProfileAdhocDate(FilterArgs args, Field field, string dateField)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            Guard.NotNull(dateField, nameof(dateField));
            string whereConditionField;

            if (dateField.ContainsAnyIgnoreCase(NameQDate, NameQualificationDate))
            {
                whereConditionField = "ps.QualificationDate";
            }
            else if (dateField.ContainsAnyIgnoreCase(NameTransactionDate, NamePubTransactionDate))
            {
                whereConditionField = IsProductViewType(args.Filter.ViewType)
                    ? $"convert(date,ps.{dateField})"
                    : $"convert(date,s.{dateField})";
            }
            else if (dateField.ContainsIgnoreCase(NameStatusUpdatedDate))
            {
                whereConditionField = $"convert(date,ps.{dateField})";
            }
            else if (dateField.ContainsAnyIgnoreCase(NameDateCreated, NameDateUpdated))
            {
                whereConditionField = args.Filter.BrandID > 0 || IsProductViewType(args.Filter.ViewType)
                    ? $"ps.{dateField}"
                    : $"s.{dateField}";
            }
            else
            {
                whereConditionField = IsProductViewType(args.Filter.ViewType)
                    ? $"ps.{dateField}"
                    : $"s.{dateField}";
            }

            var builder = new StringBuilder();
            FilterMVC.AppendDateCondition(builder, whereConditionField, field.SearchCondition, field.Values);
            args.Where += builder.ToString();
        }

        private static void UpdateProfileAdhocCountOrScore(FilterArgs args, Field field, string fieldName)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            Guard.NotNull(fieldName, nameof(fieldName));

            if (fieldName.EqualsIgnoreCase(NameProductCount))
            {
                args.Where += "(";

                if (args.Filter.BrandID > 0)
                {
                    args.Where +=
                        $" s.SubscriptionID in (select ps2.subscriptionid FROM pubsubscriptions ps2 WITH (nolock) join branddetails bd2 WITH (nolock) ON bd2.pubid = ps2.pubid where bd2.BrandID = {args.Filter.BrandID} group by ps2.subscriptionid ";
                }
                else
                {
                    args.Where += " s.SubscriptionID in (select ps2.subscriptionid FROM pubsubscriptions ps2 WITH (nolock) group by ps2.subscriptionid ";
                }

                if (field.SearchCondition.EqualsIgnoreCase(ConditionEqual))
                {
                    args.Where += $" having COUNT(ps2.PubID) = {field.Values})";
                }
                else if (field.SearchCondition.EqualsIgnoreCase(ConditionGreater))
                {
                    args.Where += $" having COUNT(ps2.PubID) > {field.Values})";
                }
                else if (field.SearchCondition.EqualsIgnoreCase(ConditionLesser))
                {
                    args.Where += $" having COUNT(ps2.PubID) < {field.Values})";
                }

                args.Where += ")";
            }
            else
            {
                UpdateProfileAdhocScore(args, field, fieldName);
            }
        }

        private static void UpdateProfileAdhocScore(FilterArgs args, Field field, string fieldName)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            Guard.NotNull(fieldName, nameof(fieldName));
            if (args.Filter.BrandID != 0 && fieldName.EqualsIgnoreCase(NameScore))
            {
                args.Query += $" join BrandScore bs  with (nolock)  on s.SubscriptionId = bs.SubscriptionId and bs.BrandID = {args.Filter.BrandID}";
            }

            args.Where += "(";

            var valueArray = field.Values.Split(VBarChar);

            if (field.SearchCondition.EqualsIgnoreCase(ConditionEqual))
            {
                UpdateProfileAdhocScoreWhere(args, "=", fieldName, valueArray[0]);
            }
            else if (field.SearchCondition.EqualsIgnoreCase(ConditionRange))
            {
                UpdateProfileAdhocScoreWhere(args, ">=", fieldName, valueArray[0]);

                var multiNames = !valueArray[0].IsNullOrWhiteSpace();
                if (!valueArray[1].IsNullOrWhiteSpace())
                {
                    if (args.Filter.BrandID != 0 && fieldName.EqualsIgnoreCase(NameScore))
                    {
                        args.Where += $"{(multiNames ? " and " : string.Empty)}bs.{fieldName} <= {valueArray[1]}";
                    }
                    else
                    {
                        if (IsProductViewType(args.Filter.ViewType))
                        {
                            args.Where += $"{(multiNames ? " and " : string.Empty)}ps.{fieldName} <= {valueArray[1]}";
                        }
                        else
                        {
                            args.Where += $"{(multiNames ? " and " : string.Empty)}s.{fieldName} <= {valueArray[1]}";
                        }
                    }
                }
            }
            else if (field.SearchCondition.EqualsIgnoreCase(ConditionGreater))
            {
                UpdateProfileAdhocScoreWhere(args, ">", fieldName, valueArray[0]);
            }
            else if (field.SearchCondition.EqualsIgnoreCase(ConditionLesser))
            {
                UpdateProfileAdhocScoreWhere(args, "<", fieldName, valueArray[0]);
            }

            args.Where += ")";
        }

        private static void UpdateProfileAdhocScoreWhere(FilterArgs args, string operatorString, string fieldName, string valueString)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(fieldName, nameof(fieldName));

            if (!valueString.IsNullOrWhiteSpace())
            {
                if (args.Filter.BrandID != 0 && fieldName.EqualsIgnoreCase(NameScore))
                {
                    args.Where += $"bs.{fieldName} {operatorString} {valueString}";
                }
                else
                {
                    if (IsProductViewType(args.Filter.ViewType))
                    {
                        args.Where += $"ps.{fieldName} {operatorString} {valueString}";
                    }
                    else
                    {
                        args.Where += $"s.{fieldName} {operatorString} {valueString}";
                    }
                }
            }
        }

        private static void UpdateProfileAdhocPubId(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            if (IsProductViewType(args.Filter.ViewType))
            {
                if (field.SearchCondition.EqualsAnyIgnoreCase(ConditionDoesNotContain, ConditionIsEmpty))
                {
                    args.Where += " ps.PubSubscriptionID not in (select E.PubSubscriptionID" +
                                  " FROM PubSubscriptionsExtension E with (nolock) join pubsubscriptions ps on ps.pubsubscriptionID = E.pubsubscriptionID" +
                                  " WHERE ps.PubID = " + args.Filter.PubID + " and ";
                }
                else
                {
                    args.Where += " ps.PubSubscriptionID in (select E.PubSubscriptionID" +
                                  " FROM PubSubscriptionsExtension E with (nolock) join pubsubscriptions ps on ps.pubsubscriptionID = E.pubsubscriptionID" +
                                  " WHERE ps.PubID = " + args.Filter.PubID + " and ";
                }
            }
            else
            {
                if (field.SearchCondition.EqualsAnyIgnoreCase(ConditionDoesNotContain, ConditionIsEmpty))
                {
                    args.Where += " s.SubscriptionID not in (select E.SubscriptionID" +
                                  " FROM SubscriptionsExtension E with (nolock) " +
                                  " WHERE ";
                }
                else
                {
                    args.Where += " s.SubscriptionID in (select E.SubscriptionID" +
                                  " FROM SubscriptionsExtension E with (nolock) " +
                                  " WHERE ";
                }
            }
        }

        private static void UpdateProfileAdhocCompareNumber(FilterArgs args, Field field, string columnName, string typeFlag)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            columnName = AdhocFlagI.EqualsIgnoreCase(typeFlag)
                ? $"CAST({columnName} AS INT)"
                : $"CAST({columnName} AS FLOAT)";

            var valueArray = field.Values.Split(VBarChar);
            var valueString = valueArray[0];
            var hasValue = !valueString.IsNullOrWhiteSpace();

            args.Where += "(";

            if (field.SearchCondition.EqualsIgnoreCase(ConditionEqual))
            {
                if (hasValue)
                {
                    args.Where += $"{columnName} = {valueString}";
                }
            }
            else if (field.SearchCondition.EqualsIgnoreCase(ConditionRange))
            {
                if (hasValue)
                {
                    args.Where += $"{columnName} >= {valueString}";
                }

                if (!valueArray[1].IsNullOrWhiteSpace())
                {
                    args.Where += $"{(hasValue ? " and " : " ")}{columnName} <= {valueArray[1]}";
                }
            }
            else if (field.SearchCondition.EqualsIgnoreCase(ConditionGreater))
            {
                if (hasValue)
                {
                    args.Where += $"{columnName} > {valueString}";
                }
            }
            else if (field.SearchCondition.EqualsIgnoreCase(ConditionLesser))
            {
                if (hasValue)
                {
                    args.Where += $"{columnName} < {valueString}";
                }
            }

            args.Where += "))";
        }

        private static void UpdateProfileAdhocSearchString(FilterArgs args, Field field, string columnName)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            var valueArray = field.Values.Split(CommaChar);

            for (var i = 0; i < valueArray.Length; i++)
            {
                var itemValue = Utilities.ReplaceSingleQuotes(valueArray[i].Trim());
                var itemValueEscaped = itemValue.Replace("_", "[_]");
                var orOrEmpty = i > 0 ? " OR " : string.Empty;

                if (field.SearchCondition.EqualsIgnoreCase(ConditionEqual))
                {
                    args.Where += $"{orOrEmpty} {columnName} = '{itemValue}'";
                }
                else if (field.SearchCondition.EqualsAnyIgnoreCase(ConditionContains, ConditionDoesNotContain))
                {
                    args.Where += $"{orOrEmpty} PATINDEX('%{itemValueEscaped}%', {columnName}) > 0 ";
                }
                else if (field.SearchCondition.EqualsIgnoreCase(ConditionStartWith))
                {
                    args.Where += $"{orOrEmpty} PATINDEX('{itemValueEscaped}%', {columnName}) > 0 ";
                }
                else if (field.SearchCondition.EqualsIgnoreCase(ConditionEndWith))
                {
                    args.Where += $"{orOrEmpty} PATINDEX('%{itemValueEscaped}', {columnName}) > 0 ";
                }
                else if (field.SearchCondition.EqualsAnyIgnoreCase(ConditionIsEmpty, ConditionIsNotEmpty))
                {
                    args.Where += $"{orOrEmpty} (ISNULL({columnName}, '') != ''  )";
                }
            }

            args.Where += ")";
        }

        private static void UpdateProfileAdhocSingleGroup(FilterArgs args, Field field)
        {
            Guard.NotNull(args, nameof(args));
            Guard.NotNull(field, nameof(field));
            string whereConditionField;

            if (field.Group.EqualsIgnoreCase(GroupNameCountry))
            {
                if (IsProductViewType(args.Filter.ViewType))
                {
                    if (field.SearchCondition.EqualsIgnoreCase(ConditionIsEmpty))
                    {
                        args.Query += " left outer join UAD_Lookup..Country c on c.CountryID = ps.countryID ";
                    }
                    else
                    {
                        args.Query += " join UAD_Lookup..Country c on c.CountryID = ps.countryID ";
                    }
                }
                else
                {
                    if (field.SearchCondition.EqualsIgnoreCase(ConditionIsEmpty))
                    {
                        args.Query += " left outer join UAD_Lookup..Country c on c.CountryID = s.countryID ";
                    }
                    else
                    {
                        args.Query += " join UAD_Lookup..Country c on c.CountryID = s.countryID ";
                    }
                }

                whereConditionField = "c.ShortName";
            }
            else if (field.Group.EqualsIgnoreCase(GroupNameGrpNo))
            {
                whereConditionField = $"cast(s.{field.Group} as varchar(100))";
            }
            else
            {
                whereConditionField = IsProductViewType(args.Filter.ViewType)
                    ? $"ps.{field.Group}"
                    : $"s.{field.Group}";
            }

            args.Where += FilterMVC.CreateStringCondition(
                args.Where,
                field.Group,
                field.SearchCondition,
                field.Values,
                whereConditionField);
        }

        private static bool IsProductViewType(Enums.ViewType type)
        {
            return type.IsAny(Enums.ViewType.ProductView, Enums.ViewType.CrossProductView);
        }

        private static void UpdateTempTablesOpen(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));

            if (args.OpenQuery.Length > 0)
            {
                if (args.JoinBlastForOpen)
                {
                    args.CreateTempTableQuery += "CREATE TABLE #tempOblast (blastid INT); ";
                    args.CreateTempTableQuery += "CREATE UNIQUE CLUSTERED INDEX #tempOblast_1 ON #tempOblast (blastid);  ";
                    args.CreateTempTableQuery += " insert into #tempOblast SELECT distinct blastid FROM blast bla WITH(nolock)  ";
                    args.CreateTempTableQuery += args.OpenBlastCondition;
                }

                args.CreateTempTableQuery += "; Create table #tempSOA (subscriptionid int);";
                args.CreateTempTableQuery += " CREATE UNIQUE CLUSTERED INDEX tempSOA_1 on #tempSOA  (subscriptionid); ";

                args.CreateTempTableQuery += $" Insert into #tempSOA {args.OpenQuery};";

                if (args.OpenCount == 0)
                {
                    args.Query += " left outer join #tempSOA soa1 on soa1.SubscriptionID = s.SubscriptionID ";
                    args.Where += $"{(args.Where.Length == 0 ? " " : " and ")} soa1.SubscriptionID is null";
                }
                else
                {
                    args.Query += " join #tempSOA soa1 on soa1.SubscriptionID = s.SubscriptionID ";
                }

                args.DropTempTableQuery += " drop table #tempSOA; ";
            }
        }

        private static void UpdateTempTablesClick(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));

            if (args.ClickQuery.Length > 0)
            {
                if (args.JoinBlastForClick)
                {
                    args.CreateTempTableQuery += "CREATE TABLE #tempCblast (blastid INT); ";
                    args.CreateTempTableQuery += "CREATE UNIQUE CLUSTERED INDEX #tempCblast_1 ON #tempCblast (blastid);  ";
                    args.CreateTempTableQuery += "Insert into #tempCblast SELECT distinct blastid FROM blast bla WITH (nolock)  ";
                    args.CreateTempTableQuery += args.ClickBlastCondition;
                }

                args.CreateTempTableQuery += "; Create table #tempSCA (subscriptionid int);";
                args.CreateTempTableQuery += " CREATE UNIQUE CLUSTERED INDEX tempSCA_1 on #tempSCA  (subscriptionid); ";

                args.CreateTempTableQuery += $" Insert into #tempSCA {args.ClickQuery};";

                if (args.ClickCount == 0)
                {
                    args.Query += " left outer join #tempSCA sca1 on sca1.SubscriptionID = s.SubscriptionID ";
                    args.Where += $"{(args.Where.Length == 0 ? " " : " and ")} sca1.SubscriptionID is null";
                }
                else
                {
                    args.Query += " join #tempSCA sca1 on sca1.SubscriptionID = s.SubscriptionID ";
                }

                args.DropTempTableQuery += " drop table #tempSCA; ";
            }
        }

        private static void UpdateTempTablesVisit(FilterArgs args)
        {
            Guard.NotNull(args, nameof(args));

            if (args.VisitQuery.Length > 0)
            {
                args.CreateTempTableQuery += "; Create table #tempSVA (subscriptionid int);";
                args.CreateTempTableQuery += " CREATE UNIQUE CLUSTERED INDEX tempSVA_1 on #tempSVA  (subscriptionid); ";

                args.CreateTempTableQuery += $" Insert into #tempSVA {args.VisitQuery};";

                if (args.VisitCount == 0)
                {
                    args.Query += " left outer join #tempSVA sva1 on sva1.SubscriptionID = s.SubscriptionID ";
                    args.Where += $"{(args.Where.Length == 0 ? " " : " and ")} sva1.SubscriptionID is null";
                }
                else
                {
                    args.Query += " join #tempSVA sva1 on sva1.SubscriptionID = s.SubscriptionID ";
                }

                args.DropTempTableQuery += " drop table #tempSVA; ";
            }
        }

        private static string CreateQueryWithBrand(
            Filter filter,
            StringBuilder condition,
            int count,
            bool joinBlast,
            char abbreviation,
            string joinTable,
            string countColumn)
        {
            var query = new StringBuilder();

            query.AppendFormat(
                " select s{0}.SubscriptionID from PubSubscriptions ps{0}  with (NOLOCK)  join {1} s{0}  with (NOLOCK) on s{0}.PubSubscriptionID = ps{0}.PubSubscriptionID ",
                abbreviation,
                joinTable);

            if (filter.BrandID > 0)
            {
                condition.Append(condition.Length > 0 ? " and " : " where ");
                condition.AppendFormat(" bd.BrandID in ({0}) and b.Isdeleted = 0", filter.BrandID);
                query.AppendFormat(" JOIN branddetails bd  with (NOLOCK) ON bd.pubID = ps{0}.pubID join Brand b  with (NOLOCK) on b.BrandID = bd.BrandID ", abbreviation);
            }

            if (joinBlast)
            {
                query.AppendFormat(" join #temp{1}blast t{0}b  with (NOLOCK) on s{0}.blastID = t{0}b.blastID ", abbreviation, char.ToUpper(abbreviation));
            }

            query.Append(condition);
            query.AppendFormat(" group by s{0}.SubscriptionID", abbreviation);
            query.AppendFormat(
                " HAVING Count(s{0}.{2}) >= {1}",
                abbreviation,
                count > 0 ? count.ToString() : " ",
                countColumn);

            return query.ToString();
        }
    }
}
