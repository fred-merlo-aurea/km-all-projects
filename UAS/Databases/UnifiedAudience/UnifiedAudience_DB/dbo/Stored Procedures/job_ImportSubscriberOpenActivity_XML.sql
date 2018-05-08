--------------------------------------
-- 2014-10-13 MK Added logic to populate SubscriptionId
-- 2015-01-29 Sunil Added code to populate Blasts
--------------------------------------

CREATE PROCEDURE [dbo].[job_ImportSubscriberOpenActivity_XML]
@Xml xml
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @docHandle INT
    DECLARE @insertcount INT

	CREATE TABLE #import    
	(  
		EmailAddress VARCHAR(255), 
		OpenTime DATETIME, 
		PubID INT, 
		BlastID int, 
		Subject varchar(255), 
		SendTime datetime
	)  

	CREATE INDEX EA_1 ON #import (EmailAddress)
	CREATE INDEX EA_2 ON #import (EmailAddress, pubID)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Xml  

	-- IMPORT FROM XML TO TEMP TABLE
	INSERT INTO #import(
		EmailAddress, 
		OpenTime, 
		pubID,  
		BlastID, 
		Subject, 
		sendtime)  
	SELECT EmailAddress, 
		OpenTime, 
		p.PubID, 
		x.blastID, 
		x.Subject, 
		x.sendtime
	FROM
	(
		SELECT EmailAddress, 
			CAST(OpenTime as datetime) as OpenTime, 
			PubCode,  blastID, Subject, sendtime
		FROM OPENXML(@docHandle, N'/XML/OpenActivity')   
			WITH   
			(  
				EmailAddress varchar(255) 'EmailAddress', 
				OpenTime datetime 'OpenTime', 
				PubCode varchar(50) 'PubCode',
				BlastID int 'BlastID',
				Subject varchar(255) 'Subject',
				SendTime datetime 'SendTime'
			) ) x 
			join Pubs p  with (NOLOCK) on x.PubCode = p.pubcode
	
	EXEC sp_xml_removedocument @docHandle		
	
	INSERT INTO Blast (
		BlastID, 
		EmailSubject, 
		SendTime)
	select i.blastID, 
		MAX(i.subject), 
		MAX(i.sendtime)
	from #import i  with (NOLOCK) 
		left outer join Blast b with (NOLOCK) on i.BlastID = b.BlastID
	where i.blastID > 0 
		and b.BlastID is null
	group by i.blastID
	
	INSERT INTO SubscriberOpenActivity(
		PubSubscriptionID, 
		BlastID, 
		ActivityDate,
		SubscriptionId)
	SELECT DISTINCT ps.PubSubscriptionID, 
		BlastID, 
		OpenTime,
		SubscriptionId
	FROM #import i 
		INNER JOIN PubSubscriptions ps WITH (NOLOCK) ON ps.EMAIL = i.EmailAddress and ps.PubID = i.pubID
	
    SET @insertcount = @@ROWCOUNT

	PRINT ('Step 2 check : ' + CAST(@insertcount as nvarchar(20)) )

	DROP TABLE #import
END