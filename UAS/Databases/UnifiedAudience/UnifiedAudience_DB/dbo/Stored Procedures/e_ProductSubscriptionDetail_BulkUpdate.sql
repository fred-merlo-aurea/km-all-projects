CREATE PROCEDURE [dbo].[e_ProductSubscriptionDetail_BulkUpdate]
@xml xml
AS
BEGIN

	SET NOCOUNT ON	

	DECLARE @docHandle int

    DECLARE @insertcount int
    
	CREATE TABLE #import
	(  
		PubSubscriptionID int,
		SubscriptionID int,
		CodeSheetID int,
		DateCreated datetime,
		DateUpdated datetime,
		CreatedByUserID int,
		UpdatedByUserID int,
		ResponseOther varchar(300)
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	INSERT INTO #import 
	(
		 PubSubscriptionID,SubscriptionID, CodeSheetID, DateCreated, DateUpdated,CreatedByUserID, UpdatedByUserID, ResponseOther
	)  
	
	SELECT PubSubscriptionID,SubscriptionID,CodeSheetID,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ResponseOther
	FROM OPENXML(@docHandle,N'/XML/ProductSubscriptionDetail')
	WITH
	(
		PubSubscriptionID int 'PubSubscriptionID',
		SubscriptionID int 'SubscriptionID',
		CodeSheetID int 'CodeSheetID',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		ResponseOther varchar(300) 'ResponseOther'
	)
	
	EXEC sp_xml_removedocument @docHandle

	-- If the record joins, do updates
	--UPDATE SubscriptionResponseMap
	--SET 
	--	DateUpdated = CASE WHEN ISNULL(i.DateUpdated,'')='' THEN GETDATE() ELSE i.DateUpdated END,
	--	UpdatedByUserID = i.UpdatedByUserID,
	--	ResponseOther = i.ResponseOther
	--FROM #import i
	--WHERE SubscriptionResponseMap.SubscriptionID = i.SubscriptionID AND SubscriptionResponseMap.CodeSheetID = i.CodeSheetID;
	
	--INSERT INTO PubSubscriptionDetail (SubscriptionID,CodeSheetID,DateCreated,CreatedByUserID,ResponseOther)
	--SELECT DISTINCT i.SubscriptionID,i.CodeSheetID,i.DateCreated,i.CreatedByUserID,i.ResponseOther 
	--FROM #import i INNER JOIN PubSubscriptionDetail srm ON srm.SubscriptionID = i.SubscriptionID
	--WHERE i.CodeSheetID NOT IN (Select srm.CodeSheetID FROM SubscriptionResponseMap srm INNER JOIN #import i on srm.SubscriptionID = i.SubscriptionID)

	DELETE ps 
	FROM PubSubscriptionDetail ps 
		join #import i on ps.PubSubscriptionID = i.PubSubscriptionID

	INSERT INTO PubSubscriptionDetail(PubSubscriptionID,SubscriptionID,CodeSheetID,DateCreated,CreatedByUserID,ResponseOther)
	SELECT i.PubSubscriptionID,i.SubscriptionID,i.CodeSheetID,i.DateCreated,i.CreatedByUserID, case when LTRIM(RTRIM(i.ResponseOther)) = '' then null else i.ResponseOther end
	FROM #import i
	
	
	-- Removes records that were marked inactive
	--DELETE FROM SubscriptionResponseMap WHERE IsActive = 0 and SubscriptionID IN (SELECT SubscriptionID FROM #import GROUP BY SubscriptionID)
	
	-- Rebuild Consensus
	
	declare @subscriptionID int
	set @subscriptionID = (SELECT DISTINCT SubscriptionID FROM #import)

	delete sd
	from SubscriptionDetails sd
		join CodeSheet_Mastercodesheet_Bridge cmb on sd.MasterID = cmb.MasterID 
	where sd.SubscriptionID = @subscriptionID
	                        
	delete 
	from SubscriberMasterValues
	where SubscriptionID = @subscriptionID

	insert into SubscriptionDetails (SubscriptionID, MasterID)
		select distinct psd.SubscriptionID, cmb.masterID 
		from PubSubscriptionDetail psd with (NOLOCK) 
			join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
		where psd.SubscriptionID = @subscriptionID
	        
	insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
	SELECT MasterGroupID, [SubscriptionID] , 
		STUFF((
			SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
			FROM [dbo].[SubscriptionDetails] sd1  with (NOLOCK)  join Mastercodesheet mc1  with (NOLOCK) on sd1.MasterID = mc1.MasterID  
			WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
			FOR XML PATH ('')),1,1,''
		) AS CombinedValues
	FROM 
		(
			SELECT distinct sd.SubscriptionID, mc.MasterGroupID
			FROM [dbo].[SubscriptionDetails] sd  with (NOLOCK) 
				join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID                    
			where sd.SubscriptionID = @subscriptionID
		)
	Results
	GROUP BY [SubscriptionID] , MasterGroupID
	order by SubscriptionID    

	
	SELECT DISTINCT p.* 
	FROM PubSubscriptionDetail p
		JOIN #import i ON i.PubSubscriptionID = p.PubSubscriptionID 

	DROP TABLE #import

END