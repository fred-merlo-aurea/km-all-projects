CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_BulkUpdate]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int

    DECLARE @insertcount int
    
	CREATE TABLE #import
	(  
		SubscriptionID int,
		ResponseID int,
		IsActive bit,
		DateCreated datetime,
		DateUpdated datetime,
		CreatedByUserID int,
		UpdatedByUserID int,
		ResponseOther varchar(300)
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	INSERT INTO #import 
	(
		 SubscriptionID, CodeSheetID, IsActive, DateCreated, DateUpdated,CreatedByUserID, UpdatedByUserID, ResponseOther
	)  
	
	SELECT SubscriptionID,CodeSheetID,IsActive,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ResponseOther
	FROM OPENXML(@docHandle,N'/XML/SubscriptionResponseMap')
	WITH
	(
		SubscriptionID int 'SubscriptionID',
		CodeSheetID int 'CodeSheetID',
		IsActive bit 'IsActive',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		ResponseOther varchar(300) 'ResponseOther'
	)
	
	EXEC sp_xml_removedocument @docHandle

	-- If the record joins, do updates
	UPDATE SubscriptionResponseMap
	SET 
		IsActive = i.IsActive,
		DateUpdated = CASE WHEN ISNULL(i.DateUpdated,'')='' THEN GETDATE() ELSE i.DateUpdated END,
		UpdatedByUserID = i.UpdatedByUserID,
		ResponseOther = i.ResponseOther
	FROM #import i
	WHERE SubscriptionResponseMap.SubscriptionID = i.SubscriptionID AND SubscriptionResponseMap.CodeSheetID = i.CodeSheetID;
	
	INSERT INTO SubscriptionResponseMap (SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther)
	SELECT DISTINCT i.SubscriptionID,i.CodeSheetID,i.IsActive,i.DateCreated,i.CreatedByUserID,i.ResponseOther 
	FROM #import i INNER JOIN SubscriptionResponseMap srm ON srm.SubscriptionID = i.SubscriptionID
	WHERE i.CodeSheetID NOT IN (Select srm.CodeSheetID FROM SubscriptionResponseMap srm INNER JOIN #import i on srm.SubscriptionID = i.SubscriptionID)

	INSERT INTO SubscriptionResponseMap (SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther)
	SELECT DISTINCT i.SubscriptionID,i.CodeSheetID,i.IsActive,i.DateCreated,i.CreatedByUserID,i.ResponseOther 
	FROM #import i
	WHERE i.SubscriptionID NOT IN (SELECT SubscriptionID FROM SubscriptionResponseMap GROUP BY SubscriptionID)
	
	-- Removes records that were marked inactive
	DELETE FROM SubscriptionResponseMap WHERE IsActive = 0 and SubscriptionID IN (SELECT SubscriptionID FROM #import GROUP BY SubscriptionID)

	DROP TABLE #import