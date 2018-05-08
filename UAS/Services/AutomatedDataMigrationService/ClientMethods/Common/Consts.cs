using System;

namespace ADMS.ClientMethods.Common
{
    public static class Consts
    {
        public static readonly int RowBatch = 2500;

        public static readonly char CommaSeparator = ',';
        public static readonly string CommaJoin = ",";
        public static readonly string CsvExtension = ".csv";
        public static readonly string DotSepator = ".";
        public static readonly string DoubleBackwardSlash = "\\";
        public static readonly string CommaFileDelimiter = "comma";
        public static readonly string TabColumnDelimiter = "tab";
        public static readonly string TxtExtension = ".txt";
        public static readonly string XlsxExtension = ".xlsx";

        public static readonly string StartQuote = "\"";
        public static readonly string FinishQuote = "\",";
        public static readonly string ProcessStarted = "Started:";
        public static readonly string ProcessFinished = "Finished:";

        public static readonly string SnwaeCode = "SNWAE";
        public static readonly string SnwauCode = "SNWAU";
        public static readonly string SnwohCode = "SNWOH";
        public static readonly string SnwtnaeCode = "SNWTNAE";
        public static readonly string SnwtnallCode = "SNWTNALL";
        public static readonly string SnwtnauCode = "SNWTNAU";
        public static readonly string SnwtnelCode = "SNWTNEL";
        public static readonly string SnwthdCode = "SNWTNHD";
        public static readonly string SnwtnofCode = "SNWTNOF";
        public static readonly string SaeroCode = "SAERO";
        public static readonly string SautoCode = "SAUTO";
        public static readonly string SdigautoCode = "SDIGAUTO";
        public static readonly string SofhCode = "SOFH";
        public static readonly string SsaeupDomCode = "SSAEUP-DOM";

        public static readonly string MessageCurrentRowCount = "Current Row Count: ";
        public static readonly string MessageGettingAlgorithms = "Getting Algorithms {0}";
        public static readonly string MessageGettingFile = "Getting File {0}";
        public static readonly string MessageUploadingToFtp = "Uploading to FTP ";
        public static readonly string MessageFinishedUpload = "Finished Uploading to FTP ";
        public static readonly string NewBatchStatus = "New Batch: ";
        public static readonly string NextBatchStatus = " . Next Batch: ";
        public static readonly string ProcessedStatus = "Processed: ";
        public static readonly string ProcessingStatus = "Processing: ";
        public static readonly string PubCodeKey = "PubCode";
        public static readonly string PubCodeUpperKey = "PUBCODE";
        public static readonly string YesCode = "Y";
        public static readonly string NoCode = "N";

        public static readonly string DefaultColumnHeaders =
            "Additional_,AppNeeded_,CanAppPend_,CleanAuto_,Comment_,ConfirmDat_,DateBounce_,DateHeld_,DateJoined_,DateUnsub_,Domain_,EmailAddr_,ExpireDate_,FullName_,IsListAdm_,List_,MailFormat_,MemberID_,MemberType_,NoRepro_,NotifyErr_,NotifySubm_,NumAppNeed_,NumBounces_,Password_,PermissionGroupID_,RcvAdmMail_,ReadsHtml_,ReceiveAck_,SubType_,UserID_,UserNameLC_,UnsubMessageID_,EnableWYSIWYG_,SyncCanResubscribe_,SyncResubscribeType_,First_Name_,Last_Name_,Age_,Birthday_,Gender_,Interests_,Education_,Marital_Status_,Number_of_Children_,Address_1_,Address_2_,State_Province_,Postal_Code_,Country_,Phone_Home_,Phone_Office_,Occupation_,Income_,Company_Name_,Company_Type_,Company_URL_,Number_Employees_,Product_,Download_Date_,Field_1_,Field_2_,Field_3_,Field_4_,Field_5_,Field_6_,Field_7_,Field_8_,Field_9_,Field_10_,address_3_,fax_,city_,IMS_ProfileCenterPrgms,field_11_,field_12_,field_13_,field_14_,field_15_,field_16_,field_17_,field_18_,field_19_,field_20_,IMSLastUpdate,IMS_Program_OptInDT,IMS_Program_OptOutDT,Ims_unsub_mailingrefcode,Ims_optin_mailingids,Ims_optout_mailingids,Ims_unsub_reason2,Ims_unsub_reason,IMS_memberFlag,IMS_Program01,IMS_Program02,IMS_Program03,IMS_Program04,IMS_Program05,IMS_Program06,IMS_Program07,IMS_Program08,IMS_Program09,IMS_Program10,IMS_Program11,IMS_Program12,IMS_Program13,IMS_Program14,IMS_Program15,IMS_Program16,IMS_Program17,IMS_Program18,IMS_Program19,IMS_Program20,IMS_Program21,IMS_Program22,IMS_Program23,IMS_Program24,IMS_Program25,IMS_Program26,IMS_Program27,IMS_Program28,IMS_Program29,IMS_Program30,Subscriber_ID,profile_updated_once_,IMS_Program31,IMS_Program32,IMS_Program33,IMS_Program34,IMS_Program35,MiddleName,SubPostalCode,LyrisBehaviour_";

        public static readonly string AddressColumnHeaders =
            "cust_num,email_addr,gender,full_name,prefix,first_name,last_name,suffix,formatted_name,employer_name,jobTtl,addr_type,dept,address,mailstop,address3,city,state,country,mbr_lvl,partial_cust_num,noemail,noadvemail,nopubemail,nocdsemail,noconfemail,noawimemail,nopdemail,nostdemail,create_date,modified_date,email_format_pref,bus_phn_num,notelemktg,mbr_yrs_of_service,mbr_status,bpaJobCd,bpaIndCd,majorIndCd,indAssignCd,secondJobCd,techCd,lastLoginDate,PUBCODE,pd_company_quest,pd_industry_quest,pd_primary_job_quest,evt_techCd_quest,email_verify_status,email_verify_date";

        public static readonly string BriefMediaColumnHeaders =
            "Date,Qty,Title,First_Name,Last_Name,Practice/Company_Name,Home/Practice,Address1,City,State,Zip,Phone,Country,Email,Fax,2013_Update,2012_Update,2011_Update,2010_Update,Binder,Spiral";

        public static readonly string MysaeColumnHeaders =
            "cust_num,email_addr,gender,full_name,prefix,first_name,last_name,suffix,formatted_name,employer_name,jobTtl,addr_type,dept,address,mailstop,address3,city,state,country,mbr_lvl,partial_cust_num,noemail,noadvemail,nopubemail,nocdsemail,noconfemail,noawimemail,nopdemail,nostdemail,create_date,modified_date,email_format_pref,bus_phn_num,notelemktg,mbr_yrs_of_service,mbr_status,bpaJobCd,bpaIndCd,majorIndCd,indAssignCd,secondJobCd,techCd,lastLoginDate,PUBCODE,pd_company_quest,pd_industry_quest,pd_primary_job_quest,evt_techCd_quest,email_verify_status,email_verify_date,SNWAE,SNWAU,SNWOH,SNWTNAE,SNWTNALL,SNWTNAU,SNWTNEL,SNWTNHD,SNWTNOF,MAG_SAERO,MAG_SAUTO,MAG_SDIGAUTO,MAG_SOFH,MAG_SSAEUP_DOM";

        public static readonly string PubCodesColumnHeaders =
            "PubCode,GlobalUserKey,Email,LoginDate,UpdateDate,IsLegacy,FirstName,LastName,AccountNbr,Status,Title,Industry,CompanyName,Address,City,State,Country,Zip,SalesVolume,Purchased,DestBooked,Affiliations,Services,JobResp,PlanResp,FacType,HoldOffSite,HoldOffSiteAreas,EstAttendance,PrimBusiness";

        public static readonly string ScanTimeColumnHeaders =
            "Scan Time,Location,Badge,RegClass,FirstName,LastName,Company,Street,Street2,City,State,Zipcode,Country,Phone,Email";
    }
}
