CREATE PROCEDURE job_SYNC_recent_CIRC_data_to_Subscriptions
	@ProcessCode varchar(50),
	@Demo31OverRide bit = 'false',
	@Demo32OverRide bit = 'false',
	@Demo33OverRide bit = 'false',
	@Demo34OverRide bit = 'false',
	@Demo35OverRide bit = 'false',
	@Demo36OverRide bit = 'false'
as
BEGIN

	SET NOCOUNT ON
	
	Create table #tbl1 (SubscriptionID int, PubSubscriptionID int, UpdateAddressinSubscriptions bit DEFAULT 0, ResetGeoCodesinSubscriptions bit Default 0)

	CREATE CLUSTERED INDEX IDX_C_PubSubscriptions ON #tbl1(PubSubscriptionID)

	insert into #tbl1 (SubscriptionID, PubSubscriptionID, UpdateAddressinSubscriptions)
	select	
			ps.SubscriptionID,
			ps.PubSubscriptionID,
			(CASE WHEN ISNULL(ps.Address1,'')!='' AND ISNULL(ps.City,'')!='' AND ISNULL(ps.RegionCode,'')!='' AND ISNULL(ps.ZipCode,'')!='' THEN 1 ELSE 0 END)
	from
	(			
		select  ps.PubsubscriptionID, 
				Row_Number() over (partition by ps.SubscriptionID order by isnull(ps.DateUpdated, ps.DateCreated) desc) as rownumber
		from SubscriberFinal sf WITH (nolock) 
			join Subscriptions s WITH (nolock) on sf.IGRP_NO = s.IGRP_NO 
			join PubSubscriptions ps with (NOLOCK) on S.SubscriptionID = ps.SubscriptionID 
			join Pubs p on p.PubID = ps.PubID 
		where sf.ProcessCode = @ProcessCode and  
			isUpdatedinLIVE = 1  and 
			p.IsCirc = 1	
	) x 
		join PubSubscriptions ps on ps.PubsubscriptionID = x.PubsubscriptionID and x.rownumber = 1
	order by ps.SubscriptionID
	
	update t
	set ResetGeoCodesinSubscriptions = 1
	From PubSubscriptions ps 
		join #tbl1 t on ps.SubscriptionID = t.SubscriptionID and ps.PubSubscriptionID = t.PubSubscriptionID
        join Subscriptions s on s.SubscriptionID = ps.SubscriptionID
	Where UpdateAddressinSubscriptions = 1 and
		(
			ISNULL(ps.address1,'') <> ISNULL(s.address,'') or 
			ISNULL(ps.City,'') <> ISNULL(s.CITY,'') or 
			ISNULL(ps.RegionCode,'') <> ISNULL(s.STATE,'') or 
			ISNULL(ps.ZipCode,'') <>  ISNULL(s.ZIP,'') or 
			ISNULL(ps.CountryID,0) <> ISNULL(s.CountryID,0)
		)
			
	Update S
		Set
			[SEQUENCE]  = convert(int,isnull(ps.[SequenceID], 0)),
			FNAME       = (CASE WHEN ISNULL(ps.FirstName,'')!='' AND ISNULL(ps.LastName,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.FirstName, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FNAME END),
			LNAME       = (CASE WHEN ISNULL(ps.FirstName,'')!='' AND ISNULL(ps.LastName,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.LastName, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.LName END),
			TITLE       = (CASE WHEN ISNULL(ps.TITLE,'')!='' THEN left(REPLACE(REPLACE(REPLACE(ps.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),100) ELSE s.TITLE END),
			COMPANY     = (CASE WHEN ISNULL(ps.COMPANY,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.COMPANY END),
			ADDRESS     = (CASE WHEN	t.UpdateAddressinSubscriptions = 1 THEN left(REPLACE(REPLACE(REPLACE(ps.Address1, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),255) ELSE s.ADDRESS END),
			MAILSTOP    = (CASE WHEN t.UpdateAddressinSubscriptions = 1 THEN left(REPLACE(REPLACE(REPLACE(ps.Address2, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),255) ELSE s.MAILSTOP END),
			ADDRESS3 = (CASE WHEN t.UpdateAddressinSubscriptions = 1 THEN left(REPLACE(REPLACE(REPLACE(ps.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),255) ELSE s.ADDRESS3 END),               
			CITY        = (CASE WHEN t.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.CITY END),
			STATE       = (CASE WHEN t.UpdateAddressinSubscriptions = 1 THEN LEFT(REPLACE(REPLACE(REPLACE(ps.RegionCode, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),50) ELSE s.STATE END),
			ZIP         = (CASE WHEN t.UpdateAddressinSubscriptions = 1 THEN LEFT(REPLACE(REPLACE(REPLACE(ps.ZipCode, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),10) ELSE s.ZIP END),
			PLUS4       = (CASE WHEN t.UpdateAddressinSubscriptions = 1 THEN left(REPLACE(REPLACE(REPLACE(ps.PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),4) ELSE s.PLUS4 END),
			COUNTY      = (CASE WHEN t.UpdateAddressinSubscriptions = 1 THEN left(REPLACE(REPLACE(REPLACE(ps.COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 20) ELSE s.COUNTY END),
			COUNTRY     = (CASE WHEN t.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.COUNTRY END),
			CountryID   = (CASE WHEN t.UpdateAddressinSubscriptions = 1 THEN ps.CountryID ELSE s.CountryID END),
			Latitude    =   (CASE WHEN t.ResetGeoCodesinSubscriptions = 1 THEN NULL	ELSE s.Latitude END),
			Longitude   =   (CASE WHEN t.ResetGeoCodesinSubscriptions = 1 THEN NULL	ELSE s.Longitude END),
			IsLatLonValid = (CASE WHEN t.ResetGeoCodesinSubscriptions = 1 THEN 0		ELSE s.IsLatLonValid END),
			LatLonMsg =	   (CASE WHEN t.ResetGeoCodesinSubscriptions = 1 THEN ''	ELSE s.LatLonMsg END),
			PHONE       = (CASE WHEN ISNULL(ps.Phone,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.PHONE END),
			FAX         = (CASE WHEN ISNULL(ps.FAX,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FAX END),
			MOBILE      = (CASE WHEN ISNULL(ps.MOBILE,'')!='' THEN left(REPLACE(REPLACE(REPLACE(ps.MOBILE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 30) ELSE s.mobile END),
			CategoryID  = ps.PubCategoryID,
			TransactionID = ps.PubTransactionID,
			TransactionDate = ISNULL(ps.dateupdated, ps.DateCreated),
			QDate       =  ps.Qualificationdate,
			Email       = (CASE WHEN ISNULL(ps.EMail,'')!='' THEN ps.Email ELSE s.EMAIL END),
			Demo7       =  case when isnull(ps.demo7,'') = '' then 'A' else ps.demo7 end,
			QSourceID   = case when isnull(ps.PubQSourceID, -1) > 0 then ps.PubQSourceID else S.QSourceID end,
			PAR3C       =  ps.Par3CID,
			emailexists = (case when (ltrim(rtrim(isnull(ps.email,''))) <> '' or ltrim(rtrim(isnull(s.email,''))) <> '')  then 1 else 0 end),  
			Faxexists   = (case when ltrim(rtrim(isnull(ps.Fax,''))) <> '' then 1 else 0 end), 
			PhoneExists = (case when ltrim(rtrim(isnull(ps.PHONE,''))) <> '' then 1 else 0 end),
			Gender = (CASE WHEN ISNULL(ps.Gender,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.Gender END),
			--IsMailable = ps.IsMailable,
			SubSrc = left(ps.SubscriberSourceCode,25),
			OrigsSrc = left(ps.OrigsSrc,25),
			DateUpdated = GETDATE()
		From PubSubscriptions ps 
			join #tbl1 t on ps.SubscriptionID = t.SubscriptionID and ps.PubSubscriptionID = t.PubSubscriptionID
            join Subscriptions s on s.SubscriptionID = ps.SubscriptionID

	drop table #tbl1
	
End
go