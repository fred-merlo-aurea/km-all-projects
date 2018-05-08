CREATE PROCEDURE [dbo].[e_HistoryToHistoryResponse_BulkSave]
@xml xml
AS
BEGIN

	SET NOCOUNT ON  	

	DECLARE @docHandle int
    DECLARE @insertcount int
    
	DECLARE @import TABLE    
	(  
		HistoryID int,
		HistoryResponseID int
	)
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  
	INSERT INTO @import 
	(
		 HistoryID,HistoryResponseID
	)  
	
	SELECT HistoryID,HistoryResponseID
	FROM OPENXML(@docHandle,N'/XML/History')
	WITH
	(
		HistoryID int 'HistoryID' ,
		HistoryResponseID int 'HistoryResponseID'
	)
	
	EXEC sp_xml_removedocument @docHandle

	INSERT INTO HistoryToHistoryResponse (HistoryID,HistoryResponseID)
	SELECT HistoryID,HistoryResponseID FROM @import

END