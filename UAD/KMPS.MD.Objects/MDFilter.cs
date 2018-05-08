using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FrameworkUAD_Lookup.Entity;
using KM.Common;
using KMPlatform.Object;

namespace KMPS.MD.Objects
{
    public class MDFilter
    {
        private const char CommaSeparator = ',';
        private const string Print = "Print";
        private const string Miles = "miles";
        private const char PipeSeparator = '|';
        private const string OpenCriteria = "OPEN CRITERIA";
        private const string ClickCriteria = "CLICK CRITERIA";
        private const string VisitCriteria = "VISIT CRITERIA";
        private const string OpenActivity = "OPEN ACTIVITY";
        private const string OpenBlastid = "OPEN BLASTID";
        private const string OpenCampaigns = "OPEN CAMPAIGNS";
        private const string OpenEmailSubject = "OPEN EMAIL SUBJECT";
        private const string OpenEmailSentDate = "OPEN EMAIL SENT DATE";
        private const string LinkActivity = "LINK";
        private const string ClickActivity = "CLICK ACTIVITY";
        private const string ClickBlastid = "CLICK BLASTID";
        private const string ClickCampaigns = "CLICK CAMPAIGNS";
        private const string ClickEmailSubject = "CLICK EMAIL SUBJECT";
        private const string ClickEmailSentDate = "CLICK EMAIL SENT DATE";
        private const string DomainTrackingActivity = "DOMAIN TRACKING";
        private const string UrlActivity = "URL";
        private const string VisitActivity = "VISIT ACTIVITY";
        private const string NoVisits = "No Visits";
        private const string NoClicks = "No Clicks";
        private const string NoOpens = "No Opens";
        private const string Visited = "Visited";
        private const string Clicked = "Clicked";
        private const string Opened = "Opened";
        private const string State = "STATE";
        private const string CountryStandard = "COUNTRY";
        private const string Media = "MEDIA";
        private const string Email = "EMAIL";
        private const string Phone = "PHONE";
        private const string Fax = "FAX";
        private const string MailPermission = "MAILPERMISSION";
        private const string FaxPermission = "FAXPERMISSION";
        private const string PhonePermission = "PHONEPERMISSION";
        private const string OtherProductsPermission = "OTHERPRODUCTSPERMISSION";
        private const string ThirdpartyPermission = "THIRDPARTYPERMISSION";
        private const string EmailRenewPermission = "EMAILRENEWPERMISSION";
        private const string TextPermission = "TEXTPERMISSION";
        private const string Geolocated = "GEOLOCATED";
        private const string EmailStatusStandard = "EMAIL STATUS";
        private const string Yes = "Yes";
        private const string No = "No";
        private const string Blank = "Blank";
        private const string One = "1";
        private const string Zero = "0";
        private const string MediaPrint = "A";
        private const string MediaDigital = "B";
        private const string MediaOptOut = "O";
        private const string MediaBoth = "C";
        private const string Digital = "Digital";
        private const string OptOut = "Opt Out";
        private const string Both = "Both";
        private const string AdHocFilterM = "m";
        private const string AdHocFilterE = "e";
        private const string CategoryType = "CATEGORY TYPE";
        private const string CategoryCode = "CATEGORY CODE";
        private const string XAct = "XACT";
        private const string TransactionCode = "TRANSACTION CODE";
        private const string QsourceType = "QSOURCE TYPE";
        private const string QsourceCode = "QSOURCE CODE";
        private const string QualificationYear = "QUALIFICATION YEAR";
        private const string QFrom = "QFROM";
        private const string Qto = "QTO";
        private const string WaveMailing = "WAVE MAILING";
        private const string IsWaveMailed = "Is Wave Mailed";
        private const string IsNotWaveMailed = "Is Not Wave Mailed";

        #region Properties
        public int FilterId { get; set; }
        public string Name { get; set; }
        public string FilterXML { get; set; }
        public string FilterType { get; set; }
        public Enums.ViewType ViewType { get; set; }
        public int CreatedUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PubID { get; set; }
        public string  PubName { get; set; }
        public bool IsDeleted { get; set; }
        public int UpdatedUserID { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? BrandID { get; set; }
        public string BrandName { get; set; }
        public int? FilterCategoryID { get; set; }
        public string FilterCategoryName { get; set; }
        public bool AddtoSalesView { get; set; }
        public int? QuestionCategoryID { get; set; }
        public string QuestionCategoryName { get; set; }
        public string QuestionName { get; set; }
        public bool IsLocked { get; set; }
        public string Notes { get; set; }
        #endregion

        public static Filters LoadFilters(KMPlatform.Object.ClientConnections clientconnection, int filterID, int userID)
        {
            Filters fc = new Filters(clientconnection, userID);

            MDFilter mdfilter = MDFilter.GetByFilterID(clientconnection, filterID);

            List<FilterGroup> lfg = FilterGroup.getByFilterID(clientconnection, filterID);

            int i = 1;
            foreach (FilterGroup fg in lfg)
            {
                Filter f = new Filter();
                //f.FilterID = mdfilter.FilterId.ToString();
                f.FilterName = mdfilter.Name;
                f.ViewType = mdfilter.ViewType;
                f.PubID = mdfilter.PubID;
                f.BrandID = mdfilter.BrandID == null ? 0 : (Int32)(mdfilter.BrandID);
                f.FilterGroupID = fg.FilterGroupID;
                f.FilterGroupName = "Data Segment" + i;
                f.FilterNo = fc.Count + 1;

                List<FilterDetails> fds = FilterDetails.getByFilterGroupID(clientconnection, fg.FilterGroupID);

                int pubID = 0;
                if (f.ViewType == Enums.ViewType.CrossProductView)
                    pubID = Convert.ToInt32(fds.Where(x => x.Name.ToLower() == "product").FirstOrDefault().Values);
                else
                    pubID = mdfilter.PubID;

                foreach (FilterDetails fd in fds)
                {
                    switch (fd.FilterType)
                    {
                        case Enums.FiltersType.Dimension:
                            string displayName = string.Empty;
                            if (f.ViewType == Enums.ViewType.ProductView || f.ViewType == Enums.ViewType.CrossProductView)
                            {
                                List<ResponseGroup> rg = ResponseGroup.GetByPubID(clientconnection, pubID);
                                if (rg.Exists(x => x.ResponseGroupName.ToUpper() == fd.Group.ToUpper()))
                                {
                                    displayName = rg.Find(x => x.ResponseGroupName.ToUpper() == fd.Group.ToUpper()).DisplayName;
                                    f.Fields.Add(new Field(displayName, fd.Values, "", fd.SearchCondition, fd.FilterType, fd.Group));
                                }
                                else
                                {
                                    throw new Exception("Response Group " + fd.Group.ToUpper() + " does not exists.");
                                }
                            }
                            else
                            {
                                List<MasterGroup> mg = MasterGroup.GetAll(clientconnection);

                                if (mg.Exists(x => x.ColumnReference.ToUpper() == fd.Group.ToUpper()))
                                {
                                    displayName = mg.Find(x => x.ColumnReference.ToUpper() == fd.Group.ToUpper()).DisplayName;
                                    f.Fields.Add(new Field(displayName, fd.Values, "", fd.SearchCondition, fd.FilterType, fd.Group));
                                }
                                else
                                {
                                    throw new Exception("Master Group " + fd.Group.ToUpper() + " does not exists.");
                                }
                            }
                            break;
                        default:
                            f.Fields.Add(new Field(fd.Name, fd.Values, "", fd.SearchCondition, fd.FilterType, fd.Group));
                            break;
                    }

                    if (fd.Name.ToLower() == "product" && f.ViewType == Enums.ViewType.CrossProductView)
                        f.PubID = Convert.ToInt32(fd.Values);
                }

                fc.Add(f);
                i++;
            }

            return GetTextValues(clientconnection, fc);
        }

        private static Filters GetTextValues(ClientConnections clientConnections, Filters filters)
        {
            var products = GetActiveProducts(clientConnections, filters);
            var codeSheets = GetMasterCodeSheets(clientConnections, filters);
            var countries = GetCountries(filters);
            var regions = GetRegions(filters);
            var campaigns = GetEcnCampaigns(clientConnections, filters);
            var categoryCodeTypes = GetCategoryCodeTypes(filters);
            var categories = GetCategories(filters);
            var codeTypes = GetTransactionCodeTypes(filters);
            var transactionCodes = GetTransactionCodes(filters);
            var codes = GetCodes(filters);
            var emailStates = GetEmailStates(clientConnections, filters);

            foreach (var filter in filters)
            {
                foreach (var field in filter.Fields)
                {
                    switch (field.FilterType)
                    {
                        case Enums.FiltersType.Brand:
                            BrandFilter(clientConnections, field);
                            break;
                        case Enums.FiltersType.Product:
                            ProductFilter(filter, field, products);
                            break;
                        case Enums.FiltersType.Dimension:
                            DimensionFilter(clientConnections, filter, field, codeSheets);
                            break;
                        case Enums.FiltersType.Standard:
                            StandardFilter(field, regions, countries, emailStates);
                            break;
                        case Enums.FiltersType.Geo:
                            GeoFilter(field);
                            break;
                        case Enums.FiltersType.Activity:
                            ActivityFilter(clientConnections, field, campaigns);
                            break;
                        case Enums.FiltersType.Adhoc:
                            AdhocFilter(clientConnections, field, filter);
                            break;
                        case Enums.FiltersType.Circulation:
                            CirculationFilter(field, categoryCodeTypes, categories, codeTypes, transactionCodes, codes);
                            break;
                    }
                }
            }

            return filters;
        }

        private static void BrandFilter(ClientConnections clientConnections, Field field)
        {
            field.Text = Brand.GetAll(clientConnections)
                .Find(x => x.BrandID == Convert.ToInt32(field.Values))
                .BrandName;
        }

        private static void GeoFilter(Field field)
        {
            var strvalues = field.SearchCondition.Split(PipeSeparator);
            field.Text = $"{strvalues[0]} & {strvalues[1]} {Miles} - {strvalues[2]} {Miles}";
        }

        private static void ActivityFilter(ClientConnections clientConnections, Field field, IEnumerable<ECNCampaign> campaigns)
        {
            switch (field.Name.ToUpper())
            {
                case OpenCriteria:
                    ActivityEventCriteria(NoOpens, Opened, field);
                    break;
                case ClickCriteria:
                    ActivityEventCriteria(NoClicks, Clicked, field);
                    break;
                case VisitCriteria:
                    ActivityEventCriteria(NoVisits, Visited, field);
                    break;
                case OpenActivity:
                case OpenBlastid:
                    field.Text = field.Values;
                    break;
                case OpenCampaigns:
                    ActivityOpenCampaigns(field, campaigns);
                    break;
                case OpenEmailSubject:
                case OpenEmailSentDate:
                case LinkActivity:
                case ClickActivity:
                case ClickBlastid:
                    field.Text = field.Values;
                    break;
                case ClickCampaigns:
                    ActivityClickCampaigns(field, campaigns);
                    break;
                case ClickEmailSubject:
                case ClickEmailSentDate:
                    field.Text = field.Values;
                    break;
                case DomainTrackingActivity:
                    field.Text = DomainTracking.Get(clientConnections)
                        .Find(x => x.DomainTrackingID == Convert.ToInt32(field.Values))
                        .DomainName;
                    break;
                case UrlActivity:
                case VisitActivity:
                    field.Text = field.Values;
                    break;
            }
        }

        private static void ActivityClickCampaigns(Field field, IEnumerable<ECNCampaign> campaigns)
        {
            var campaignIds = field.Values.Split(CommaSeparator)
                .Select(int.Parse)
                .ToList();

            var cquery = from campaignId in campaignIds
                         join campaign in campaigns on campaignId equals campaign.ECNCampaignID
                         select new { text = campaign.ECNCampaignName };

            field.Text = string.Join(", ", cquery.Select(i => i.text.Trim()));
        }

        private static void ActivityOpenCampaigns(Field field, IEnumerable<ECNCampaign> campaigns)
        {
            var campaignIds = field.Values.Split(CommaSeparator)
                .Select(int.Parse)
                .ToList();

            var oquery = from campaignId in campaignIds
                         join campaign in campaigns on campaignId equals campaign.ECNCampaignID
                         select new { text = campaign.ECNCampaignName };

            field.Text = string.Join(", ", oquery.Select(i => i.text.Trim()));
        }

        private static void ActivityEventCriteria(string noEvent, string eventName, Field field)
        {
            int numberOfEvents;
            if (!int.TryParse(field.Values, out numberOfEvents))
            {
                return;
            }

            switch (numberOfEvents)
            {
                case 0:
                    field.Text = noEvent;
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 10:
                case 15:
                case 20:
                    field.Text = $"{eventName} {numberOfEvents}+";
                    break;
                case 30:
                    field.Text = $"{eventName} 30+";
                    break;

                // POSSIBLE BUG - should maybe use > to also handle for inbetween values like 8 correctly 
                // POSSIBLE BUG - no default case in switch
            }
        }

        private static void StandardFilter(Field field, IEnumerable<Region> regions, IEnumerable<Country> countries, IEnumerable<EmailStatus> emailStates)
        {
            switch (field.Name.ToUpper())
            {
                case State:
                    StandardState(field, regions);
                    break;
                case CountryStandard:
                    StandardCountry(field, countries);
                    break;
                case Media:
                    StandardMedia(field);
                    break;
                case Email:
                case Phone:
                case Fax:
                case MailPermission:
                case FaxPermission:
                case PhonePermission:
                case OtherProductsPermission:
                case ThirdpartyPermission:
                case EmailRenewPermission:
                case TextPermission:
                case Geolocated:
                    StandardGeoLocated(field);
                    break;
                case EmailStatusStandard:
                    StandardEmailStatus(field, emailStates);
                    break;

                // POSSIBLE BUG - no default case
            }
        }

        private static void StandardEmailStatus(Field field, IEnumerable<EmailStatus> emailStates)
        {
            var emailStateIds = field.Values.Split(CommaSeparator)
                .Select(int.Parse)
                .ToList();

            var emailJoins = from emailstatusId in emailStateIds
                      join es in emailStates on emailstatusId equals es.EmailStatusID
                      select new { text = es.Status };

            field.Text = string.Join(", ", emailJoins.Select(i => i.text.Trim()));
        }

        private static void StandardGeoLocated(Field field)
        {
            var geoStrings = field.Values.Split(CommaSeparator);
            var fieldText = string.Empty;

            foreach (var geoString in geoStrings)
            {
                var text = geoString == One 
                               ? Yes : geoString == Zero 
                                   ? No : Blank;

                fieldText += fieldText == string.Empty
                                        ? text
                                        : ", " + text;
            }

            field.Text = fieldText;
        }

        private static void StandardState(Field field, IEnumerable<Region> regions)
        {
            var regionCodes = field.Values
                .Split(CommaSeparator)
                .ToList();

            var regionJoins = from regionCode in regionCodes
                              join rc in regions on regionCode equals rc.RegionCode
                              select new { text = rc.RegionCode };

            field.Text = string.Join(", ", regionJoins.Select(i => i.text.Trim()));
        }

        private static void StandardCountry(Field field, IEnumerable<Country> countries)
        {
            var countryIds = field.Values.Split(CommaSeparator)
                .Select(int.Parse)
                .ToList();

            var countryJoins = from countryId in countryIds
                               join ctry in countries on countryId equals ctry.CountryID
                               select new { text = ctry.ShortName };

            field.Text = string.Join(", ", countryJoins.Select(i => i.text.Trim()));
        }

        private static void StandardMedia(Field field)
        {
            var mediaStrings = field.Values.Split(CommaSeparator);
            var fieldText = string.Empty;

            foreach (var media in mediaStrings)
            {
                switch (media)
                {
                    case MediaPrint:
                        fieldText += fieldText == string.Empty
                                         ? Print
                                         : CommaSeparator + Print;
                        break;
                    case MediaDigital:
                        fieldText += fieldText == string.Empty
                                         ? Digital
                                         : CommaSeparator + Digital;
                        break;
                    case MediaOptOut:
                        fieldText += fieldText == string.Empty
                                         ? OptOut
                                         : CommaSeparator + OptOut;
                        break;
                    case MediaBoth:
                        fieldText += fieldText == string.Empty
                                         ? Both
                                         : CommaSeparator + Both;
                        break;
                }
            }

            field.Text = fieldText;
        }

        private static void AdhocFilter(ClientConnections clientConnections, Field field, Filter filter)
        {
            var adhocStrings = field.Group.Split(PipeSeparator);

            switch (adhocStrings[0].ToLower())
            {
                case AdHocFilterM:
                    {
                        var groupStrings = field.Group.Split(PipeSeparator);
                        field.Text = MasterGroup.GetAll(clientConnections)
                            .Find(x => x.MasterGroupID == Convert.ToInt32(groupStrings[1]))
                            .DisplayName.ToUpper();
                    }

                    break;
                case AdHocFilterE:

                    if (filter.ViewType == Enums.ViewType.ProductView
                        || filter.ViewType == Enums.ViewType.CrossProductView)
                    {
                        var groupStrings = field.Group.Split(PipeSeparator);
                        field.Text = PubSubscriptionsExtensionMapper.GetAll(clientConnections)
                            .Find(x => x.StandardField.ToLower() == groupStrings[1].ToLower() 
                                       && x.PubID == filter.PubID)
                            .CustomField;
                    }
                    else
                    {
                        var groupStrings = field.Group.Split(PipeSeparator);
                        field.Text = SubscriptionsExtensionMapper.GetAll(clientConnections)
                            .Find(x => x.StandardField.ToLower() == groupStrings[1].ToLower())
                            .CustomField;
                    }

                    break;
                default:
                    {
                        var groupStrings = field.Group.Split(PipeSeparator);

                        if (groupStrings.Length > 1)
                        {
                            groupStrings[1] = groupStrings[1].Replace("[", string.Empty);
                            groupStrings[1] = groupStrings[1].Replace("]", string.Empty);
                            field.Text = groupStrings[1];
                        }
                        else
                        {
                            groupStrings[0] = groupStrings[0].Replace("[", string.Empty);
                            groupStrings[0] = groupStrings[0].Replace("]", string.Empty);
                            field.Text = groupStrings[0];
                        }
                    }

                    break;
            }
        }

        private static void CirculationFilter(
            Field field,
            IEnumerable<CategoryCodeType> categoryCodeTypes,
            IEnumerable<Category> categories,
            IEnumerable<TransactionCodeType> codeTypes,
            IEnumerable<TransactionCode> transactionCodes,
            IEnumerable<Code> codes)
        {
            switch (field.Name.ToUpper())
            {
                case CategoryType:
                    CirculationFilterCategoryType(field, categoryCodeTypes);
                    break;
                case CategoryCode:
                    CirculationFilterCategoryCode(field, categories);
                    break;
                case XAct:
                    CirculationFilterXact(field, codeTypes);
                    break;
                case TransactionCode:
                    CirculationFilterTransactionCode(field, transactionCodes);
                    break;
                case QsourceType:
                    CirculationFilterQSourceType(field, codes);
                    break;
                case QsourceCode:
                    CirculationFilterQSourceCode(field, codes);
                    break;
                case Media:
                    var fieldText = CirculationFilterMedia(field);
                    field.Text = fieldText;
                    break;
                case QualificationYear:
                case QFrom:
                case Qto:
                    field.Text = field.Values;
                    break;
                case WaveMailing:
                    CirculationFilterWaveMailing(field);
                    break;
            }
        }

        private static void CirculationFilterCategoryType(Field field, IEnumerable<CategoryCodeType> categoryCodeTypes)
        {
            var categoryCodeType = field.Values.Split(CommaSeparator)
                .Select(int.Parse)
                .ToList();

            var categoryCodeTypeJoins = from categoryCodeTypeId in categoryCodeType
                                        join c in categoryCodeTypes on categoryCodeTypeId equals c.CategoryCodeTypeID
                                        select new { text = c.CategoryCodeTypeName };

            field.Text = string.Join(", ", categoryCodeTypeJoins.Select(i => i.text.Trim()));
        }

        private static void CirculationFilterCategoryCode(Field field, IEnumerable<Category> categories)
        {
            var categeorycodes = field.Values.Split(CommaSeparator)
                .Select(int.Parse)
                .ToList();

            var categoryCode = from categorycodeId in categeorycodes
                               join c in categories on categorycodeId equals c.CategoryCodeID
                               select new { text = c.CategoryCodeName };

            field.Text = string.Join(", ", categoryCode.Select(i => i.text.Trim()));
        }

        private static void CirculationFilterXact(Field field, IEnumerable<TransactionCodeType> codeTypes)
        {
            var tcodetypes = field.Values.Split(CommaSeparator)
                .Select(int.Parse)
                .ToList();

            var transactionCodeTypes = from tcodetype in tcodetypes
                                       join tct in codeTypes on tcodetype equals tct.TransactionCodeTypeID
                                       select new { text = tct.TransactionCodeTypeName };

            field.Text = string.Join(", ", transactionCodeTypes.Select(i => i.text.Trim()));
        }

        private static void CirculationFilterTransactionCode(Field field, IEnumerable<TransactionCode> transactionCodes)
        {
            var transactioncodes = field.Values.Split(CommaSeparator)
                .Select(int.Parse)
                .ToList();

            var transactionCodeJoins = from transactioncodeId in transactioncodes
                                       join c in transactionCodes on transactioncodeId equals c.TransactionCodeID
                                       select new { text = c.TransactionCodeValue + ". " + c.TransactionCodeName };

            field.Text = string.Join(", ", transactionCodeJoins.Select(i => i.text.Trim()));
        }

        private static void CirculationFilterQSourceType(Field field, IEnumerable<Code> codes)
        {
            var qsourcetypes = field.Values.Split(CommaSeparator)
                .Select(int.Parse)
                .ToList();

            var sourceTypeJoins = from code in qsourcetypes
                                  join c in codes on code equals c.CodeID
                                  select new { text = c.DisplayName };

            field.Text = string.Join(", ", sourceTypeJoins.Select(i => i.text.Trim()));
            return;
        }

        private static void CirculationFilterQSourceCode(Field field, IEnumerable<Code> codes)
        {
            var qsourcecodes = field.Values.Split(CommaSeparator)
                .Select(int.Parse)
                .ToList();

            var sourceCodeJoins = from code in qsourcecodes
                                  join c in codes on code equals c.CodeID
                                  select new { text = c.DisplayName };

            field.Text = string.Join(", ", sourceCodeJoins.Select(i => i.text.Trim()));
            return;
        }

        private static void CirculationFilterWaveMailing(Field field)
        {
            var fieldText = string.Empty;

            if (field.Values == "1")
            {
                fieldText = fieldText == string.Empty
                                       ? IsWaveMailed
                                       : CommaSeparator + IsWaveMailed;
            }
            else if (field.Values == "0")
            {
                fieldText = fieldText == string.Empty
                                       ? IsNotWaveMailed
                                       : CommaSeparator + IsNotWaveMailed;
            }

            field.Text = fieldText;
        }

        private static string CirculationFilterMedia(Field field)
        {
            var mediaStrings = field.Values.Split(CommaSeparator);
            var builder = new StringBuilder();

            foreach (var media in mediaStrings)
            {
                var fieldText = string.Empty;
                if (media == MediaPrint)
                {
                    fieldText = fieldText == string.Empty
                                     ? Print
                                     : CommaSeparator + Print;
                }
                else if (media == MediaDigital)
                {
                    fieldText = fieldText == string.Empty
                                     ? Digital
                                     : CommaSeparator + Digital;
                }
                else if (media == MediaOptOut)
                {
                    fieldText = fieldText == string.Empty
                                     ? OptOut
                                     : CommaSeparator + OptOut;
                }
                else if (media == MediaBoth)
                {
                    fieldText = fieldText == string.Empty
                                     ? Both
                                     : CommaSeparator + Both;
                }

                builder.Append(fieldText);
            }

            return builder.ToString();
        }

        private static void DimensionFilter(
            ClientConnections clientConnections,
            Filter filter,
            Field field,
            IEnumerable<MasterCodeSheet> codeSheets)
        {
            if (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView)
            {
                var codeSheetsByPubId = CodeSheet.GetByPubID(clientConnections, filter.PubID);

                var codeSheetStrings = field.Values.Split(CommaSeparator)
                    .Select(int.Parse)
                    .ToList();

                var query = from codeSheetId in codeSheetStrings
                            join cs in codeSheetsByPubId on codeSheetId equals cs.CodeSheetID
                            select new { text = cs.ResponseDesc + " " + "(" + cs.ResponseValue + ")" };

                field.Text = string.Join(", ", query.Select(i => i.text.Trim()));
            }
            else
            {
                var nonProductCodeSheets = field.Values.Split(CommaSeparator)
                    .Select(int.Parse)
                    .ToList();

                var query = from codeSheet in nonProductCodeSheets
                            join mcs in codeSheets on codeSheet equals mcs.MasterID
                            select new { text = mcs.MasterDesc + " " + "(" + mcs.MasterValue + ")" };

                field.Text = string.Join(", ", query.Select(i => i.text.Trim()));
            }
        }

        private static void ProductFilter(Filter filter, Field field, IEnumerable<Pubs> products)
        {
            if (filter.ViewType == Enums.ViewType.ProductView || filter.ViewType == Enums.ViewType.CrossProductView)
            {
                field.Text = products.First(x => x.PubID == Convert.ToInt32(field.Values))
                    .PubName;
            }
            else
            {
                var productIds = field.Values.Split(CommaSeparator)
                    .Select(int.Parse)
                    .ToList();

                var query = from productId in productIds
                            join pub in products on productId equals pub.PubID
                            select new { text = pub.PubName };

                field.Text = string.Join(", ", query.Select(i => i.text.Trim()));
            }
        }

        private static IList<EmailStatus> GetEmailStates(ClientConnections clientConnections, Filters filters)
        {
            return filters.Any(x => x.Fields.Any(f => f.FilterType == Enums.FiltersType.Standard && f.Name.ToUpper() == EmailStatusStandard))
                       ? EmailStatus.GetAll(clientConnections)
                       : null;
        }

        private static IList<Code> GetCodes(Filters filters)
        {
            return filters.Any(x => x.Fields.Any(f => f.FilterType == Enums.FiltersType.Circulation 
                                                      && (f.Name.ToUpper() == QsourceType || f.Name.ToUpper() == QsourceCode)))
                       ? Code.GetAll()
                       : null;
        }

        private static IList<TransactionCode> GetTransactionCodes(Filters filters)
        {
            return filters.Any(x => x.Fields.Any(f => f.FilterType == Enums.FiltersType.Circulation && f.Name.ToUpper() == TransactionCode))
                       ? new FrameworkUAD_Lookup.BusinessLogic.TransactionCode().Select()
                       : null;
        }

        private static IList<TransactionCodeType> GetTransactionCodeTypes(Filters filters)
        {
            return filters.Any(x => x.Fields.Any(f => f.FilterType == Enums.FiltersType.Circulation && f.Name.ToUpper() == XAct))
                       ? TransactionCodeType.GetAll()
                       : null;
        }

        private static IList<Category> GetCategories(Filters filters)
        {
            return filters.Any(x => x.Fields.Any(f => f.FilterType == Enums.FiltersType.Circulation && f.Name.ToUpper() == CategoryCode))
                       ? Category.GetAll()
                       : null;
        }

        private static IList<CategoryCodeType> GetCategoryCodeTypes(Filters filters)
        {
            return filters.Any(x => x.Fields.Any(f => f.FilterType == Enums.FiltersType.Circulation && f.Name.ToUpper() == CategoryType))
                       ? new FrameworkUAD_Lookup.BusinessLogic.CategoryCodeType().Select()
                       : null;
        }

        private static IList<ECNCampaign> GetEcnCampaigns(ClientConnections clientConnections, Filters filters)
        {
            return filters.Any(x => x.Fields.Any(f => f.FilterType == Enums.FiltersType.Activity
                                      && (f.Name.ToUpper() == OpenCampaigns || f.Name.ToUpper() == ClickCampaigns)))
                       ? ECNCampaign.GetAll(clientConnections)
                       : null;
        }

        private static IList<Region> GetRegions(Filters filters)
        {
            return filters.Any(x => x.Fields.Any(f => f.FilterType == Enums.FiltersType.Standard && f.Name.ToUpper() == State))
                       ? Region.GetAll()
                       : null;
        }

        private static IList<Country> GetCountries(Filters filters)
        {
            return filters.Any(x => x.Fields.Any(f => f.FilterType == Enums.FiltersType.Standard && f.Name.ToUpper() == CountryStandard))
                       ? Country.GetAll()
                       : null;
        }

        private static IList<MasterCodeSheet> GetMasterCodeSheets(ClientConnections clientConnections, Filters filters)
        {
            return filters.Any(x => !(x.ViewType == Enums.ViewType.ProductView || x.ViewType == Enums.ViewType.CrossProductView)
                                    && x.Fields.Any(f => f.FilterType == Enums.FiltersType.Dimension))
                       ? MasterCodeSheet.GetAll(clientConnections)
                       : null;
        }

        private static IList<Pubs> GetActiveProducts(ClientConnections clientConnections, Filters filters)
        {
            return filters.Any(x => x.Fields.Any(f => f.FilterType == Enums.FiltersType.Product))
                       ? Pubs.GetActive(clientConnections)
                       : null;
        }

        public static Filters LoadFilters(KMPlatform.Object.ClientConnections clientconnection, int filterID, int userID, Field dcfield)
        {
            Filters fc = new Filters(clientconnection, userID);

            MDFilter mdfilter = MDFilter.GetByFilterID(clientconnection, filterID);

            List<FilterGroup> lfg = FilterGroup.getByFilterID(clientconnection, filterID);

            int i = 1;
            foreach (FilterGroup fg in lfg)
            {
                Filter f = new Filter();
                //f.FilterID = mdfilter.FilterId.ToString();
                f.FilterName = mdfilter.Name;
                f.ViewType = mdfilter.ViewType;
                f.PubID = mdfilter.PubID;
                f.BrandID = mdfilter.BrandID == null ? 0 : (Int32)(mdfilter.BrandID);
                f.FilterGroupID = fg.FilterGroupID;
                f.FilterGroupName = "Data Segment" + i;
                f.FilterNo = fc.Count + 1;

                List<FilterDetails> fds = FilterDetails.getByFilterGroupID(clientconnection, fg.FilterGroupID);

                foreach (FilterDetails fd in fds)
                {
                    f.Fields.Add(new Field(fd.Name, fd.Values, "", fd.SearchCondition, fd.FilterType, fd.Group));

                    if (fd.Name.ToLower() == "product" && f.ViewType == Enums.ViewType.CrossProductView)
                        f.PubID = Convert.ToInt32(fd.Values);
                }

                f.Fields.Add(dcfield);

                fc.Add(f);

                i++;
            }

            return GetTextValues(clientconnection, fc);
        }


        #region Data
        public static List<MDFilter> GetFilterByUserIDType(KMPlatform.Object.ClientConnections clientconnection, int userID, Enums.ViewType ViewType, int pubID, int brandID, bool IsAdmin, bool IsViewMode)
        {
            List<MDFilter> retList = new List<MDFilter>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            string sqlquery = "select f.*, b.BrandID, BrandName from Filters f left outer join brand b on f.brandID = b.BrandID left outer join filtersegmentation fs on f.filterID = fs.FilterID ";

            if (IsViewMode)
            {
                if (ViewType == Enums.ViewType.ProductView)
                    sqlquery += " where filterType = 'ProductView' and pubID = @PubID";
                else if (ViewType == Enums.ViewType.CrossProductView)
                    sqlquery += " where filterType = 'CrossProductView' ";
                else
                    sqlquery += " where (Isnull(filterType, '') = 'ConsensusView' or filterType = 'RecencyView') ";

                if (brandID > 0)
                    sqlquery += " and b.brandID = @BrandID";

                if (userID > 0)
                {
                    sqlquery += " and f.CreatedUserID = @UserID";
                }
                else
                {
                    if (!IsAdmin)
                        sqlquery += " and islocked = 0 or (islocked = 1 and f.CreatedUserID = @UserID)";
                }
            }
            else
            {
                if (ViewType == Enums.ViewType.ProductView)
                    sqlquery += " where filterType = 'ProductView' and pubID = @PubID";
                else if (ViewType == Enums.ViewType.RecencyView)
                    sqlquery += " where filterType = 'RecencyView' ";
                else if (ViewType == Enums.ViewType.CrossProductView)
                    sqlquery += " where filterType = 'CrossProductView' ";
                else if (ViewType == Enums.ViewType.ConsensusView)
                    sqlquery += " where Isnull(filterType, '') = 'ConsensusView' and Isnull(pubID,0)=0";

                if (!IsAdmin)
                    sqlquery += " and islocked = 0 or (islocked = 1 and f.CreatedUserID = @UserID)";

                if(brandID > 0)
                    sqlquery += " and b.brandID = @BrandID";
                else
                    sqlquery += " and (b.brandID = 0 or b.brandID is null)";
            }

            sqlquery += " and f.IsDeleted=0 and isnull(b.IsDeleted,0) = 0 and fs.filtersegmentationID is null order by Name ASC";

            SqlCommand cmd = new SqlCommand(sqlquery, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@PubID", pubID);
            cmd.Parameters.AddWithValue("@BrandID", brandID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<MDFilter> builder = DynamicBuilder<MDFilter>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    MDFilter x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static MDFilter GetByFilterID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            MDFilter x = new MDFilter();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("Select * from Filters where FilterID = @FilterID", conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<MDFilter> builder = DynamicBuilder<MDFilter>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    x = builder.Build(rdr);

                    if (x.FilterType == null || x.FilterType == string.Empty)
                        x.FilterType = "ConsensusView";

                    x.ViewType = GetViewType(x.FilterType);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return x;
        }

        public static List<MDFilter> GetAll(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<MDFilter> retList = new List<MDFilter>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_Filters_Select_All", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<MDFilter> builder = DynamicBuilder<MDFilter>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    MDFilter x = builder.Build(rdr);

                    if (x.FilterType == null || x.FilterType == string.Empty)
                        x.FilterType = "ConsensusView";

                    x.ViewType = GetViewType(x.FilterType);

                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static List<MDFilter> GetNotInBrand_TypeAddedinName(KMPlatform.Object.ClientConnections clientconnection)
        {
            List<MDFilter> retList = GetAll(clientconnection).FindAll(x => x.BrandID == 0 || x.BrandID == null);

            var query = retList.Select(r => new MDFilter()
            {
                FilterId = r.FilterId,
                Name = 
                (
                    r.FilterType == "ProductView" ? "[Product] " + r.Name :
                    r.FilterType == "RecencyView" ? "[Recency] " + r.Name :
                    r.FilterType == "CrossProductView" ? "[CrossProduct] " + r.Name : "[Consensus] " + r.Name
                )
            }).ToList();

            return query;
        }

        public static List<MDFilter> GetByBrandID_TypeAddedinName(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            List<MDFilter> retList = GetAll(clientconnection).FindAll(x => x.BrandID == brandID);

            var query = retList.Select(r => new MDFilter()
            {
                FilterId = r.FilterId,
                Name =
                (
                    r.FilterType == "ProductView" ? "[Product] " + r.Name :
                    r.FilterType == "RecencyView" ? "[Recency] " + r.Name :
                    r.FilterType == "CrossProductView" ? "[CrossProduct] " + r.Name : "[Consensus] " + r.Name
                )
            }).ToList();

            return query;
        }

        public static List<MDFilter> GetByBrandID(KMPlatform.Object.ClientConnections clientconnection, int brandID)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == brandID);
        }

        public static List<MDFilter> GetInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID > 0);
        }

        public static List<MDFilter> GetNotInBrand(KMPlatform.Object.ClientConnections clientconnection)
        {
            return GetAll(clientconnection).FindAll(x => x.BrandID == 0 || x.BrandID == null);
        }

        public static List<MDFilter> GetByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            List<MDFilter> retList = new List<MDFilter>();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("e_Filters_Select_UserID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<MDFilter> builder = DynamicBuilder<MDFilter>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    MDFilter x = builder.Build(rdr);
                    retList.Add(x);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retList;
        }

        public static List<MDFilter> GetInBrandByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            return GetByUserID(clientconnection, userID).FindAll(x => x.BrandID > 0);
        }

        public static List<MDFilter> GetByBrandIDUserID(KMPlatform.Object.ClientConnections clientconnection, int brandID, int userID)
        {
            return GetByUserID(clientconnection, userID).FindAll(x => x.BrandID == brandID);
        }

        public static List<MDFilter> GetNotInBrandByUserID(KMPlatform.Object.ClientConnections clientconnection, int userID)
        {
            return GetByUserID(clientconnection, userID).FindAll(x => x.BrandID == 0 || x.BrandID == null);
        }

        public static MDFilter GetByID(KMPlatform.Object.ClientConnections clientconnection, int filterID)
        {
            MDFilter retItem = new MDFilter();
            SqlConnection conn = DataFunctions.GetClientSqlConnection(clientconnection);
            SqlCommand cmd = new SqlCommand("select * from Filters where FilterID = @FilterID", conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@FilterID", filterID);
            cmd.CommandTimeout = 0;
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DynamicBuilder<MDFilter> builder = DynamicBuilder<MDFilter>.CreateBuilder(rdr);
                while (rdr.Read())
                {
                    retItem = builder.Build(rdr);

                    if (retItem.FilterType == null || retItem.FilterType == string.Empty)
                        retItem.FilterType = "ConsensusView";

                    retItem.ViewType = GetViewType(retItem.FilterType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return retItem;
        }

        private static Enums.ViewType GetViewType(string ViewType)
        {
            return (Enums.ViewType)Enum.Parse(typeof(Enums.ViewType), ViewType, true);
        }

        public static bool ExistsByFilterName(KMPlatform.Object.ClientConnections clientconnection, int filterID, string filterName)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Exists_ByFilterName");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterName", filterName));
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }
        public static bool ExistsByFilterCategoryID(KMPlatform.Object.ClientConnections clientconnection, int filterCategoryID)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Exists_ByFilterCategoryID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@filterCategoryID", filterCategoryID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static bool ExistsByQuestionCategoryID(KMPlatform.Object.ClientConnections clientconnection, int questionCategoryID)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Exists_ByQuestionCategoryID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@questionCategoryID", questionCategoryID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }

        public static bool ExistsQuestionName(KMPlatform.Object.ClientConnections clientconnection, int filterID, string questionName)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Exists_QuestionName");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@QuestionName", questionName));
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection))) > 0 ? true : false;
        }
        #endregion

        #region CRUD
        public static int insert(KMPlatform.Object.ClientConnections clientconnection, MDFilter mdf)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Save");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterID", mdf.FilterId));
            cmd.Parameters.Add(new SqlParameter("@Name", mdf.Name));
            cmd.Parameters.Add(new SqlParameter("@FilterXML", (object)mdf.FilterXML ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterType", (object)mdf.FilterType.ToString() ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@PubID", (object)mdf.PubID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@BrandID", (object)mdf.BrandID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@FilterCategoryID", (object)mdf.FilterCategoryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@AddtoSalesView", (object)mdf.AddtoSalesView ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@QuestionName", (object)mdf.QuestionName ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@QuestionCategoryID", (object)mdf.QuestionCategoryID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsDeleted", (object)mdf.IsDeleted ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedUserID", (object)mdf.UpdatedUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@UpdatedDate", (object)mdf.UpdatedDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedUserID", (object)mdf.CreatedUserID ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@CreatedDate", (object)mdf.CreatedDate ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@IsLocked", (object)mdf.IsLocked ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Notes", (object)mdf.Notes ?? DBNull.Value));
            return Convert.ToInt32(DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection)));
        }

        public static void Delete(KMPlatform.Object.ClientConnections clientconnection, int filterID, int userID)
        {
            SqlCommand cmd = new SqlCommand("e_Filters_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FilterID", filterID));
            cmd.Parameters.Add(new SqlParameter("@UserID", userID));
            DataFunctions.executeScalar(cmd, DataFunctions.GetClientSqlConnection(clientconnection));
        }
        #endregion
    }
}