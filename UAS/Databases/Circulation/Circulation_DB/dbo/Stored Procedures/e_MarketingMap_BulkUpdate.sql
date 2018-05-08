CREATE PROCEDURE [dbo].[e_MarketingMap_BulkUpdate]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int

    DECLARE @insertcount int
    
	CREATE TABLE #import
	(  
		MarketingID int,
		SubscriberID int,
		PublicationID int,
		IsActive bit,
		DateCreated datetime,
		DateUpdated datetime,
		CreatedByUserID int,
		UpdatedByUserID int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	INSERT INTO #import 
	(
		 MarketingID,SubscriberID, PublicationID, IsActive, DateCreated, DateUpdated,CreatedByUserID, UpdatedByUserID
	)  
	
	SELECT MarketingID,SubscriberID,PublicationID,IsActive,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID
	FROM OPENXML(@docHandle,N'/XML/MarketingMap')
	WITH
	(
		MarketingID  int 'MarketingID',
		SubscriberID int 'SubscriberID',
		PublicationID int 'PublicationID',
		IsActive bit 'IsActive',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID'
	)
	
	EXEC sp_xml_removedocument @docHandle

	-- If the record joins, do updates
	UPDATE MarketingMap
	SET 
		IsActive = i.IsActive,
		DateUpdated = CASE WHEN ISNULL(i.DateUpdated,'')='' THEN GETDATE() ELSE i.DateUpdated END,
		UpdatedByUserID = i.UpdatedByUserID
	FROM #import i
	WHERE MarketingMap.SubscriberID = i.SubscriberID and MarketingMap.MarketingID = i.MarketingID
	
	INSERT INTO MarketingMap (MarketingID,SubscriberID, PublicationID, IsActive, DateCreated,CreatedByUserID)
	SELECT DISTINCT i.MarketingID,i.SubscriberID,i.PublicationID,i.IsActive,i.DateCreated,i.CreatedByUserID
	FROM #import i INNER JOIN MarketingMap srm ON srm.SubscriberID = i.SubscriberID
	WHERE i.MarketingID NOT IN (Select mm.MarketingID FROM MarketingMap mm INNER JOIN #import i on mm.SubscriberID = i.SubscriberID)

	INSERT INTO MarketingMap (MarketingID,SubscriberID, PublicationID, IsActive, DateCreated, CreatedByUserID)
	SELECT DISTINCT i.MarketingID,i.SubscriberID,i.PublicationID,i.IsActive,i.DateCreated,i.CreatedByUserID
	FROM #import i
	WHERE i.SubscriberID NOT IN (SELECT SubscriberID FROM MarketingMap GROUP BY SubscriberID)
	
	-- Removes records that were marked inactive
	DELETE FROM MarketingMap WHERE IsActive = 0 and SubscriberID IN (SELECT SubscriberID FROM #import GROUP BY SubscriberID)

	DROP TABLE #import