CREATE PROCEDURE [dbo].[job_ImportSubscriberVisitActivity_XML]
@Xml xml
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @docHandle int

    declare @insertcount int
    set @insertcount = 0
    
	CREATE TABLE #import  
	(  
		tmpImportID int IDENTITY(1,1), 
		EmailAddress varchar(255), 
		Visittime datetime, 
		URL varchar(255),
		DomainName varchar(200),
		DomainTrackingID int
	)  

	CREATE INDEX EA_1 on #import (EmailAddress)
	CREATE INDEX EA_2 on #import (DomainName)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into #import (
		 EmailAddress, 
		 Visittime, 
		 URL,
		 domainname, DomainTrackingID)  
	select EmailAddress, Visittime, URL, domainname, 0
	from
	(
		SELECT EmailAddress, 
			Visittime, 
			substring (URL,1,255) as URL,
			substring (REPLACE(REPLACE(URL, 'https://',''), 'http://',''),1, (case when CHARINDEX('/', REPLACE(REPLACE(URL, 'https://',''), 'http://','')) > 0 then (CHARINDEX('/', REPLACE(REPLACE(URL, 'https://',''), 'http://','')) - 1) else 200 end)) as domainname
		FROM OPENXML(@docHandle, N'/XML/Visit')   
		WITH   
		(  
			EmailAddress  varchar(255) 'EmailAddress', 
			Visittime datetime 'Visittime',
			URL varchar(255) 'URL'
		) 
	) x
	where LEN(emailaddress) > 0 and domainname like '%.%.%'
	
	EXEC sp_xml_removedocument @docHandle    
	
	
	if exists  (select top 1 1 from #import)
	Begin
		INSERT INTO DomainTracking (DomainName)
		select distinct i.DomainName
		from #import i  with (NOLOCK) 
			left outer join DomainTracking  dt with (NOLOCK) on i.DomainName = dt.DomainName
		where dt.DomainTrackingID is null
			
		Update i
		set DomainTrackingID = dt.domaintrackingID
		from #import i 
			join DomainTracking dt on dt.DomainName = i.DomainName
		
		INSERT INTO SubscriberVisitActivity (
			SubscriptionID, 
			DomainTrackingID, 
			URL, 
			ActivityDate)
		SELECT DISTINCT ps.SubscriptionID, 
			DomainTrackingID, 
			URL, 
			Visittime
		FROM #import i 
			INNER JOIN PubSubscriptions ps WITH (NOLOCK) ON ps.EMAIL = i.EmailAddress
		Where i.DomainTrackingID > 0
		
		set @insertcount = @@ROWCOUNT
	End
	
	print ('Step 2 check : '  + CAST(@insertcount as nvarchar(20)) )
	
	DROP TABLE #import

END
GO