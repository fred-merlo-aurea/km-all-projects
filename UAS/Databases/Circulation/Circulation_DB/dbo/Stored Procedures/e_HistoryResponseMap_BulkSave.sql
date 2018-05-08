
CREATE PROCEDURE [dbo].[e_HistoryResponseMap_BulkSave]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int
    DECLARE @insertcount int
    
	DECLARE @import TABLE    
	(  
		SubscriptionID int,
		ResponseID int,
		IsActive bit,
		DateCreated datetime,
		CreatedByUserID int,
		ResponseOther varchar(300)
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  
	INSERT INTO @import 
	(
		 SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther
	)  
	
	SELECT SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther
	FROM OPENXML(@docHandle,N'/XML/HistoryResponseMap')
	WITH
	(
		SubscriptionID int 'SubscriptionID' ,
		ResponseID int 'ResponseID',
		IsActive bit 'IsActive',
		DateCreated datetime 'DateCreated',
		CreatedByUserID int 'CreatedByUserID',
		ResponseOther varchar(300) 'ResponseOther'
	)
	
	EXEC sp_xml_removedocument @docHandle
	
	DECLARE @userLogId TABLE (HistoryResponseMapID int)
	
	INSERT INTO HistoryResponseMap(SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther)
	OUTPUT inserted.HistoryResponseMapID INTO @userLogId
	SELECT SubscriptionID,ResponseID,IsActive,(CASE WHEN ISNULL(DateCreated,'')='' THEN GETDATE() ELSE DateCreated END) AS DateCreated,CreatedByUserID,ResponseOther	
	FROM @import

	SELECT hrm.HistoryResponseMapID,SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther
	FROM HistoryResponseMap hrm INNER JOIN @userLogId u on hrm.HistoryResponseMapID = u.HistoryResponseMapID


