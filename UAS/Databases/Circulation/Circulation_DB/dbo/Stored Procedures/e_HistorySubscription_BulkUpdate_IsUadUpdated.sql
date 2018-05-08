
CREATE PROCEDURE [dbo].[e_HistorySubscription_BulkUpdate_IsUadUpdated]
@xml xml
AS

	SET NOCOUNT ON  	

	DECLARE @docHandle int

    DECLARE @insertcount int
    
	CREATE TABLE #import
	(  
		HistorySubscriptionID int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	INSERT INTO #import 
	(
		 HistorySubscriptionID
	)  
	
	SELECT HistorySubscriptionID
	
	FROM OPENXML(@docHandle,N'/XML/HistorySubscriptionID')
	WITH
	(
		HistorySubscriptionID  int 'HistorySubscriptionID'
	)
	
	EXEC sp_xml_removedocument @docHandle

	Update HistorySubscription
	Set IsUadUpdated = 1, UadUpdatedDate = GETDATE()
	Where HistorySubscriptionID in (Select HistorySubscriptionID from #import)

	DROP TABLE #import
