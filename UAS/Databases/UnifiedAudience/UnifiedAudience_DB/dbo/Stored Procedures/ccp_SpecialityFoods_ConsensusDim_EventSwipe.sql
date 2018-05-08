CREATE PROCEDURE ccp_SpecialityFoods_ConsensusDim_EventSwipe
@xml xml
AS
BEGIN
	SET NOCOUNT ON  	
	DECLARE @docHandle int
	DECLARE @import TABLE    
	(  
		Booth int, Exh int, SubProdCatID int, RegNumb int
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 Booth, Exh, SubProdCatID, RegNumb
	)  
	
	SELECT 
		 Booth, Exh, SubProdCatID, RegNumb
	FROM OPENXML(@docHandle, N'/DocumentElement/RegData')   
	WITH   
	(  
		Booth int 'Booth',
		Exh int 'Exh',
		SubProdCatID int 'SubProdCatID',
		RegNumb int 'RegNumb'
	)  

	EXEC sp_xml_removedocument @docHandle   
	
	---RegNumb will match to Subscriptions.SEQUENCE field
	--SubProdCatID = Master_DirListCat
	--Booth = Master_Booth
	--Exh = Master_Exhnumb
	
	DECLARE @BoothXML xml
	SET @BoothXML = (SELECT DISTINCT s.IGRP_NO as 'igroupno', i.Booth as 'mastervalue' ,i.Booth as 'masterdesc'
					 FROM @import i
					 JOIN Subscriptions s With(NoLock) ON i.RegNumb = s.SEQUENCE
					 FOR XML RAW ('mastergroup'),ELEMENTS, ROOT('mastergrouplist'))
	
	DECLARE @BoothMasterGroupID int = (SELECT MasterGroupID FROM MasterGroups With(NoLock) WHERE Name = 'Master_Booth')
	
	EXEC SP_IMPORT_SUBSCRIBER_MASTERCODESHEET @BoothMasterGroupID, @BoothXML
	
	
	DECLARE @ExhXML xml
	SET @ExhXML = (SELECT DISTINCT s.IGRP_NO as 'igroupno', i.Exh as 'mastervalue' ,i.Exh as 'masterdesc'
					 FROM @import i
					 JOIN Subscriptions s With(NoLock) ON i.RegNumb = s.SEQUENCE
					 FOR XML RAW ('mastergroup'),ELEMENTS, ROOT('mastergrouplist'))
	
	DECLARE @ExhMasterGroupID int = (SELECT MasterGroupID FROM MasterGroups With(NoLock) WHERE Name = 'Master_Exhnumb')
	
	EXEC SP_IMPORT_SUBSCRIBER_MASTERCODESHEET @ExhMasterGroupID, @ExhXML
	
	
	DECLARE @SubProdCatIDXML xml
	SET @SubProdCatIDXML = (SELECT DISTINCT s.IGRP_NO as 'igroupno', i.SubProdCatID as 'mastervalue' ,i.SubProdCatID as 'masterdesc'
					 FROM @import i
					 JOIN Subscriptions s With(NoLock) ON i.RegNumb = s.SEQUENCE
					 FOR XML RAW ('mastergroup'),ELEMENTS, ROOT('mastergrouplist'))
	
	DECLARE @SubProdCatIDMasterGroupID int = (SELECT MasterGroupID FROM MasterGroups With(NoLock) WHERE Name = 'Master_DirListCat')
	
	EXEC SP_IMPORT_SUBSCRIBER_MASTERCODESHEET @SubProdCatIDMasterGroupID, @SubProdCatIDXML

END
GO