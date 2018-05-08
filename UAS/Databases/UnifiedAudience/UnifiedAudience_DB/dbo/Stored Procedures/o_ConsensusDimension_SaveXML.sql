CREATE PROCEDURE o_ConsensusDimension_SaveXML
@XML xml,
@MasterGroupID int
AS
BEGIN

	SET NOCOUNT ON  	
	DECLARE @docHandle int
    DECLARE @insertcount int
    
	CREATE TABLE #importCD    
	(  
		FirstName varchar(100),LastName varchar(100), Company varchar(100),Address1 varchar(255),Address2 varchar(255),City varchar(50),State varchar(50),Zipcode varchar(50),
		Country varchar(100),Phone varchar(100),Email varchar(100),ActivityDescription varchar(255),ActivityResponse varchar(100),IGrp_No UNIQUEIDENTIFIER null
	)  
	CREATE INDEX idx_cdALL ON #importCD(FirstName,LastName,Company,Address1,Address2,City,State,Zipcode,Country,Phone,Email)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @XML  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into #importCD 
	(
		 FirstName,LastName,Company,Address1,Address2,City,State,Zipcode,Country,Phone,Email,ActivityDescription,ActivityResponse
	)  
	
	SELECT LEFT(FirstName,3),LEFT(LastName,6),Company,LEFT(Address1,15),Address2,City,State,Zipcode,Country,Phone,Email,ActivityDescription,ActivityResponse
	FROM OPENXML(@docHandle, N'/XML/ConsensusDimension')   
	WITH   
	(  
		FirstName varchar(100) 'FirstName',
		LastName varchar(100) 'LastName', 
		Company varchar(100) 'Company',
		Address1 varchar(255) 'Address1',
		Address2 varchar(255) 'Address2',
		City varchar(50) 'City',
		State varchar(50) 'State',
		Zipcode varchar(50) 'Zipcode',
		Country varchar(100) 'Country',
		Phone varchar(100) 'Phone',
		Email varchar(100) 'Email',
		ActivityDescription varchar(255) 'ActivityDescription',
		ActivityResponse varchar(100) 'ActivityResponse'
	)  

	EXEC sp_xml_removedocument @docHandle 
	
	---------------------------------------------------------------------
	UPDATE i
	SET IGrp_No = s.igrp_no
	FROM #importCD i 
		INNER JOIN Subscriptions s WITH(NoLock) ON  i.FirstName = LEFT(s.fname,3) 
													AND i.LastName = LEFT(s.LName,6) 
													AND i.Address1 = LEFT(s.[Address],15) 
													AND i.Zipcode = s.Zip  
	WHERE i.IGrp_No IS NULL
	---------------------------------------------------------------------
	UPDATE i
	SET IGrp_No = s.igrp_no
	FROM #importCD i 
		INNER JOIN PubSubscriptions ps WITH(NoLock) ON i.Email = ps.email
		INNER JOIN Subscriptions s WITH(NoLock) ON  ps.SubscriptionId = s.SubscriptionId 
													AND i.FirstName = LEFT(s.fname,3) 
													AND i.LastName = LEFT(s.lname,6)
	WHERE i.IGrp_No IS NULL
	---------------------------------------------------------------------
	UPDATE i
	SET IGrp_No = s.igrp_no
	FROM #importCD i 
		INNER JOIN Subscriptions s WITH(NoLock) ON  i.FirstName = LEFT(s.fname,3) 
													AND i.LastName = LEFT(s.LName,6) 
													AND i.Company = LEFT(s.Company,8)
	WHERE i.IGrp_No IS NULL
	---------------------------------------------------------------------
	UPDATE i
	SET IGrp_No = s.igrp_no
	FROM #importCD i 
		INNER JOIN Subscriptions s WITH(NoLock) ON  i.FirstName = LEFT(s.fname,3) 
													AND i.LastName = LEFT(s.LName,6) 
													AND i.Phone = s.Phone
	WHERE i.IGrp_No IS NULL
	---------------------------------------------------------------------
	UPDATE i
	SET IGrp_No = s.igrp_no
	FROM #importCD i 
		INNER JOIN PubSubscriptions ps WITH(NoLock) ON i.Email = ps.email
		INNER JOIN Subscriptions s WITH(NoLock) ON ps.SubscriptionId = s.SubscriptionId 
	WHERE i.IGrp_No IS NULL
	---------------------------------------------------------------------
	
	-----***/ Send to other sproc now /***--------
	DECLARE @FinalXML xml
	SET @FinalXML = (SELECT i.IGrp_No as 'igroupno', i.ActivityResponse as 'mastervalue', i.ActivityDescription as 'masterdesc',m.MasterGroupID 
					 FROM #importCD i With(NoLock)
						JOIN Mastercodesheet m With(NoLock) ON i.ActivityDescription = m.MasterDesc AND i.ActivityResponse = m.MasterValue 
					 WHERE i.IGrp_No IS NOT NULL AND m.MasterGroupID = @MasterGroupID
					 FOR XML RAW ('mastergroup'),ELEMENTS, ROOT('mastergrouplist'))		 
	
	EXEC SP_IMPORT_SUBSCRIBER_MASTERCODESHEET @MasterGroupID, @FinalXML

END
GO