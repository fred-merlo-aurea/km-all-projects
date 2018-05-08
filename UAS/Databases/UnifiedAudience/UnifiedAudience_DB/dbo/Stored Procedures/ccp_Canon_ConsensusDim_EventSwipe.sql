CREATE PROCEDURE ccp_Canon_ConsensusDim_EventSwipe
@xml xml
AS
BEGIN

	SET NOCOUNT ON  
		
	DECLARE @docHandle int
	DECLARE @import TABLE    
	(  
		PubCode varchar(50),RegID int, BoothNumber varchar(50)
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 PubCode, RegID,BoothNumber
	)  
	
	SELECT 
		 PubCode, RegID,BoothNumber
	FROM OPENXML(@docHandle, N'/DocumentElement/RegData')   
	WITH   
	(  
		PubCode varchar(50) 'PubCode',
		RegID int 'RegID',
		BoothNumber varchar(50) 'BoothNumber'
	)  

	EXEC sp_xml_removedocument @docHandle   
	
	---REGID will match to Subscriptions.SEQUENCE field
	DECLARE @FinalXML xml
	SET @FinalXML = (SELECT s.IGRP_NO as 'igroupno',i.PubCode + i.BoothNumber as 'mastervalue' ,i.PubCode + i.BoothNumber as 'masterdesc'
					 FROM @import i
					 JOIN Subscriptions s With(NoLock) ON i.RegID = s.SEQUENCE
					 JOIN PubSubscriptions ps With(NoLock) ON s.SubscriptionID = ps.SubscriptionID 
					 JOIN Pubs p With(NoLock) ON ps.PubID = p.PubID
					 WHERE p.PubCode = i.PubCode 
					 FOR XML RAW ('mastergroup'),ELEMENTS, ROOT('mastergrouplist'))
	
	DECLARE @MasterGroupID int = (SELECT MasterGroupID FROM MasterGroups With(NoLock) WHERE Name = 'Master_PUBBOOTH')
	
	EXEC SP_IMPORT_SUBSCRIBER_MASTERCODESHEET @MasterGroupID, @FinalXML

END	
GO