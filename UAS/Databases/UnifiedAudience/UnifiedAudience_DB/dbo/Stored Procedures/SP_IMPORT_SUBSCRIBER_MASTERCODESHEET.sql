CREATE PROCEDURE [dbo].[SP_IMPORT_SUBSCRIBER_MASTERCODESHEET]
	@MasterGroupID INT,
	@importXML TEXT,
	@UserID int
AS
BEGIN	
		
	DECLARE @docHandle INT		

	SET NOCOUNT ON

	CREATE TABLE #tmpData (
		IGROUPNO UNIQUEIDENTIFIER, 
		mastervalue VARCHAR(100),
		masterdesc VARCHAR(255), 
		CONSTRAINT [PK_TMPDATA] PRIMARY KEY (IGROUPNO, MASTERVALUE, MASTERDESC))

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @importXML  

	INSERT INTO #tmpData (
		IGROUPNO, 
		MASTERVALUE, 
		MASTERDESC
		)  
	SELECT 	DISTINCT IGROUPNO, 
		MASTERVALUE,
		MASTERDESC
	FROM OPENXML(@docHandle, N'/mastergrouplist/mastergroup')   
	WITH   
		(  
		IGROUPNO VARCHAR(255) 'igroupno', 
		MASTERVALUE VARCHAR(50) 'mastervalue', 
		MASTERDESC VARCHAR(255) 'masterdesc'
		) 

	EXEC sp_xml_removedocument @docHandle  

	SELECT * 
	FROM #tmpData

	INSERT INTO Mastercodesheet (
		MasterValue,
		MasterDesc,
		MasterGroupID,
		MasterDesc1,
		EnableSearching,
		SortOrder,
		DateCreated,
		CreatedByUserID)
	SELECT DISTINCT 
		MASTERVALUE, 
		MASTERDESC, 
		@MasterGroupID, 
		NULL, 
		0, 
		NULL,
		GETDATE(),
		@UserID
	FROM #tmpData
	WHERE MASTERVALUE NOT IN (SELECT MC.MASTERVALUE FROM Mastercodesheet MC WHERE mastergroupID = @MasterGroupID)

	DECLARE @Subscriber table (SUBSCRIPTIONID int, IGROUPNO UNIQUEIDENTIFIER, MASTERID INT)

	INSERT INTO @Subscriber
	SELECT DISTINCT  
		s.SubscriptionID, 
		s.IGRP_NO, 
		masterID 
	FROM subscriptions s 
		JOIN #tmpData t ON t.IGROUPNO = s.igrp_no 
		JOIN Mastercodesheet ms ON t.MASTERVALUE = ms.MASTERVALUE
	WHERE MasterGroupID = @MasterGroupID

	INSERT INTO SubscriptionDetails
	SELECT S.subscriptionID, 
		S.masterID 
	FROM @Subscriber S 
		LEFT OUTER JOIN SubscriptionDetails SD ON S.subscriptionID = SD.subscriptionID AND S.MASTERID = SD.MASTERID
	WHERE SD.MASTERID IS NULL

	DROP TABLE #tmpData

	DECLARE @SubscriptionID int,
			@mvalue VARCHAR(1000)
				
	DECLARE c_subs CURSOR FOR SELECT DISTINCT SUBSCRIPTIONID FROM @Subscriber

	OPEN c_subs  
	FETCH NEXT FROM c_subs INTO @SubscriptionID

	WHILE @@FETCH_STATUS = 0  
	BEGIN  

		SET @mvalue = ''

		SELECT @mvalue = STUFF((
					SELECT DISTINCT ',' + mastervalue 
					FROM SubscriptionDetails sd 
						JOIN mastercodesheet m ON sd.masterID = m.masterID 
					WHERE sd.SubscriptionID = @SubscriptionID 
						AND m.MasterGroupID = @MasterGroupID
					ORDER BY ',' + mastervalue
					FOR XML PATH('')),1, 1, '')
	                		
		IF EXISTS (SELECT TOP 1 subscriptionID FROM SubscriberMasterValues WHERE SubscriptionID = @SubscriptionID AND MasterGroupID = @MasterGroupID )
			BEGIN
				UPDATE 
					SubscriberMasterValues
				SET [MastercodesheetValues] = @mvalue
				WHERE SubscriptionID = @SubscriptionID 
					AND MasterGroupID = @MasterGroupID
			END
		ELSE
			BEGIN
				INSERT INTO SubscriberMasterValues (
					MasterGroupID,
					SubscriptionID,
					[MastercodesheetValues]) 
				VALUES (
					@MasterGroupID, 
					@SubscriptionID, 
					@mvalue	)
			END		
		FETCH NEXT FROM c_subs INTO @SubscriptionID

	END
	
	CLOSE c_subs  
	DEALLOCATE c_subs 
		
END

--SP_IMPORT_SUBSCRIBER_MASTERCODESHEET @MasterGroupID = 1, '<MasterGroupList><MasterGroup><IGROUPNO>1</IGROUPNO><MASTERVALUE>MV1</MASTERVALUE><MASTERDESC>MD1</MASTERDESC><IGROUPNO>1</IGROUPNO><MASTERVALUE>MV2</MASTERVALUE><MASTERDESC>MD2</MASTERDESC></MasterGroup>
--<MasterGroup><IGROUPNO>1</IGROUPNO><MASTERVALUE>MV1</MASTERVALUE><MASTERDESC>MD1</MASTERDESC><IGROUPNO>1</IGROUPNO><MASTERVALUE>MV2</MASTERVALUE><MASTERDESC>MD2</MASTERDESC></MasterGroup>
--<MasterGroup><IGROUPNO>2</IGROUPNO><MASTERVALUE>MV2</MASTERVALUE><MASTERDESC>MD2</MASTERDESC></MasterGroup>
--<MasterGroup><IGROUPNO>6</IGROUPNO><MASTERVALUE>MV3</MASTERVALUE><MASTERDESC>MD3</MASTERDESC></MasterGroup></MasterGroupList>'
GO