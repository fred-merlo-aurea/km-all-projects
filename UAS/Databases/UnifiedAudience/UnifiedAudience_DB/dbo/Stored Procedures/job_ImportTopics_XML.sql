--------------------------------------
-- 2014-10-13 MK 
--		Added logic to populate SubscriberTopicActivity
--		Updated Email Joins to Use Pubsubscription
-- 2015-05-19 MK
--		Defined column list for insert into PubSubscriptionDetail 
--------------------------------------

CREATE PROCEDURE [dbo].[job_ImportTopics_XML]
@Xml xml
AS
BEGIN

	SET NOCOUNT ON
 	

	DECLARE @docHandle int

	CREATE TABLE #import    
	(  
		tmpImportID INT IDENTITY(1,1), 
		EmailAddress VARCHAR(255),
		ActivityDatetime DATETIME, 
		PubID INT, 
		TopicCode VARCHAR(50)
	)  

	CREATE INDEX EA_1 ON #import (EmailAddress)
	CREATE INDEX EA_2 ON #import (EmailAddress, PubID, TopicCode)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Xml  

	-- IMPORT FROM XML TO TEMP TABLE
	
	
	INSERT INTO #import (
		EmailAddress, 
		ActivityDatetime, 
		PubID, 
		TopicCode)  
	SELECT X.EmailAddress, 
		CAST(X.ActivityDatetime AS DATETIME), 
		p.PubID, 
		x.TopicCode
	FROM
	(
		SELECT  EmailAddress, 
			ActivityDatetime, 
			PubCode, 
			TopicCode, 
			TopicCodeText
		FROM OPENXML(@docHandle, N'/XML/Topic')   
			WITH   
			(  
				EmailAddress VARCHAR(255) 'EmailAddress', 
				ActivityDatetime DATETIME 'ActivityDatetime', 
				PubCode VARCHAR(50) 'PubCode', 
				TopicCode VARCHAR(50) 'TopicCode', 
				TopicCodeText VARCHAR(500) 'TopicCodeText'
			)
		) x 
		join Pubs p on x.PubCode = p.pubcode
	 
	EXEC sp_xml_removedocument @docHandle    
	 
    DECLARE @insertcount VARCHAR(50)
    SET @insertcount = (SELECT COUNT(*) FROM #import)

	PRINT ('Import Count: '  + @insertcount )

	
	INSERT INTO PubSubscriptionDetail (
		PubSubscriptionID,
		SubscriptionID,
		CodesheetID
		)
	SELECT DISTINCT
		ps.PubSubscriptionID ,
		ps.SubscriptionID,
		cs.CodeSheetID
	FROM PubSubscriptions ps WITH (NOLOCK)
		JOIN #import i WITH (NOLOCK) on i.emailaddress = ps.EMAIL and i.pubID = ps.PubID 
		JOIN dbo.responsegroups rg  WITH (NOLOCK)on rg.pubID = i.pubID 
		JOIN CodeSheet cs WITH (NOLOCK)ON cs.PubID = i.pubID AND cs.ResponseGroupID  = rg.ResponseGroupID and cs.Responsevalue = i.TopicCode
		LEFT JOIN PubSubscriptionDetail pds WITH(NOLOCK) ON pds.PubSubscriptionID = ps.PubSubscriptionID and pds.CodesheetID = cs.CodeSheetID
	WHERE ResponseGroupName = 'TOPICS'
		AND pds.pubsubscriptiondetailid IS NULL

	INSERT INTO SubscriptionDetails(
		SubscriptionID, 
		MasterID)
	SELECT DISTINCT
		ps.SubscriptionID,
		cmb.MasterID
	FROM PubSubscriptions ps WITH (NOLOCK)
		JOIN #import i WITH (NOLOCK) on i.emailaddress = ps.EMAIL and i.pubID = ps.PubID 
        JOIN dbo.responsegroups rg  WITH (NOLOCK)on rg.pubID = i.pubID 
        JOIN CodeSheet cs WITH (NOLOCK)ON cs.PubID = i.pubID AND cs.ResponseGroupID  = rg.ResponseGroupID and cs.Responsevalue = i.TopicCode
        JOIN CodeSheet_Mastercodesheet_Bridge cmb WITH(NOLOCK) ON cmb.CodeSheetID= cs.CodeSheetID
        LEFT JOIN SubscriptionDetails sd WITH(NOLOCK) ON sd.SubscriptionID = ps.SubscriptionID AND sd.MasterID = cmb.MasterID
	WHERE rg.ResponseGroupName = 'TOPICS'
        AND sd.sdID IS NULL


	INSERT INTO SubscriberTopicActivity ( 
		PubsubScriptionID,
		TopicCode,
		ActivityDate,
		SubscriptionID) 

	SELECT DISTINCT ps.PubsubScriptionID,
		MasterDesc,
		CONVERT(DATE,i.ActivityDateTime) AS ActivityDate,
		SubscriptionID
	FROM PubSubscriptions ps WITH (NOLOCK)
		JOIN Pubs p WITH (NOLOCK) ON p.PubID = ps.PubID
		JOIN #import i WITH (NOLOCK) on i.emailaddress = ps.EMAIL and i.PubID = ps.PubID
		JOIN Mastercodesheet mc on i.TopicCode = mc.MasterValue
	ORDER BY ActivityDate

	--execute your sproc on each row
	EXEC Usp_MergeSubscriberMasterValues 'topics'
	
	DROP TABLE #import

END