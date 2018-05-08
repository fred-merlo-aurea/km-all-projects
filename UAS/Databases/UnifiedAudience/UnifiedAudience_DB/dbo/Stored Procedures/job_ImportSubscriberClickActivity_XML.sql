--------------------------------------
-- 2014-10-13 MK Added logic to populate SubscriptionId
-- 2015-01-29 Sunil Added code to populate Blasts
--------------------------------------

CREATE PROCEDURE [dbo].[job_ImportSubscriberClickActivity_XML]
@Xml xml
AS
BEGIN	
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	CREATE TABLE #import  
	(  
		tmpImportID int IDENTITY(1,1), 
		EmailAddress varchar(255), 
		ClickTime datetime, 
		PubCode nvarchar(50), 
		PubID int, 
		ClickURL varchar(255), 
		Alias varchar(100), 
		BlastID int, 
		Subject varchar(255), 
		SendTime datetime
	)  

	CREATE INDEX EA_1 on #import (EmailAddress)
	CREATE INDEX EA_2 on #import (EmailAddress, PubCode)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into #import (
		 EmailAddress, 
		 PubCode, 
		 PubID, 
		 ClickTime,
		 ClickURL, 
		 Alias,  
		 BlastID, 
		 Subject, 
		 sendtime)  
	select 
		x.EmailAddress, 
		x.PubCode, 
		p.pubID, 
		x.ClickTime,
		x.ClickURL, 
		x.Alias, 
		x.blastID, 
		x.Subject, 
		x.sendtime
	from
	(
		SELECT 
			EmailAddress, 
			PubCode, 
			ClickTime, 
			ClickURL, 
			Alias,  
			blastID, 
			Subject, 
			sendtime
		FROM OPENXML(@docHandle, N'/XML/ClickActivity')   
		WITH   
		(  
			EmailAddress  varchar(255) 'EmailAddress', 
			PubCode varchar(50) 'PubCode', 
			ClickTime datetime 'ClickTime',
			ClickURL varchar(255) 'ClickURL',
			Alias varchar(100) 'Alias',
			BlastID int 'BlastID',
			Subject varchar(255) 'Subject',
			SendTime datetime 'SendTime'
		)  
	) x join Pubs p  with (NOLOCK) on x.PubCode = p.pubcode
	
	EXEC sp_xml_removedocument @docHandle    
	
	INSERT INTO Blast (
		BlastID, 
		EmailSubject, 
		SendTime)
	select 
		i.blastID, 
		MAX(i.subject), 
		MAX(i.sendtime)
	from 
		#import i  with (NOLOCK) 
		left outer join Blast b with (NOLOCK) on i.BlastID = b.BlastID
	where 
		i.blastID > 0 
		and b.BlastID is null
	group by 
		i.blastID
	
	INSERT INTO SubscriberClickActivity (
		PubSubscriptionID, 
		BlastID, 
		Link, 
		LinkAlias, 
		LinkSource, 
		LinkType, 
		ActivityDate,
		SubscriptionId)
	SELECT DISTINCT 
		ps.PubSubscriptionID, 
		BlastID, 
		ClickURL, 
		i.Alias, 
		null, 
		null, 
		ClickTime,
		SubscriptionId
	FROM 
		#import i 
		INNER JOIN PubSubscriptions ps WITH (NOLOCK) ON ps.EMAIL = i.EmailAddress AND ps.PubID = i.pubID

	set @insertcount = @@ROWCOUNT

	print ('Step 2 check : '  + CAST(@insertcount as nvarchar(20)) )
	
	DROP TABLE #import
END