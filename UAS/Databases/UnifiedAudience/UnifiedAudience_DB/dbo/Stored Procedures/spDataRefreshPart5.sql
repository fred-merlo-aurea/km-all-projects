CREATE proc  [dbo].[spDataRefreshPart5]
as
BEGIN

	SET NOCOUNT ON    
	
	insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
	SELECT  MasterGroupID, [SubscriptionID] , 
		  STUFF((
				SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
				FROM [dbo].[SubscriptionDetails] sd1 
					join Mastercodesheet mc1 on sd1.MasterID = mc1.MasterID  
				WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
				FOR XML PATH (''))
				,1,1,'') AS CombinedValues
	FROM 
		(
		SELECT distinct sd.SubscriptionID, mg.MasterGroupID
		FROM [dbo].[SubscriptionDetails] sd 
			join Mastercodesheet mc on sd.MasterID = mc.MasterID 
			join MasterGroups mg on mg.MasterGroupID = mc.MasterGroupID
		)
	Results
	GROUP BY [SubscriptionID] , MasterGroupID
	order by SubscriptionID
	
	-- Clean UP CR,LF & TAB in TEXT Fields	
	update subscriptions
		set FNAME = REPLACE(REPLACE(REPLACE(FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			LNAME = REPLACE(REPLACE(REPLACE(LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			TITLE = REPLACE(REPLACE(REPLACE(TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			COMPANY = REPLACE(REPLACE(REPLACE(COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			ADDRESS = REPLACE(REPLACE(REPLACE(ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			MAILSTOP = REPLACE(REPLACE(REPLACE(MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			CITY = REPLACE(REPLACE(REPLACE(CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			STATE = REPLACE(REPLACE(REPLACE(STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			ZIP = REPLACE(REPLACE(REPLACE(ZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			PLUS4 = REPLACE(REPLACE(REPLACE(PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			FORZIP = REPLACE(REPLACE(REPLACE(FORZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			COUNTY = REPLACE(REPLACE(REPLACE(COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			COUNTRY = REPLACE(REPLACE(REPLACE(COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			PHONE = REPLACE(REPLACE(REPLACE(PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			FAX = REPLACE(REPLACE(REPLACE(FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			EMAIL = REPLACE(REPLACE(REPLACE(EMAIL, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			REGCODE = REPLACE(REPLACE(REPLACE(REGCODE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			VERIFIED = REPLACE(REPLACE(REPLACE(VERIFIED, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			SUBSRC = REPLACE(REPLACE(REPLACE(SUBSRC, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			ORIGSSRC = REPLACE(REPLACE(REPLACE(ORIGSSRC, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			PAR3C = REPLACE(REPLACE(REPLACE(PAR3C, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			SOURCE = REPLACE(REPLACE(REPLACE(SOURCE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			PRIORITY = REPLACE(REPLACE(REPLACE(PRIORITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			SIC = REPLACE(REPLACE(REPLACE(SIC, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			SICCODE = REPLACE(REPLACE(REPLACE(SICCODE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Gender = REPLACE(REPLACE(REPLACE(Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			IGRP_RANK = REPLACE(REPLACE(REPLACE(IGRP_RANK, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			CGRP_RANK = REPLACE(REPLACE(REPLACE(CGRP_RANK, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			ADDRESS3 = REPLACE(REPLACE(REPLACE(ADDRESS3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			HOME_WORK_ADDRESS = REPLACE(REPLACE(REPLACE(HOME_WORK_ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			PubIDs = REPLACE(REPLACE(REPLACE(PubIDs, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Demo7 = REPLACE(REPLACE(REPLACE(Demo7, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			mobile = REPLACE(REPLACE(REPLACE(mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')
	
	update SubscriptionsExtension
		set Field1 = REPLACE(REPLACE(REPLACE(Field1, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field2 = REPLACE(REPLACE(REPLACE(Field2, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field3 = REPLACE(REPLACE(REPLACE(Field3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field4 = REPLACE(REPLACE(REPLACE(Field4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field5 = REPLACE(REPLACE(REPLACE(Field5, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field6 = REPLACE(REPLACE(REPLACE(Field6, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field7 = REPLACE(REPLACE(REPLACE(Field7, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field8 = REPLACE(REPLACE(REPLACE(Field8, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field9 = REPLACE(REPLACE(REPLACE(Field9, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field10 = REPLACE(REPLACE(REPLACE(Field10, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field11 = REPLACE(REPLACE(REPLACE(Field11, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field12 = REPLACE(REPLACE(REPLACE(Field12, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field13 = REPLACE(REPLACE(REPLACE(Field13, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field14 = REPLACE(REPLACE(REPLACE(Field14, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field15 = REPLACE(REPLACE(REPLACE(Field15, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field16 = REPLACE(REPLACE(REPLACE(Field16, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field17 = REPLACE(REPLACE(REPLACE(Field17, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field18 = REPLACE(REPLACE(REPLACE(Field18, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field19 = REPLACE(REPLACE(REPLACE(Field19, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field20 = REPLACE(REPLACE(REPLACE(Field20, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field21 = REPLACE(REPLACE(REPLACE(Field21, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field22 = REPLACE(REPLACE(REPLACE(Field22, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field23 = REPLACE(REPLACE(REPLACE(Field23, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field24 = REPLACE(REPLACE(REPLACE(Field24, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field25 = REPLACE(REPLACE(REPLACE(Field25, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field26 = REPLACE(REPLACE(REPLACE(Field26, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field27 = REPLACE(REPLACE(REPLACE(Field27, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field28 = REPLACE(REPLACE(REPLACE(Field28, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field29 = REPLACE(REPLACE(REPLACE(Field29, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field30 = REPLACE(REPLACE(REPLACE(Field30, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field31 = REPLACE(REPLACE(REPLACE(Field31, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field32 = REPLACE(REPLACE(REPLACE(Field32, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field33 = REPLACE(REPLACE(REPLACE(Field33, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field34 = REPLACE(REPLACE(REPLACE(Field34, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field35 = REPLACE(REPLACE(REPLACE(Field35, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field36 = REPLACE(REPLACE(REPLACE(Field36, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field37 = REPLACE(REPLACE(REPLACE(Field37, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field38 = REPLACE(REPLACE(REPLACE(Field38, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field39 = REPLACE(REPLACE(REPLACE(Field39, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field40 = REPLACE(REPLACE(REPLACE(Field40, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field41 = REPLACE(REPLACE(REPLACE(Field41, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field42 = REPLACE(REPLACE(REPLACE(Field42, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field43 = REPLACE(REPLACE(REPLACE(Field43, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field44 = REPLACE(REPLACE(REPLACE(Field44, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field45 = REPLACE(REPLACE(REPLACE(Field45, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field46 = REPLACE(REPLACE(REPLACE(Field46, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field47 = REPLACE(REPLACE(REPLACE(Field47, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field48 = REPLACE(REPLACE(REPLACE(Field48, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field49 = REPLACE(REPLACE(REPLACE(Field49, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Field50 = REPLACE(REPLACE(REPLACE(Field50, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') 


	update PubSubscriptions
		set demo7 = REPLACE(REPLACE(REPLACE(demo7, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			StatusUpdatedReason = REPLACE(REPLACE(REPLACE(StatusUpdatedReason, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Email = REPLACE(REPLACE(REPLACE(Email, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			AccountNumber = REPLACE(REPLACE(REPLACE(AccountNumber, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			IMBSEQ = REPLACE(REPLACE(REPLACE(IMBSEQ, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			MemberGroup = REPLACE(REPLACE(REPLACE(MemberGroup, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			OnBehalfOf = REPLACE(REPLACE(REPLACE(OnBehalfOf, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			OrigsSrc = REPLACE(REPLACE(REPLACE(OrigsSrc, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Status = REPLACE(REPLACE(REPLACE(Status, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			SubscriberSourceCode = REPLACE(REPLACE(REPLACE(SubscriberSourceCode, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Verify = REPLACE(REPLACE(REPLACE(Verify, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			FirstName = REPLACE(REPLACE(REPLACE(FirstName, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			LastName = REPLACE(REPLACE(REPLACE(LastName, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Company = REPLACE(REPLACE(REPLACE(Company, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Title = REPLACE(REPLACE(REPLACE(Title, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Occupation = REPLACE(REPLACE(REPLACE(Occupation, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Address1 = REPLACE(REPLACE(REPLACE(Address1, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Address2 = REPLACE(REPLACE(REPLACE(Address2, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Address3 = REPLACE(REPLACE(REPLACE(Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			City = REPLACE(REPLACE(REPLACE(City, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			RegionCode = REPLACE(REPLACE(REPLACE(RegionCode, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			ZipCode = REPLACE(REPLACE(REPLACE(ZipCode, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Plus4 = REPLACE(REPLACE(REPLACE(Plus4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			CarrierRoute = REPLACE(REPLACE(REPLACE(CarrierRoute, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			County = REPLACE(REPLACE(REPLACE(County, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Country = REPLACE(REPLACE(REPLACE(Country, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			AddressValidationSource = REPLACE(REPLACE(REPLACE(AddressValidationSource, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			AddressValidationMessage = REPLACE(REPLACE(REPLACE(AddressValidationMessage, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Phone = REPLACE(REPLACE(REPLACE(Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Fax = REPLACE(REPLACE(REPLACE(Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Mobile = REPLACE(REPLACE(REPLACE(Mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Website = REPLACE(REPLACE(REPLACE(Website, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Income = REPLACE(REPLACE(REPLACE(Income, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			Gender = REPLACE(REPLACE(REPLACE(Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ,
			PhoneExt = REPLACE(REPLACE(REPLACE(PhoneExt, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')

End