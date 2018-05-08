using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonifyToECNEngine.PersonifyClasses
{
    public class Customer
    {
        public Customer()
        {
            MASTER_CUSTOMER_ID = string.Empty;
            SUB_CUSTOMER_ID = string.Empty;
            ORG_ID = string.Empty;
            ORG_UNIT_ID = string.Empty;
            RECORD_TYPE = string.Empty;
            SEARCH_NAME = string.Empty;
            LABEL_NAME = string.Empty;
            CAN_PLACE_ORDER_FLAG = string.Empty;
            BILL_PRIMARY_ACCOUNT_FLAG = string.Empty;
            NAME_PREFIX = string.Empty;
            FIRST_NAME = string.Empty;
            MIDDLE_NAME = string.Empty;
            LAST_NAME = string.Empty;
            NAME_SUFFIX = string.Empty;
            NAME_CREDENTIALS = string.Empty;
            NICKNAME = string.Empty;
            FORMAL_SALUTATION = string.Empty;
            INFORMAL_SALUTATION = string.Empty;
            CUSTOMER_CLASS_CODE = string.Empty;
            CUSTOMER_STATUS_CODE = string.Empty;
            CUSTOMER_STATUS_DATE = string.Empty;
            ALLOW_FAX_FLAG = string.Empty;
            ALLOW_EMAIL_FLAG = string.Empty;
            ALLOW_PHONE_FLAG = string.Empty;
            ALLOW_LABEL_SALES_FLAG = string.Empty;
            ALLOW_SOLICITATION_FLAG = string.Empty;
            ALLOW_INTERNAL_MAIL_FLAG = string.Empty;
            SOLICITATION_REMOVAL_DATE = string.Empty;
            TAXABLE_FLAG = string.Empty;
            FEDERAL_TAX_ID = string.Empty;
            VAT_ID = string.Empty;
            TAX_EXEMPT_ID = string.Empty;
            SSN = string.Empty;
            GENDER_CODE = string.Empty;
            BIRTH_DATE = string.Empty;
            ETHNICITY_CODE = string.Empty;
            ANNUAL_INCOME_RANGE_CODE = string.Empty;
            JOB_FUNCTION_CODE = string.Empty;
            REVENUE_RANGE_CODE = string.Empty;
            STAFF_RANGE_CODE = string.Empty;
            FOUNDATION_FLAG = string.Empty;
            FND_MATCHING_FLAG = string.Empty;
            FND_MATCHING_PERCENT = string.Empty;
            FND_MATCHING_LIMIT = string.Empty;
            LIST_CODE = string.Empty;
            ENFORCE_COM_STRUCT_FLAG = string.Empty;
            NOT_A_DUPLICATE_FLAG = string.Empty;
            MERGED_TO_MAST_CUST = string.Empty;
            MERGED_TO_SUB_CUST = string.Empty;
            EXHIBITOR_FLAG = string.Empty;
            VIP_COMMITTEE_PRIORITY = string.Empty;
            HOME_PHONE = string.Empty;
            WORK_PHONE = string.Empty;
            PRIMARY_PHONE = string.Empty;
            PRIMARY_FAX = string.Empty;
            PRIMARY_EMAIL_ADDRESS = string.Empty;
            PRIMARY_URL = string.Empty;
            PRIMARY_JOB_TITLE = string.Empty;
            PRIMARY_PHONE_LOCATION_CODE = string.Empty;
            PRIMARY_EMAIL_LOCATION_CODE = string.Empty;
            PRIMARY_FAX_LOCATION_CODE = string.Empty;
            PUBLISH_PRIMARY_FAX_FLAG = string.Empty;
            PUBLISH_PRIMARY_PHONE_FLAG = string.Empty;
            PUBLISH_PRIMARY_EMAIL_FLAG = string.Empty;
            PUBLISH_HOME_PHONE_FLAG = string.Empty;
            PUBLISH_WORK_PHONE_FLAG = string.Empty;
            DEFAULT_LANGUAGE_CODE = string.Empty;
            ADDOPER = string.Empty;
            ADDDATE = string.Empty;
            MODOPER = string.Empty;
            MODDATE = string.Empty;
            CONCURRENCY_ID = string.Empty;
            IMAGE_ID = string.Empty;
            PREFERRED_COMM_METHOD_CODE = string.Empty;
            BILL_PRIMARY_EMPLOYER_FLAG = string.Empty;
            LAST_FIRST_NAME = string.Empty;
            TRN_COUNTRY_CODE = string.Empty;
            TRN_STATE_CODE = string.Empty;
            ORIG_CUS_ADDRESS_ID = string.Empty;
            ORIG_CUS_ADDRESS_TYPE_CODE = string.Empty;
            WEB_SEGMENT_CREATION_FLAG = string.Empty;
            SEGMENT_RULE_CODE = string.Empty;
            SEGMENT_QUALIFIER1 = string.Empty;
            SEGMENT_QUALIFIER2 = string.Empty;
            INCLUDE_IN_DIRECTORY_FLAG = string.Empty;
            SEGMENT_DESCR = string.Empty;
            USR_HIMSS_TERRITORY = string.Empty;
            CAN_CREATE_SEGMENTS_FLAG = string.Empty;
            MAILING_PER_YEAR = string.Empty;
            USR_VENDOR_REGID = string.Empty;
            USR_HIMSS_ACC_MGR_ID = string.Empty;
            USR_LDCUNIQUEID = string.Empty;
            USR_OPTOUT_COMMENTS = string.Empty;
            USR_OPTOUT_REASON_CODE = string.Empty;
            PRIMARY_CONTACT_MAST_CUST_ID = string.Empty;
            PRIMARY_CONTACT_SUB_CUST_ID = string.Empty;
            SHORT_NAME_CODE = string.Empty;
            OFFICIAL_COMMITTEE_FLAG = string.Empty;
            COM_SUBGROUP_MAST_CUST_ID = string.Empty;
            COM_SUBGROUP_SUB_CUST_ID = string.Empty;
            CUS_VOLUNTEER_FLAG = string.Empty;
            ADVERTISER_FLAG = string.Empty;
            ADVERTISING_AGENCY_FLAG = string.Empty;
            BPA_TITLE_ONLY_FLAG = string.Empty;
            PROSPECT_ID = string.Empty;
            POTENTIAL_DUPLICATE_REVIEW_FLAG = string.Empty;
            DUPLICATE_REASON = string.Empty;
            POTENTIAL_MATCH_PROBABILITY = string.Empty;
            ORIGINAL_ORG_ID = string.Empty;
            ORIGINAL_ORG_UNIT_ID = string.Empty;
            ORIGINAL_SOURCE_CODE = string.Empty;
            ALLOW_SYSTEM_NOTIFICATION_FLAG = string.Empty;
            SPOUSE_FIRST_NAME = string.Empty;
            MARITAL_STATUS_CODE = string.Empty;
            ALLOW_ADVOCACY_FLAG = string.Empty;
            CURRENCY_CODE = string.Empty;
            PRIMARY_SEARCH_GROUP = string.Empty;
            FAMILY_FLAG = string.Empty;
            PRIMARY_SEARCH_GROUP_OVERRIDE_FLAG = string.Empty;
            NAME_CREDENTIALS_DESC = string.Empty;
            USR_AUTHOR_BIOGRAPHY = string.Empty;

        }

        public string MASTER_CUSTOMER_ID { get; set; }
        public string SUB_CUSTOMER_ID { get; set; }
        public string ORG_ID { get; set; }
        public string ORG_UNIT_ID { get; set; }
        public string RECORD_TYPE { get; set; }
        public string SEARCH_NAME { get; set; }
        public string LABEL_NAME { get; set; }
        public string CAN_PLACE_ORDER_FLAG { get; set; }
        public string BILL_PRIMARY_ACCOUNT_FLAG { get; set; }
        public string NAME_PREFIX { get; set; }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string NAME_SUFFIX { get; set; }
        public string NAME_CREDENTIALS { get; set; }
        public string NICKNAME { get; set; }
        public string FORMAL_SALUTATION { get; set; }
        public string INFORMAL_SALUTATION { get; set; }
        public string CUSTOMER_CLASS_CODE { get; set; }
        public string CUSTOMER_STATUS_CODE { get; set; }
        public string CUSTOMER_STATUS_DATE { get; set; }
        public string ALLOW_FAX_FLAG { get; set; }
        public string ALLOW_EMAIL_FLAG { get; set; }
        public string ALLOW_PHONE_FLAG { get; set; }
        public string ALLOW_LABEL_SALES_FLAG { get; set; }
        public string ALLOW_SOLICITATION_FLAG { get; set; }
        public string ALLOW_INTERNAL_MAIL_FLAG { get; set; }
        public string SOLICITATION_REMOVAL_DATE { get; set; }
        public string TAXABLE_FLAG { get; set; }
        public string FEDERAL_TAX_ID { get; set; }
        public string VAT_ID { get; set; }
        public string TAX_EXEMPT_ID { get; set; }
        public string SSN { get; set; }
        public string GENDER_CODE { get; set; }
        public string BIRTH_DATE { get; set; }
        public string ETHNICITY_CODE { get; set; }
        public string ANNUAL_INCOME_RANGE_CODE { get; set; }
        public string JOB_FUNCTION_CODE { get; set; }
        public string REVENUE_RANGE_CODE { get; set; }
        public string STAFF_RANGE_CODE { get; set; }
        public string FOUNDATION_FLAG { get; set; }
        public string FND_MATCHING_FLAG { get; set; }
        public string FND_MATCHING_PERCENT { get; set; }
        public string FND_MATCHING_LIMIT { get; set; }
        public string LIST_CODE { get; set; }
        public string ENFORCE_COM_STRUCT_FLAG { get; set; }
        public string NOT_A_DUPLICATE_FLAG { get; set; }
        public string MERGED_TO_MAST_CUST { get; set; }
        public string MERGED_TO_SUB_CUST { get; set; }
        public string EXHIBITOR_FLAG { get; set; }
        public string VIP_COMMITTEE_PRIORITY { get; set; }
        public string HOME_PHONE { get; set; }
        public string WORK_PHONE { get; set; }
        public string PRIMARY_PHONE { get; set; }
        public string PRIMARY_FAX { get; set; }
        public string PRIMARY_EMAIL_ADDRESS { get; set; }
        public string PRIMARY_URL { get; set; }
        public string PRIMARY_JOB_TITLE { get; set; }
        public string PRIMARY_PHONE_LOCATION_CODE { get; set; }
        public string PRIMARY_EMAIL_LOCATION_CODE { get; set; }
        public string PRIMARY_FAX_LOCATION_CODE { get; set; }
        public string PUBLISH_PRIMARY_FAX_FLAG { get; set; }
        public string PUBLISH_PRIMARY_PHONE_FLAG { get; set; }
        public string PUBLISH_PRIMARY_EMAIL_FLAG { get; set; }
        public string PUBLISH_HOME_PHONE_FLAG { get; set; }
        public string PUBLISH_WORK_PHONE_FLAG { get; set; }
        public string DEFAULT_LANGUAGE_CODE { get; set; }
        public string ADDOPER { get; set; }
        public string ADDDATE { get; set; }
        public string MODOPER { get; set; }
        public string MODDATE { get; set; }
        public string CONCURRENCY_ID { get; set; }
        public string IMAGE_ID { get; set; }
        public string PREFERRED_COMM_METHOD_CODE { get; set; }
        public string BILL_PRIMARY_EMPLOYER_FLAG { get; set; }
        public string LAST_FIRST_NAME { get; set; }
        public string TRN_COUNTRY_CODE { get; set; }
        public string TRN_STATE_CODE { get; set; }
        public string ORIG_CUS_ADDRESS_ID { get; set; }
        public string ORIG_CUS_ADDRESS_TYPE_CODE { get; set; }
        public string WEB_SEGMENT_CREATION_FLAG { get; set; }
        public string SEGMENT_RULE_CODE { get; set; }
        public string SEGMENT_QUALIFIER1 { get; set; }
        public string SEGMENT_QUALIFIER2 { get; set; }
        public string INCLUDE_IN_DIRECTORY_FLAG { get; set; }
        public string SEGMENT_DESCR { get; set; }
        public string USR_HIMSS_TERRITORY { get; set; }
        public string CAN_CREATE_SEGMENTS_FLAG { get; set; }
        public string MAILING_PER_YEAR { get; set; }
        public string USR_VENDOR_REGID { get; set; }
        public string USR_HIMSS_ACC_MGR_ID { get; set; }
        public string USR_LDCUNIQUEID { get; set; }
        public string USR_OPTOUT_COMMENTS { get; set; }
        public string USR_OPTOUT_REASON_CODE { get; set; }
        public string PRIMARY_CONTACT_MAST_CUST_ID { get; set; }
        public string PRIMARY_CONTACT_SUB_CUST_ID { get; set; }
        public string SHORT_NAME_CODE { get; set; }
        public string OFFICIAL_COMMITTEE_FLAG { get; set; }
        public string COM_SUBGROUP_MAST_CUST_ID { get; set; }
        public string COM_SUBGROUP_SUB_CUST_ID { get; set; }
        public string CUS_VOLUNTEER_FLAG { get; set; }
        public string ADVERTISER_FLAG { get; set; }
        public string ADVERTISING_AGENCY_FLAG { get; set; }
        public string BPA_TITLE_ONLY_FLAG { get; set; }
        public string PROSPECT_ID { get; set; }
        public string POTENTIAL_DUPLICATE_REVIEW_FLAG { get; set; }
        public string DUPLICATE_REASON { get; set; }
        public string POTENTIAL_MATCH_PROBABILITY { get; set; }
        public string ORIGINAL_ORG_ID { get; set; }
        public string ORIGINAL_ORG_UNIT_ID { get; set; }
        public string ORIGINAL_SOURCE_CODE { get; set; }
        public string ALLOW_SYSTEM_NOTIFICATION_FLAG { get; set; }
        public string SPOUSE_FIRST_NAME { get; set; }
        public string MARITAL_STATUS_CODE { get; set; }
        public string ALLOW_ADVOCACY_FLAG { get; set; }
        public string CURRENCY_CODE { get; set; }
        public string PRIMARY_SEARCH_GROUP { get; set; }
        public string FAMILY_FLAG { get; set; }
        public string PRIMARY_SEARCH_GROUP_OVERRIDE_FLAG { get; set; }
        public string NAME_CREDENTIALS_DESC { get; set; }
        public string USR_AUTHOR_BIOGRAPHY { get; set; }
    }

    public class CustomerDemographic
    {
        public CustomerDemographic()
        {
            CUS_DEMOGRAPHIC_ID = string.Empty;
            MASTER_CUSTOMER_ID = string.Empty;
            SUB_CUSTOMER_ID = string.Empty;
            DEMOGRAPHIC_CODE = string.Empty;
            DEMOGRAPHIC_SUBCODE = string.Empty;
            USER_D1 = string.Empty;
            USER_D2 = string.Empty;
            USER_N1 = string.Empty;
            COMMENTS = string.Empty;
            ADDOPER = string.Empty;
            ADDDATE = string.Empty;
            MODOPER = string.Empty;
            MODDATE = string.Empty;
            CONCURRENCY_ID = string.Empty;
            USR_PRODUCT_CODE = string.Empty;
            USR_DESCRIPTION = string.Empty;
        }

        public string CUS_DEMOGRAPHIC_ID { get; set; }
        public string MASTER_CUSTOMER_ID { get; set; }
        public string SUB_CUSTOMER_ID { get; set; }
        public string DEMOGRAPHIC_CODE { get; set; }
        public string DEMOGRAPHIC_SUBCODE { get; set; }
        public string USER_D1 { get; set; }
        public string USER_D2 { get; set; }
        public string USER_N1 { get; set; }
        public string COMMENTS { get; set; }
        public string ADDOPER { get; set; }
        public string ADDDATE { get; set; }
        public string MODOPER { get; set; }
        public string MODDATE { get; set; }
        public string CONCURRENCY_ID { get; set; }
        public string USR_PRODUCT_CODE { get; set; }
        public string USR_DESCRIPTION { get; set; }
    }

    public class AppCode
    {
        public AppCode()
        {
            CODE = string.Empty;
        }

        public string CODE { get; set; }
        //SUBSYSTEM
        //TYPE
        //CODE
        //DESCR
        //SYSTEM_FLAG
        //DISPLAY_ORDER
        //SCREEN_RESTRICTION
        //PRIMARY_SUBCODE
        //ACTIVE_FLAG
        //STATUS_CHANGE_DATE
        //AUTO_POPULATE_FLAG
        //AVAILABLE_TO_WEB_FLAG
        //OPTION_1
        //OPTION_2
        //OPTION_3
        //SUBCODE_OPTION_1_REQD_FLAG
        //SUBCODE_OPTION_2_REQD_FLAG
        //SUBCODE_OPTION_3_REQD_FLAG
        //SUBCODE_OPTION_1_CAPTION
        //SUBCODE_OPTION_2_CAPTION
        //SUBCODE_OPTION_3_CAPTION
        //ADDOPER
        //ADDDATE
        //MODOPER
        //MODDATE
        //CONCURRENCY_ID
        //OPTION_4
        //PUBLIC_CODE_FLAG
        //OPTION_5
        //OPTION_6
        //CODE_OPTION_5_REQD_FLAG
        //CODE_OPTION_6_REQD_FLAG
        //CODE_OPTION_5_CAPTION
        //CODE_OPTION_6_CAPTION
        //USR_MTG_FIELD_TYPE
        //USR_MTG_FIELD_REQUIRED
        //USR_MTG_PROD_ID
        //USR_MTG_QUEST_TEXT
        //USR_MTG_TEXT_MULTILINE
        //USR_MTG_LIST_ORIENTATION
        //ACCESSPOINT_CODE
        //USER_INSTRUCTION
        //HELP_TEXT

    }

    public class AppSubCode
    {
        public AppSubCode()
        {
            CODE = string.Empty;
            ACTIVE_FLAG = string.Empty;
        }

        public string CODE { get; set; }
        public string ACTIVE_FLAG { get; set; }
    }

    public class CustomerAddressDetail
    {
        public CustomerAddressDetail()
        {
            COMPANY_NAME = string.Empty;
            CUS_ADDRESS_ID = string.Empty;
            ADDRESS_TYPE_CODE = string.Empty;
        }

        public string COMPANY_NAME { get; set; }
        public string CUS_ADDRESS_ID { get; set; }
        public string ADDRESS_TYPE_CODE { get; set; }
    }

    public class CustomerAddress
    {
        public CustomerAddress()
        {
            CUS_ADDRESS_ID = string.Empty;
            ADDRESS_1 = string.Empty;
            ADDRESS_2 = string.Empty;
            ADDRESS_3 = string.Empty;
            ADDRESS_4 = string.Empty;
            CITY = string.Empty;
            STATE = string.Empty;
            POSTAL_CODE = string.Empty;
            COUNTRY_CODE = string.Empty;
            COUNTRY_DESCR = string.Empty;
        }

        public string CUS_ADDRESS_ID { get; set; }
        public string ADDRESS_1 { get; set; }
        public string ADDRESS_2 { get; set; }
        public string ADDRESS_3 { get; set; }
        public string ADDRESS_4 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string POSTAL_CODE { get; set; }
        public string COUNTRY_CODE { get; set; }
        public string COUNTRY_DESCR { get; set; }
    }

    public class OrderDetail
    {
        public OrderDetail()
        {
            ORDER_NO = string.Empty;
            ORDER_LINE_NO = string.Empty;
            ORDER_DATE = string.Empty;
            PARENT_PRODUCT = string.Empty;
            PRODUCT_CODE = string.Empty;
            SUBSYSTEM = string.Empty;
            PRODUCT_TYPE_CODE = string.Empty;
            INVOICE_NO = string.Empty;
            LINE_STATUS_CODE = string.Empty;
            LINE_STATUS_DATE = string.Empty;
            SHIP_MASTER_CUSTOMER_ID = string.Empty;
            SHIP_SUB_CUSTOMER_ID = string.Empty;
            SHIP_ADDRESS_ID = string.Empty;
            ACTUAL_TOTAL_AMOUNT = string.Empty;
            CYCLE_END_DATE = string.Empty;
            GRACE_DATE = string.Empty;
            ATTENDANCE_FLAG = string.Empty;
        }

        public string ORDER_NO { get; set; }
        public string ORDER_LINE_NO { get; set; }
        public string ORDER_DATE { get; set; }
        public string PARENT_PRODUCT { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string SUBSYSTEM { get; set; }
        public string PRODUCT_TYPE_CODE { get; set; }
        public string INVOICE_NO { get; set; }
        public string LINE_STATUS_CODE { get; set; }
        public string LINE_STATUS_DATE { get; set; }
        public string SHIP_MASTER_CUSTOMER_ID { get; set; }
        public string SHIP_SUB_CUSTOMER_ID { get; set; }
        public string SHIP_ADDRESS_ID { get; set; }
        public string ACTUAL_TOTAL_AMOUNT { get; set; }
        public string CYCLE_END_DATE { get; set; }
        public string GRACE_DATE { get; set; }
        public string ATTENDANCE_FLAG { get; set; }

    }

    public class OrderMaster
    {
        public OrderMaster()
        {
            ORDER_STATUS_CODE = string.Empty;
        }

        public string ORDER_STATUS_CODE { get; set; }
    }

    public class ProductCategory
    {
        public ProductCategory()
        {
            PRODUCT_CATEGORY_ID = string.Empty;
            PRODUCT_ID = string.Empty;
            CATEGORY = string.Empty;
        }

        public string PRODUCT_CATEGORY_ID { get; set; }
        public string PRODUCT_ID { get; set; }
        public string CATEGORY { get; set; }
    }

    public class Product
    {
        public Product()
        {
            PRODUCT_TYPE_CODE = string.Empty;
            PRODUCT_CLASS_CODE = string.Empty;
        }

        public string PRODUCT_TYPE_CODE { get; set; }
        public string PRODUCT_CLASS_CODE { get; set; }
    }

    public class CommitteeMember
    {
        public CommitteeMember()
        {
            COM_COMMITTEE_MEMBER_ID = string.Empty;
        }

        public string COM_COMMITTEE_MEMBER_ID { get; set; }
    }
}
