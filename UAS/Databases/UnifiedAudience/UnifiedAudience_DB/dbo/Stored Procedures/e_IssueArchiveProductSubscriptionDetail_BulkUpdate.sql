CREATE PROCEDURE [dbo].[e_IssueArchiveProductSubscriptionDetail_BulkUpdate]
@xml xml
AS
BEGIN

	SET NOCOUNT ON	

	DECLARE @docHandle int

    DECLARE @insertcount int
    
	CREATE TABLE #import
	(  
		IssueArchiveSubscriptionId int,
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
		 IssueArchiveSubscriptionId,PubSubscriptionID,SubscriptionID, CodeSheetID, DateCreated, DateUpdated,CreatedByUserID, UpdatedByUserID, ResponseOther
	)  
	
	SELECT IssueArchiveSubscriptionId,PubSubscriptionID,SubscriptionID,CodeSheetID,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ResponseOther
	FROM OPENXML(@docHandle,N'/XML/IssueArchiveProductSubscriptionDetail')
	WITH
	(
		IssueArchiveSubscriptionId int 'IssueArchiveSubscriptionId',
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
	

	DELETE iapsd 
	FROM IssueArchiveProductSubscriptionDetail iapsd
		join #import i on iapsd.IssueArchiveSubscriptionId = i.IssueArchiveSubscriptionId

	INSERT INTO IssueArchiveProductSubscriptionDetail(IssueArchiveSubscriptionId,PubSubscriptionID,SubscriptionID,CodeSheetID,DateCreated,CreatedByUserID,ResponseOther)
	SELECT i.IssueArchiveSubscriptionId,i.PubSubscriptionID,i.SubscriptionID,i.CodeSheetID,i.DateCreated,i.CreatedByUserID,i.ResponseOther 
	FROM #import i		

	
	SELECT DISTINCT iapsd.* 
	FROM IssueArchiveProductSubscriptionDetail iapsd
		JOIN #import i ON i.IssueArchiveSubscriptionId = iapsd.IssueArchiveSubscriptionId 

	DROP TABLE #import

END
