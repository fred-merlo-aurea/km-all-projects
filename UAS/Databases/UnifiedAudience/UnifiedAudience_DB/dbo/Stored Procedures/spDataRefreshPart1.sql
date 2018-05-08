CREATE PROC [dbo].[spDataRefreshPart1]
AS
BEGIN

	SET NOCOUNT ON

	
	TRUNCATE TABLE SubscriberTopicActivity 
	TRUNCATE TABLE SubscriberVisitActivity 
	TRUNCATE TABLE SubscriberClickActivity 
	TRUNCATE TABLE SubscriberOpenActivity
	TRUNCATE TABLE Subscriptiondetails
	Delete from SubscriberMasterValues
	delete from SubscriptionsExtension
	dbcc checkident (Subscriptiondetails, 'reseed', 0)
	delete from PubSubscriptionDetail
	dbcc checkident (PubSubscriptionDetail, 'reseed', 0)
	delete from PubSubscriptions
	dbcc checkident (PubSubscriptions, 'reseed', 0)
	TRUNCATE TABLE IncomingDataDetails
	Delete from CampaignFilterDetails
	dbcc checkident (CampaignFilterDetails, 'reseed', 0)
	Delete from CampaignFilter
	dbcc checkident (CampaignFilter, 'reseed', 0)
	Delete from Campaigns
	dbcc checkident (Campaigns, 'reseed', 0)
	Delete from Subscriptions_DQM
	dbcc checkident (Subscriptions_DQM, 'reseed', 0)
	Delete from Subscriptions
	dbcc checkident (Subscriptions, 'reseed', 0)
	--TRUNCATE TABLE igrpdetails
	--TRUNCATE TABLE igrpmastervalues
	

	--ALTER TABLE Subscriptions
	--ADD PubID INT NULL 

	UPDATE incomingdata SET ctry='2' WHERE country= 'canada'
	UPDATE incomingdata SET CTRY = null WHERE CTRY = ','
	UPDATE incomingdata SET XACT = 10 WHERE XACT = 0
	UPDATE incomingdata SET cat = 10 WHERE cat = 0
	UPDATE IncomingData set GENDER = null where GENDER !='M' and GENDER !='F' --change 4/21
	--UPDATE pubs SET enablesearching = 1
	
	
	--update incomingdata set demo31 = 'N' where igrp_no in (select distinct igrp_no from incomingdata where igrp_rank = 'S' and demo31 = 'N') and igrp_rank = 'M' and demo31 = 'Y'
	--update incomingdata set demo32 = 'N' where igrp_no in (select distinct igrp_no from incomingdata where igrp_rank = 'S' and demo32 = 'N') and igrp_rank = 'M' and demo32 = 'Y'
	--update incomingdata set demo33 = 'N' where igrp_no in (select distinct igrp_no from incomingdata where igrp_rank = 'S' and demo33 = 'N') and igrp_rank = 'M' and demo33 = 'Y'
	--update incomingdata set demo34 = 'N' where igrp_no in (select distinct igrp_no from incomingdata where igrp_rank = 'S' and demo34 = 'N') and igrp_rank = 'M' and demo34 = 'Y'
	--update incomingdata set demo35 = 'N' where igrp_no in (select distinct igrp_no from incomingdata where igrp_rank = 'S' and demo35 = 'N') and igrp_rank = 'M' and demo35 = 'Y'
	--update incomingdata set demo36 = 'N' where igrp_no in (select distinct igrp_no from incomingdata where igrp_rank = 'S' and demo36 = 'N') and igrp_rank = 'M' and demo36 = 'Y'

	INSERT INTO [Subscriptions] 
	(	[SEQUENCE], 
		FNAME, 
		LNAME, 
		TITLE, 
		COMPANY, 
		ADDRESS, 
		MAILSTOP,
		Address3, 
		CITY, 
		STATE, 
		ZIP, 
		PLUS4,
		FORZIP,
		COUNTY,
		COUNTRY,
		CountryID,
		PHONE,
		FAX,
		EMAIL,
		CategoryID, 
		TransactionID, 
		TransactionDate,
		QDate, 
		QSourceID,
		PAR3C,
		MailPermission,
		FaxPermission,
		PhonePermission,
		OtherProductsPermission,
		ThirdPartyPermission,
		EmailRenewPermission,
		IGRP_NO, 
		IGRP_CNT,
		emailexists, 
		Faxexists, 
		PhoneExists, 
		gender
	)
	SELECT
		convert(int,[SEQUENCE]) as sequence, 
		SUBSTRING(FNAME, 1, 100),
		SUBSTRING(LNAME, 1, 100),
		SUBSTRING(TITLE, 1, 100),
		SUBSTRING(COMPANY, 1, 100),
		SUBSTRING(ADDRESS, 1, 255),
		SUBSTRING(MAILSTOP, 1, 255),
		SUBSTRING(ADDRESS3, 1, 255),
		SUBSTRING(CITY, 1, 50),
		SUBSTRING(STATE, 1,50),
		SUBSTRING(ZIP, 1,10),
		SUBSTRING(PLUS4, 1, 4),
		SUBSTRING(FORZIP, 1, 50),
		SUBSTRING(COUNTY, 1, 20),
		SUBSTRING(COUNTRY, 1, 100),
		CONVERT(int,CTRY),
		SUBSTRING(PHONE, 1, 100),
		SUBSTRING(FAX, 1, 100),
		SUBSTRING(EMAIL, 1, 100),
		case when CONVERT(INT,CAT) =0  then NULL else CONVERT(INT,CAT) end as CategoryID, 
		CONVERT(INT,XACT) as TransactionID, 
		CONVERT(VARCHAR(10),XACTDATE,101) as TransactionDate, 
		CONVERT(VARCHAR(10),QDATE,101) as QDate, 
		DBO.FN_GetQSourceID(QSOURCE) as QSourceID, 
		SUBSTRING(PAR3C, 1, 1),
		case when Demo31 is null then 1 when Demo31 ='Y' then 1 else 0 end as Demo31, 
		case when DEMO32 is null then 1 when DEMO32 ='Y' then 1 else 0 end as Demo32, 
		case when DEMO33 is null then 1 when DEMO33 ='Y' then 1 else 0 end as Demo33, 
		case when DEMO34 is null then 1 when DEMO34 ='Y' then 1 else 0 end as Demo34, 
		case when DEMO35 is null then 1 when DEMO35 ='Y' then 1 else 0 end as Demo35, 
		case when DEMO36 is null then 1 when DEMO36 ='Y' then 1 else 0 end as Demo36, 
		IGRP_NO, 
		CONVERT(INT,IGRP_CNT) as IGRP_CNT,
		(case when ltrim(rtrim(isnull(email,''))) <> '' then 1 else 0 end),
		(case when ltrim(rtrim(isnull(Fax,''))) <> '' then 1 else 0 end),
		(case when ltrim(rtrim(isnull(PHONE,''))) <> '' then 1 else 0 end),
		substring(gender,1,1)
	FROM incomingdata 
		join pubs p on incomingdata.pubcode = p.PubCode
	WHERE incomingdata.IGRP_RANK = 'M'
	ORDER BY pubID, sequence	
	
	INSERT INTO [Subscriptions_DQM] 
	(	
		SubscriptionID, IGRP_NO, ZZ_PAR_ADDRESS_STD,ZZ_PAR_CITY_STD,ZZ_PAR_COMPANY_MATCH1,ZZ_PAR_COMPANY_MATCH2,ZZ_PAR_COMPANY_STD,ZZ_PAR_COMPANY2,
		ZZ_PAR_EMAIL_STD,ZZ_PAR_FNAME_MATCH1,ZZ_PAR_FNAME_MATCH2,ZZ_PAR_FNAME_MATCH3,ZZ_PAR_FNAME_MATCH4,ZZ_PAR_FNAME_MATCH5,ZZ_PAR_FNAME_MATCH6,
		ZZ_PAR_FNAME_STD,ZZ_PAR_FORZIP_STD,ZZ_PAR_INTL_PHONE,ZZ_PAR_LNAME_STD,ZZ_PAR_MAILSTOP_STD,ZZ_PAR_PLUS4_STD,ZZ_PAR_POBOX,ZZ_PAR_POSTCODE,
		ZZ_PAR_PRIMARY_NUMBER,ZZ_PAR_PRIMARY_POSTFIX,ZZ_PAR_PRIMARY_PREFIX,ZZ_PAR_PRIMARY_STREET,ZZ_PAR_PRIMARY_TYPE,ZZ_PAR_RR_BOX,ZZ_PAR_RR_NUMBER,
		ZZ_PAR_STATE_STD,ZZ_PAR_TITLE_STD,ZZ_PAR_UNIT_DESCRIPTION,ZZ_PAR_UNIT_NUMBER,ZZ_PAR_USCAN_PHONE,ZZ_PAR_ZIP_STD
	)
	SELECT
		s.SubscriptionID, s.IGRP_NO, ZZ_PAR_ADDRESS_STD,ZZ_PAR_CITY_STD,ZZ_PAR_COMPANY_MATCH1,ZZ_PAR_COMPANY_MATCH2,ZZ_PAR_COMPANY_STD,ZZ_PAR_COMPANY2,
		ZZ_PAR_EMAIL_STD,ZZ_PAR_FNAME_MATCH1,ZZ_PAR_FNAME_MATCH2,ZZ_PAR_FNAME_MATCH3,ZZ_PAR_FNAME_MATCH4,ZZ_PAR_FNAME_MATCH5,ZZ_PAR_FNAME_MATCH6,
		ZZ_PAR_FNAME_STD,ZZ_PAR_FORZIP_STD,ZZ_PAR_INTL_PHONE,ZZ_PAR_LNAME_STD,ZZ_PAR_MAILSTOP_STD,ZZ_PAR_PLUS4_STD,ZZ_PAR_POBOX,ZZ_PAR_POSTCODE,
		ZZ_PAR_PRIMARY_NUMBER,ZZ_PAR_PRIMARY_POSTFIX,ZZ_PAR_PRIMARY_PREFIX,ZZ_PAR_PRIMARY_STREET,ZZ_PAR_PRIMARY_TYPE,ZZ_PAR_RR_BOX,ZZ_PAR_RR_NUMBER,
		ZZ_PAR_STATE_STD,ZZ_PAR_TITLE_STD,ZZ_PAR_UNIT_DESCRIPTION,ZZ_PAR_UNIT_NUMBER,ZZ_PAR_USCAN_PHONE,ZZ_PAR_ZIP_STD
	FROM Subscriptions s 
		join incomingdata idata on s.IGRP_NO = idata.IGRP_NO and idata.IGRP_RANK = 'M'
	ORDER BY s.SubscriptionID, s.IGRP_NO
	
	INSERT INTO pubsubscriptions (
		SubscriptionID,
		PubID,
		demo7,
		Qualificationdate,
		PubQSourceID,
		PubCategoryID,
		PubTransactionID,
		EmailStatusID,
		StatusUpdatedDate,
		StatusUpdatedReason,
		Email,
		FirstName,LastName,Company,Title,Address1,Address2,Address3,City,RegionCode,ZipCode,Plus4,County,Country,CountryID,Phone,
		Fax,PubTransactionDate,MailPermission,FaxPermission,PhonePermission,OtherProductsPermission,ThirdPartyPermission,EmailRenewPermission)
	SELECT s.SubscriptionID , 
		p.PubID, 
		case when isnull(i.demo7,'') = '' then 'A' else i.demo7 end, 
		i.QDATE, 
		DBO.FN_GetQSourceID(QSOURCE), 
		case when CONVERT(INT,CAT) =0  then NULL else CONVERT(INT,CAT) end, 
		CONVERT(INT,XACT) as TransactionID,
		case when es.EmailStatusID is null then 1 else es.EmailStatusID end,
		i.QDATE, 
		'',
		i.Email,
		i.FNAME,i.LNAME,i.COMPANY,i.TITLE,i.ADDRESS,i.MAILSTOP,i.address3,
		i.CITY,i.STATE,i.ZIP,i.PLUS4,i.COUNTY,i.COUNTRY,i.CTRY,i.PHONE,
		i.FAX,i.XACTDATE,
		case when i.Demo31 is null then 1 when i.Demo31 ='Y' then 1 else 0 end as Demo31, 
		case when i.DEMO32 is null then 1 when i.DEMO32 ='Y' then 1 else 0 end as Demo32, 
		case when i.DEMO33 is null then 1 when i.DEMO33 ='Y' then 1 else 0 end as Demo33, 
		case when i.DEMO34 is null then 1 when i.DEMO34 ='Y' then 1 else 0 end as Demo34, 
		case when i.DEMO35 is null then 1 when i.DEMO35 ='Y' then 1 else 0 end as Demo35, 
		case when i.DEMO36 is null then 1 when i.DEMO36 ='Y' then 1 else 0 end as Demo36
	FROM Subscriptions s 
		INNER JOIN incomingdata i on s.IGRP_NO = i.IGRP_NO 
		INNER JOIN pubs p on p.pubcode = i.pubcode 
		LEFT OUTER JOIN EmailStatus es on es.Status = i.EMAILSTATUS
	ORDER BY s.SubscriptionID , p.PubID
	
END