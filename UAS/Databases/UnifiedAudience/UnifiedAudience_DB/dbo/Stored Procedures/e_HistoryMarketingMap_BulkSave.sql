CREATE PROCEDURE [dbo].[e_HistoryMarketingMap_BulkSave]
@xml xml
AS
BEGIN

	SET NOCOUNT ON  	

	DECLARE @docHandle int
    DECLARE @insertcount int
    
	DECLARE @import TABLE    
	(  
		MarketingID int,
		PubSubscriptionID int,
		PublicationID int,
		IsActive bit,
		DateCreated datetime,
		CreatedByUserID int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  
	INSERT INTO @import 
	(
		 MarketingID,PubSubscriptionID,PublicationID,IsActive,DateCreated,CreatedByUserID
	)  
	
	SELECT MarketingID,PubSubscriptionID,PublicationID,IsActive,DateCreated,CreatedByUserID
	FROM OPENXML(@docHandle,N'/XML/HistoryMarketingMap')
	WITH
	(
		MarketingID int 'MarketingID',
		PubSubscriptionID int 'PubSubscriptionID',
		PublicationID int 'PublicationID',
		IsActive bit 'IsActive',
		DateCreated datetime 'DateCreated',
		CreatedByUserID int 'CreatedByUserID'
	)
	
	EXEC sp_xml_removedocument @docHandle
	
	DECLARE @userLogId TABLE (HistoryMarketingMapID int)
	
	INSERT INTO HistoryMarketingMap(MarketingID,PubSubscriptionID,PublicationID,IsActive,DateCreated,CreatedByUserID)
	OUTPUT inserted.HistoryMarketingMapID INTO @userLogId
	SELECT MarketingID,PubSubscriptionID,PublicationID,IsActive,(CASE WHEN ISNULL(DateCreated,'')='' THEN GETDATE() ELSE DateCreated END) AS DateCreated,CreatedByUserID
	FROM @import
	SELECT hmm.HistoryMarketingMapID,MarketingID,PubSubscriptionID,PublicationID,IsActive,DateCreated,CreatedByUserID
	FROM HistoryMarketingMap hmm with(nolock) 
		INNER JOIN @userLogId u on hmm.HistoryMarketingMapID = u.HistoryMarketingMapID

END