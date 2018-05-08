CREATE PROCEDURE [dbo].[e_HistoryResponseMap_BulkSave]
@xml xml
AS
BEGIN
	
	SET NOCOUNT ON  	

	DECLARE @docHandle int
    DECLARE @insertcount int
    
	DECLARE @import TABLE    
	(  
		PubSubscriptionDetailID int,
		PubSubscriptionID int,
		SubscriptionID int,
		CodeSheetID int,
		IsActive bit,
		DateCreated datetime,
		CreatedByUserID int,
		ResponseOther varchar(300),
		HistorySubscriptionID int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  
	INSERT INTO @import 
	(
		 PubSubscriptionDetailID,PubSubscriptionID,SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID,ResponseOther,HistorySubscriptionID
	)  
	
	SELECT PubSubscriptionDetailID,PubSubscriptionID,SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID,ResponseOther,HistorySubscriptionID
	FROM OPENXML(@docHandle,N'/XML/HistoryResponseMap')
	WITH
	(
		PubSubscriptionDetailID int 'PubSubscriptionDetailID' ,
		PubSubscriptionID int 'PubSubscriptionID' ,
		SubscriptionID int 'SubscriptionID' ,
		CodeSheetID int 'CodeSheetID',
		IsActive bit 'IsActive',
		DateCreated datetime 'DateCreated',
		CreatedByUserID int 'CreatedByUserID',
		ResponseOther varchar(300) 'ResponseOther',
		HistorySubscriptionID int 'HistorySubscriptionID'
	)
	
	EXEC sp_xml_removedocument @docHandle
	
	DECLARE @userLogId TABLE (HistoryResponseMapID int)
	
	INSERT INTO HistoryResponseMap(PubSubscriptionDetailID,PubSubscriptionID,SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID,ResponseOther,HistorySubscriptionID)
	OUTPUT inserted.HistoryResponseMapID INTO @userLogId
	SELECT PubSubscriptionDetailID,PubSubscriptionID,SubscriptionID,CodeSheetID,IsNull(IsActive,1),(CASE WHEN ISNULL(DateCreated,'')='' THEN GETDATE() ELSE DateCreated END) AS DateCreated,CreatedByUserID,ResponseOther,HistorySubscriptionID	
	FROM @import

	SELECT hrm.HistoryResponseMapID,PubSubscriptionDetailID,PubSubscriptionID,SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID,ResponseOther,hrm.HistorySubscriptionID
	FROM HistoryResponseMap hrm with(nolock) 
		INNER JOIN @userLogId u on hrm.HistoryResponseMapID = u.HistoryResponseMapID

END